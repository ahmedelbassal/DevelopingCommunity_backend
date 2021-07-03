using Microsoft.EntityFrameworkCore.Migrations;

namespace DevelopigCommunityService.Migrations
{
    public partial class UpateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Individuals_Departments_DepartmentId",
                table: "Individuals");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Instructors",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Individuals",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Admins",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Individuals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Individuals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Individuals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Individuals_Departments_DepartmentId",
                table: "Individuals",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Individuals_Departments_DepartmentId",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Individuals");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Instructors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Individuals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Admins",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Individuals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Individuals_Departments_DepartmentId",
                table: "Individuals",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
