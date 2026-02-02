namespace RegistroEstudiantes.Server.Entities
{
    public class Professor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = [];
    }
}
