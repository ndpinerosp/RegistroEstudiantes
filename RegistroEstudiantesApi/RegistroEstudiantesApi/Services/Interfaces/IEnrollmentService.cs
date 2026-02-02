using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Server.DTOs;

namespace RegistroEstudiantes.Server.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<MyDashboardDto> GetStudentDashboardAsync(int studentId);
        Task<bool> EnrollStudentAsync(int studentId, int courseId);
        Task<bool> UnenrollStudentAsync(int studentId, int courseId);
        Task<IEnumerable<StudentProgressDto>> GetPublicDataAsync();

    }
}
