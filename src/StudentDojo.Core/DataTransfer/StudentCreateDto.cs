using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDojo.Core.DataTransfer;

public class StudentCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int ClassroomId { get; set; }
    }
