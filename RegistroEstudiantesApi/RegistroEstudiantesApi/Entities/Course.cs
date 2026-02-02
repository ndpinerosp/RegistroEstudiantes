namespace RegistroEstudiantes.Server.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; } = 3;
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}
