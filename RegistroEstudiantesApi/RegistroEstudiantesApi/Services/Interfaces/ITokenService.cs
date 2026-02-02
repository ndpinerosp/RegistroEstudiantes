using RegistroEstudiantes.Server.Entities;

namespace RegistroEstudiantes.Server.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Student student);
    }
}
