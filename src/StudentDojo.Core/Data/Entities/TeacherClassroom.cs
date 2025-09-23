namespace StudentDojo.Core.Data.Entities;

public class TeacherClassroom
{
    public int TeacherId { get; set; }
    public int ClassroomId { get; set; }
    public Teacher Teacher { get; set; } = default!;
    public Classroom Classroom { get; set; } = default!;
}
