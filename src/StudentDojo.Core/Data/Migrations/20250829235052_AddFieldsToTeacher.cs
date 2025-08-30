using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentDojo.Core.Migrations;

/// <inheritdoc />
public partial class AddFieldsToTeacher : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Name",
            table: "Teachers",
            newName: "LastName");

        migrationBuilder.AddColumn<string>(
            name: "AuthId",
            table: "Teachers",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "DisplayName",
            table: "Teachers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "FirstName",
            table: "Teachers",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AuthId",
            table: "Teachers");

        migrationBuilder.DropColumn(
            name: "DisplayName",
            table: "Teachers");

        migrationBuilder.DropColumn(
            name: "FirstName",
            table: "Teachers");

        migrationBuilder.RenameColumn(
            name: "LastName",
            table: "Teachers",
            newName: "Name");
    }
}
