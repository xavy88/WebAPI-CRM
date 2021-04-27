using Microsoft.EntityFrameworkCore.Migrations;

namespace CRMAPI.Migrations
{
    public partial class ChangingUserInsteadEmployeeInAssigmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TaskAssignments",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignments_UserId",
                table: "TaskAssignments",
                newName: "IX_TaskAssignments_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Employees_EmployeeId",
                table: "TaskAssignments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Employees_EmployeeId",
                table: "TaskAssignments");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "TaskAssignments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignments_EmployeeId",
                table: "TaskAssignments",
                newName: "IX_TaskAssignments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_UserId",
                table: "TaskAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
