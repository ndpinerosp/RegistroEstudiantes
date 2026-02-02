using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByEmailAsync(string email);
        Task<Student?> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task SaveChangesAsync();
    }
}
