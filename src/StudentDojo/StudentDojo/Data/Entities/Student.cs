namespace StudentDojo.Data.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; }
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = default!;
}
