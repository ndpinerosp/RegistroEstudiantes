using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Data.Repositories;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services.Interfaces;

namespace RegistroEstudiantes.Server.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly ICourseRepository _courseRepo;

        public EnrollmentService(IEnrollmentRepository enrollmentRepo, ICourseRepository courseRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _courseRepo = courseRepo;
        }

        public async Task<MyDashboardDto> GetStudentDashboardAsync(int studentId)
        {
            var enrollments = await _enrollmentRepo.GetByStudentIdAsync(studentId);
            var myCoursesDto = enrollments
                .Select(e => new CourseResponseDto(
                    e.Course.Id,
                    e.Course.Name,
                    e.Course.Credits,
                    e.Course.ProfessorId,
                    e.Course.Professor.Name ?? "Sin asignar",
                    e.Course.Enrollments
                        .Where(other => other.StudentId != studentId)
                        .Select(other => other.Student.Name + " " + other.Student.LastName)
                        .ToList()
                )).ToList();

            return new MyDashboardDto(
                TotalCredits: myCoursesDto.Sum(c => c.Credits),
                EnrolledCoursesCount: myCoursesDto.Count,
                MyCourses: myCoursesDto
            );
        }

        public async Task<bool> EnrollStudentAsync(int studentId, int courseId)
        {

            var course = await _courseRepo.GetByIdWithDetailsAsync(courseId);
            if (course == null) throw new KeyNotFoundException("La materia no existe.");

            var currentEnrollments = await _enrollmentRepo.GetByStudentIdAsync(studentId);

            // Máximo 3 materias
            if (currentEnrollments.Count >= 3)
                throw new InvalidOperationException("Límite de 3 materias alcanzado.");

            if (currentEnrollments.Any(e => e.CourseId == courseId))
                throw new InvalidOperationException("Ya estás inscrito en esta materia.");

            // No repetir profesor
            if (currentEnrollments.Any(e => e.Course.ProfessorId == course.ProfessorId))
                throw new InvalidOperationException("Ya tienes una materia inscrita con este profesor.");

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _enrollmentRepo.AddAsync(enrollment);
            return await _enrollmentRepo.SaveChangesAsync();
        }

        public async Task<bool> UnenrollStudentAsync(int studentId, int courseId)
        {
            var enrollment = await _enrollmentRepo.GetSpecificAsync(studentId, courseId);

            if (enrollment == null) return false;

            _enrollmentRepo.Delete(enrollment);
            return await _enrollmentRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentProgressDto>> GetPublicDataAsync()
        {
            // ista pública de registros
            return await _enrollmentRepo.GetAllDataAsync();
        }

    }
}
