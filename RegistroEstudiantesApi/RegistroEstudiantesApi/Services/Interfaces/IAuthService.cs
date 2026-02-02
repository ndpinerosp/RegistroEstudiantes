using RegistroEstudiantes.Server.DTOs;

namespace RegistroEstudiantes.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<(bool Success, string Message)> UpdateProfileAsync(string userId, UpdateUserDto dto);
        Task<(bool Success, string Message)> DeleteAccountAsync(string userId);
    }
}
