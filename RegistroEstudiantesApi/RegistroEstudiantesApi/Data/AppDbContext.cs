using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Professor> Professors => Set<Professor>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    }
}
