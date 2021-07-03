using Microsoft.EntityFrameworkCore.Migrations;

namespace DevelopigCommunityService.Migrations
{
    public partial class updateProjectPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectPhotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhotos_ProjectId",
                table: "ProjectPhotos",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPhotos_Projects_ProjectId",
                table: "ProjectPhotos",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPhotos_Projects_ProjectId",
                table: "ProjectPhotos");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPhotos_ProjectId",
                table: "ProjectPhotos");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectPhotos");
        }
    }
}
