namespace RegistroEstudiantes.Server.DTOs
{
    public record CourseResponseDto
    (
        int Id,
        string Name,
        int Credits,
        int ProfessorId,
        string ProfessorName,
        List<string> Classmates

    );
}
