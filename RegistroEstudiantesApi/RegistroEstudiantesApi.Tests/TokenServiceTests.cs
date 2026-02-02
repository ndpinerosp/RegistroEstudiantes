using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit;

namespace RegistroEstudiantes.Tests
{
    public class TokenServiceTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _configMock = new Mock<IConfiguration>();

            // simulacion de appsettings.json
            _configMock.Setup(x => x["Jwt:Key"]).Returns("esta_es_una_llave_super_secreta_y_larga_para_que_el_algoritmo_no_falle_12345");
            _configMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

            _tokenService = new TokenService(_configMock.Object);
        }

        [Fact]
        public void CreateToken_ValidJwt()
        {

            var student = new Student
            {
                Id = 1,
                Email = "test@hotmail.com",
                Name = "andres",
                LastName = "Garcia"
            };

            var token = _tokenService.CreateToken(student);

            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            Assert.Equal("TestIssuer", jsonToken.Issuer);
            Assert.Contains(jsonToken.Claims, c => c.Type == "email" && c.Value == student.Email);
            Assert.Contains(jsonToken.Claims, c => c.Type == "nameid" && c.Value == student.Id.ToString());
        }
        [Fact]
        public void CreateToken_ValidationIsExpired()
        {

            var student = new Student { Id = 1, Email = "test@test.com", Name = "Andres" };

            var token = _tokenService.CreateToken(student);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var expirationTime = jsonToken.ValidTo;
            var expectedExpiration = DateTime.UtcNow.AddMinutes(30);

            Assert.True(expirationTime > DateTime.UtcNow);
            Assert.True(expirationTime <= expectedExpiration.AddSeconds(10));
            Assert.True(expirationTime >= expectedExpiration.AddSeconds(-10));
        }
    }
}