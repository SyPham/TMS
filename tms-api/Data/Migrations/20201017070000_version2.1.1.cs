using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class version211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID");
        }
    }
}
