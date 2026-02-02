namespace RegistroEstudiantes.Server.DTOs
{
    public record LoginDto(string Email, string Password);
    public record RegisterDto(string Name, string LastName, string Email, string Password);
    public record AuthResponseDto
    {
        public string Token { get; init; } = null!;
        public string StudentName { get; init; } = null!;
        public string StudentLastName { get; init; } = null!;
        public string Email { get; init; } = null!;
    }

    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

}
