using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Course>> GetAllWithProfessorAsync()
        {
            return await _context.Courses
                .Include(c => c.Professor)
                .ToListAsync();
        }
        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<string>> GetClassmatesAsync(int courseId)
        {
            return await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.Student.Name + " " + e.Student.LastName)
                .ToListAsync();
        }

    }
}
