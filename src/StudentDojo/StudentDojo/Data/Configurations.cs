using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentDojo.Data.Entities;

namespace StudentDojo.Data;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(t => t.Classrooms)
            .WithOne(tc => tc.Teacher)
            .HasForeignKey(tc => tc.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Email);
    }
}

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClassName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(c => c.Teachers)
            .WithOne(tc => tc.Classroom)
            .HasForeignKey(tc => tc.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Students)
            .WithOne(s => s.Classroom)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Points);

        builder.HasOne(s => s.Classroom)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TeacherClassroomConfiguration : IEntityTypeConfiguration<TeacherClassroom>
{
    public void Configure(EntityTypeBuilder<TeacherClassroom> builder)
    {
        builder.HasKey(tc => new { tc.TeacherId, tc.ClassroomId });

        builder.HasOne(tc => tc.Teacher)
            .WithMany(t => t.Classrooms)
            .HasForeignKey(tc => tc.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tc => tc.Classroom)
            .WithMany(c => c.Teachers)
            .HasForeignKey(tc => tc.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}