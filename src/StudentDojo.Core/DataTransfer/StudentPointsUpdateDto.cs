using StudentDojo.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDojo.Core.DataTransfer;
public class ClassroomPointsUpdateDto
{
    public List<StudentPointsUpdateDto> Updates { get; set; } = new();
}

public class StudentPointsUpdateDto
{
    public int StudentId { get; set; }

    // Either "add" or "redeem"
    public string Operation { get; set; } = "add";

    public int Value { get; set; }
}
