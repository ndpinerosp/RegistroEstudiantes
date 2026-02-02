using Microsoft.AspNetCore.Mvc;
using Moq;
using RegistroEstudiantes.Server.Controllers;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroEstudiantesApi.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }
        #region Register
        [Fact]
        public async Task RegisterExitoso()
        {
            var dto = new RegisterDto("Juan", "Perez","test@hotmail.com","Password123!");

            _authServiceMock.Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync((true, "Usuario creado"));

            var result = await _controller.Register(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Register_EmailExist()
        {

            var dto = new RegisterDto("Juan", "Perez", "test@hotmail.com", "Password123!");
            string mensajeError = "El correo ya está en uso";

            _authServiceMock
                .Setup(s => s.RegisterAsync(dto))
                .ReturnsAsync((false, mensajeError));

            var result = await _controller.Register(dto);


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            Assert.NotNull(badRequestResult.Value);

            _authServiceMock.Verify(s => s.RegisterAsync(dto), Times.Once);
        }
        #endregion

        #region Login
        [Fact]
        public async Task Login_OK()
        {

            var dto = new LoginDto("andres@hotmail.com", "Password123!");
            var loginResponse = new AuthResponseDto { Token = "token-fallido", Email = dto.Email };

            _authServiceMock.Setup(s => s.LoginAsync(dto))
                .ReturnsAsync(loginResponse);

            var result = await _controller.Login(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(loginResponse, okResult.Value);
        }

        [Fact]
        public async Task Login_Failed()
        {

            var dto = new LoginDto("error@test.com", "password123");

            _authServiceMock.Setup(s => s.LoginAsync(dto))
                .ReturnsAsync((AuthResponseDto)null);

            var result = await _controller.Login(dto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var message = unauthorizedResult.Value.GetType().GetProperty("message")?.GetValue(unauthorizedResult.Value, null);
            Assert.Equal("Usuario o contraseña incorrectos. Por favor, inténtelo de nuevo.", message);
        }
        #endregion
    }
}
