using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class version210 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SignInHistories_SystemCode",
                table: "SignInHistories",
                column: "SystemCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SignInHistories_SystemCodes_SystemCode",
                table: "SignInHistories",
                column: "SystemCode",
                principalTable: "SystemCodes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignInHistories_SystemCodes_SystemCode",
                table: "SignInHistories");

            migrationBuilder.DropIndex(
                name: "IX_SignInHistories_SystemCode",
                table: "SignInHistories");
        }
    }
}
