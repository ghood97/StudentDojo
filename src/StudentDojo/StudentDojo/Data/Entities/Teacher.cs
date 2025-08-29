namespace StudentDojo.Data.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<TeacherClassroom> Classrooms { get; set; } = [];
}
