using Moq;
using RegistroEstudiantes.Server.Data.Repositories;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services;
using RegistroEstudiantes.Server.Services.Interfaces;

namespace RegistroEstudiantesApi.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _studentRepoMock = new Mock<IStudentRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _authService = new AuthService(
                _studentRepoMock.Object,
                _tokenServiceMock.Object
            );
        }

        #region RegisterAsync

        [Fact]
        public async Task RegisterAsync_Exitoso()
        {
            var dto = new RegisterDto ("Juan", "Pérez", "juan@test.com", "123456");

            _studentRepoMock
                .Setup(r => r.EmailExistsAsync(dto.Email))
                .ReturnsAsync(false);

            var result = await _authService.RegisterAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("Estudiante registrado correctamente.", result.Message);

            _studentRepoMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Once);
            _studentRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region LoginAsync

        [Fact]
        public async Task LoginAsync_Valid()
        {
            var password = "123456";
            var student = new Student
            {
                Id = 1,
                Name = "Ana",
                LastName = "Gómez",
                Email = "ana@hotmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _studentRepoMock
                .Setup(r => r.GetByEmailAsync(student.Email))
                .ReturnsAsync(student);

            _tokenServiceMock
                .Setup(t => t.CreateToken(student))
                .Returns("fake-jwt-token");

            var result = await _authService.LoginAsync(new LoginDto
            (student.Email, password)
            );

            Assert.NotNull(result);
            Assert.Equal("fake-jwt-token", result!.Token);
            Assert.Equal(student.Name, result.StudentName);
            Assert.Equal(student.LastName, result.StudentLastName);
            Assert.Equal(student.Email, result.Email);
        }



        [Fact]
        public async Task LoginAsync_NotExist()
        {
            _studentRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Student?)null);

            var result = await _authService.LoginAsync(new LoginDto
            ("noexiste@gmail.com", "123456"));

            Assert.Null(result);
        }

        #endregion

        #region DeleteAccountAsync

        [Fact]
        public async Task DeleteAccountAsync_UserExists()
        {
            var student = new Student { Id = 1 };

            _studentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(student);

            var result = await _authService.DeleteAccountAsync("1");

            Assert.True(result.Success);
            Assert.Equal("Cuenta eliminada correctamente.", result.Message);

            _studentRepoMock.Verify(r => r.Delete(student), Times.Once);
            _studentRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountAsync_UserDoesNotExist()
        {
            _studentRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Student?)null);

            var result = await _authService.DeleteAccountAsync("1");

            Assert.False(result.Success);
            Assert.Equal("El usuario no existe.", result.Message);
        }

        #endregion
    }
}
