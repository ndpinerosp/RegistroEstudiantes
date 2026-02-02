using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllWithProfessorAsync();
        Task<Course?> GetByIdWithDetailsAsync(int id);
        Task<List<string>> GetClassmatesAsync(int courseId);
    }
}
