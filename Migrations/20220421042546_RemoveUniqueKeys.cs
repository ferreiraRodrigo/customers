using Microsoft.EntityFrameworkCore.Migrations;

namespace Customers.Migrations
{
    public partial class RemoveUniqueKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReviewScore",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewScore",
                table: "Products",
                type: "text",
                nullable: true);
        }
    }
}
