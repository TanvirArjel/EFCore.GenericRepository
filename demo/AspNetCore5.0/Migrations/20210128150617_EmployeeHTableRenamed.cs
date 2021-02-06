using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCore5._0.Migrations
{
    public partial class EmployeeHTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_employeeHistories",
                table: "employeeHistories");

            migrationBuilder.RenameTable(
                name: "employeeHistories",
                newName: "EmployeeHistories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeHistories",
                table: "EmployeeHistories",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeHistories",
                table: "EmployeeHistories");

            migrationBuilder.RenameTable(
                name: "EmployeeHistories",
                newName: "employeeHistories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employeeHistories",
                table: "employeeHistories",
                column: "EmployeeId");
        }
    }
}
