namespace StudentDojo.Core.Data.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty;
    public ICollection<TeacherClassroom> Classrooms { get; set; } = [];
}
