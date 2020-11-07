using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentDetails_Comments_CommentID",
                table: "CommentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Deputies_Tasks_TaskID",
                table: "Deputies");

            migrationBuilder.DropForeignKey(
                name: "FK_Deputies_Users_UserID",
                table: "Deputies");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Tasks_TaskID",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Projects_ProjectID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Users_UserID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationDetails_Users_UserID",
                table: "NotificationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OCUsers_OCs_OCID",
                table: "OCUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OCUsers_Users_UserID",
                table: "OCUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_CreatedBy",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Projects_ProjectID",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tasks_TaskID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_UserID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_CreatedBy",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_OCs_OCID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Projects_ProjectID",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_UserID",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSystems_Users_UserID",
                table: "UserSystems");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentDetails_Comments_CommentID",
                table: "CommentDetails",
                column: "CommentID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskID",
                table: "Comments",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deputies_Tasks_TaskID",
                table: "Deputies",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deputies_Users_UserID",
                table: "Deputies",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Tasks_TaskID",
                table: "Follows",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Projects_ProjectID",
                table: "Managers",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Users_UserID",
                table: "Managers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationDetails_Users_UserID",
                table: "NotificationDetails",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCUsers_OCs_OCID",
                table: "OCUsers",
                column: "OCID",
                principalTable: "OCs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OCUsers_Users_UserID",
                table: "OCUsers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_CreatedBy",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Projects_ProjectID",
                table: "Rooms",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tasks_TaskID",
                table: "Tags",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_UserID",
                table: "Tags",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_CreatedBy",
                table: "Tasks",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_OCs_OCID",
                table: "Tasks",
                column: "OCID",
                principalTable: "OCs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectID",
                table: "Tasks",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Projects_ProjectID",
                table: "TeamMembers",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_UserID",
                table: "TeamMembers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSystems_Users_UserID",
                table: "UserSystems",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentDetails_Comments_CommentID",
                table: "CommentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Deputies_Tasks_TaskID",
                table: "Deputies");

            migrationBuilder.DropForeignKey(
                name: "FK_Deputies_Users_UserID",
                table: "Deputies");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Tasks_TaskID",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Projects_ProjectID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Users_UserID",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationDetails_Users_UserID",
                table: "NotificationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OCUsers_OCs_OCID",
                table: "OCUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OCUsers_Users_UserID",
                table: "OCUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_CreatedBy",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Projects_ProjectID",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tasks_TaskID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_UserID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_CreatedBy",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_OCs_OCID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Projects_ProjectID",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_UserID",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSystems_Users_UserID",
                table: "UserSystems");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentDetails_Comments_CommentID",
                table: "CommentDetails",
                column: "CommentID",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskID",
                table: "Comments",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deputies_Tasks_TaskID",
                table: "Deputies",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Deputies_Users_UserID",
                table: "Deputies",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Tasks_TaskID",
                table: "Follows",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_UserID",
                table: "Follows",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Projects_ProjectID",
                table: "Managers",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Users_UserID",
                table: "Managers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationDetails_Users_UserID",
                table: "NotificationDetails",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OCUsers_OCs_OCID",
                table: "OCUsers",
                column: "OCID",
                principalTable: "OCs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OCUsers_Users_UserID",
                table: "OCUsers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_CreatedBy",
                table: "Projects",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Projects_ProjectID",
                table: "Rooms",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tasks_TaskID",
                table: "Tags",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_UserID",
                table: "Tags",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_CreatedBy",
                table: "Tasks",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_OCs_OCID",
                table: "Tasks",
                column: "OCID",
                principalTable: "OCs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectID",
                table: "Tasks",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Projects_ProjectID",
                table: "TeamMembers",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_UserID",
                table: "TeamMembers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Tasks_TaskID",
                table: "Tutorials",
                column: "TaskID",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSystems_Users_UserID",
                table: "UserSystems",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
