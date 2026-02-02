using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Data.Repositories;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services.Interfaces;

namespace RegistroEstudiantes.Server.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;

        public CourseService(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllWithProfessorAsync();
            return courses.Select(c => new CourseResponseDto(
                c.Id,
                c.Name,
                c.Credits,
                c.ProfessorId,
                c.Professor?.Name ?? "Sin asignar",
                new List<string>()
            ));
        }

        public async Task<CourseResponseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepo.GetByIdWithDetailsAsync(id);
            if (course == null) return null;

            return new CourseResponseDto(
                course.Id,
                course.Name,
                course.Credits,
                course.ProfessorId,
                course.Professor?.Name ?? "Sin asignar",
                course.Enrollments.Select(e => e.Student.Name).ToList()
            );
        }

        public async Task<IEnumerable<string>> GetClassmatesAsync(int courseId)
        {
            return await _courseRepo.GetClassmatesAsync(courseId);
        }

    }
}
