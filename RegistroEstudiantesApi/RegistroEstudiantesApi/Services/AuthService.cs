using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Data.Repositories;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services.Interfaces;


namespace RegistroEstudiantes.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ITokenService _tokenService;
        public AuthService(IStudentRepository studentRepo, ITokenService tokenService)
        {
            _studentRepo = studentRepo;
            _tokenService = tokenService;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            var existing = await _studentRepo.EmailExistsAsync(dto.Email);
            if (existing) return (false, "El correo electrónico ya está en uso.");

            var student = new Student
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveChangesAsync();

            return (true, "Estudiante registrado correctamente.");
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var student = await _studentRepo.GetByEmailAsync(dto.Email);
            if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
                return null;

            var token = _tokenService.CreateToken(student);

            return new AuthResponseDto
            {
                Token = token,
                StudentName = student.Name,
                StudentLastName = student.LastName,
                Email = student.Email
            };

        }

        public async Task<(bool Success, string Message)> UpdateProfileAsync(string userId, UpdateUserDto dto)
        {
            if (!int.TryParse(userId, out int id))
                return (false, "ID de usuario no válido.");

            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) return (false, "Usuario no encontrado.");

            if (student.Email != dto.Email && await _studentRepo.EmailExistsAsync(dto.Email))
            {
                return (false, "el nuevo correo electrónico ya está en uso.");
            }

            student.Name = dto.Name;
            student.LastName = dto.LastName;
            student.Email = dto.Email;


            _studentRepo.Update(student);
            await _studentRepo.SaveChangesAsync();

            return (true, "Perfil actualizado correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAccountAsync(string userId)
        {
            if (!int.TryParse(userId, out int id))
                return (false, "ID de usuario no válido.");

            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) return (false, "El usuario no existe.");

            _studentRepo.Delete(student);
            await _studentRepo.SaveChangesAsync();

            return (true, "Cuenta eliminada correctamente.");
        }
    }


}
