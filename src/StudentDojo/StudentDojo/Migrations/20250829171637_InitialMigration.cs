using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentDojo.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Classrooms",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Classrooms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Teachers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teachers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Points = table.Column<int>(type: "int", nullable: false),
                ClassroomId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.Id);
                table.ForeignKey(
                    name: "FK_Students_Classrooms_ClassroomId",
                    column: x => x.ClassroomId,
                    principalTable: "Classrooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "TeacherClassrooms",
            columns: table => new
            {
                TeacherId = table.Column<int>(type: "int", nullable: false),
                ClassroomId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TeacherClassrooms", x => new { x.TeacherId, x.ClassroomId });
                table.ForeignKey(
                    name: "FK_TeacherClassrooms_Classrooms_ClassroomId",
                    column: x => x.ClassroomId,
                    principalTable: "Classrooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_TeacherClassrooms_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Students_ClassroomId",
            table: "Students",
            column: "ClassroomId");

        migrationBuilder.CreateIndex(
            name: "IX_TeacherClassrooms_ClassroomId",
            table: "TeacherClassrooms",
            column: "ClassroomId");

        migrationBuilder.CreateIndex(
            name: "IX_Teachers_Email",
            table: "Teachers",
            column: "Email");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Students");

        migrationBuilder.DropTable(
            name: "TeacherClassrooms");

        migrationBuilder.DropTable(
            name: "Classrooms");

        migrationBuilder.DropTable(
            name: "Teachers");
    }
}
