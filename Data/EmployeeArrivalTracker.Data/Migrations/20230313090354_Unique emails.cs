using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeArrivalTracker.Data.Migrations
{
    public partial class Uniqueemails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Person_Email",
                table: "Person",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Person_Email",
                table: "Person");
        }
    }
}
