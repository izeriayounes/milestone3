using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace milestone3.Migrations
{
    public partial class testToDeleteId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Customers",
                type: "int",
                nullable: true);
        }
    }
}
