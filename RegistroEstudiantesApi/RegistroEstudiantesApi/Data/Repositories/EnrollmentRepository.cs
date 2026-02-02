using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Professor)
                .Include(e => e.Course.Enrollments)
                    .ThenInclude(oe => oe.Student)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Enrollment?> GetSpecificAsync(int studentId, int courseId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<IEnumerable<StudentProgressDto>> GetAllDataAsync()
        {
            return await _context.Enrollments
                .GroupBy(e => new { e.Student.Name, e.Student.LastName, e.Student.Email })
                .Select(g => new StudentProgressDto(
                    $"{g.Key.Name} {g.Key.LastName}",
                    g.Key.Email
                ))
                .ToListAsync();
        }

        public async Task AddAsync(Enrollment enrollment) => await _context.Enrollments.AddAsync(enrollment);

        public void Delete(Enrollment enrollment) => _context.Enrollments.Remove(enrollment);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
