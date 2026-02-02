namespace RegistroEstudiantes.Server.DTOs
{
    public record EnrollmentRequestDto(int CourseId);


    public record ClassmateDto(string Name);
    public record StudentProgressDto(
    string StudentName,
    string Email
    //List<string> EnrolledCourses //activar en caso de querer las materias publicas
    );

    public record MyDashboardDto(
        int TotalCredits,
        int EnrolledCoursesCount, 
        List<CourseResponseDto> MyCourses
        );
}
