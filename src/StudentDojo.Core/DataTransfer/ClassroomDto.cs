using StudentDojo.Core.Data.Entities;

namespace StudentDojo.Core.DataTransfer;

public class ClassroomDto
{
    public int Id { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public ICollection<TeacherDto> Teachers { get; set; } = [];

    public ClassroomDto()
    {
    }

    public ClassroomDto(Classroom classroom)
    {
        Id = classroom.Id;
        ClassName = classroom.ClassName;
        Teachers = classroom.Teachers.Select(tc => new TeacherDto(tc.Teacher)).ToList();
    }
}
