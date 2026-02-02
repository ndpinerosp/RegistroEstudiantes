using RegistroEstudiantes.Server.DTOs;

namespace RegistroEstudiantes.Server.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();
        Task<CourseResponseDto?> GetCourseByIdAsync(int id);
        Task<IEnumerable<string>> GetClassmatesAsync(int courseId);
    }
}
