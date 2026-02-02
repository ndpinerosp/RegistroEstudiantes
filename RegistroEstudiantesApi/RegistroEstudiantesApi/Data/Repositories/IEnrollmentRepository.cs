using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Data.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetByStudentIdAsync(int studentId);
        Task<Enrollment?> GetSpecificAsync(int studentId, int courseId);
        Task<IEnumerable<StudentProgressDto>> GetAllDataAsync();
        Task AddAsync(Enrollment enrollment);
        void Delete(Enrollment enrollment);
        Task<bool> SaveChangesAsync();
    }
}
