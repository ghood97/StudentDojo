using StudentDojo.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDojo.Core.DataTransfer;
public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Points { get; set; }

    public StudentDto()
    {
        
    }

    public StudentDto(Student entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Points = entity.Points;
    }
}
