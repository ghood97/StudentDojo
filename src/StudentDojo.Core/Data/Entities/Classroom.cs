using System.ComponentModel.DataAnnotations;

namespace StudentDojo.Core.Data.Entities;

public class Classroom
{
    public int Id { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public ICollection<TeacherClassroom> Teachers { get; set; } = [];
    public ICollection<Student> Students { get; set; } = [];
}
