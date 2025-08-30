using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data.Entities;

namespace StudentDojo.Core.Data;

public class StudentDojoDbContext : DbContext
{
    public StudentDojoDbContext(DbContextOptions<StudentDojoDbContext> options)
        : base(options)
    {
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<TeacherClassroom> TeacherClassrooms { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentDojoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
