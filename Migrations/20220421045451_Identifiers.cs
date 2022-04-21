using Microsoft.EntityFrameworkCore.Migrations;

namespace Customers.Migrations
{
    public partial class Identifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                table: "WishLists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Customers",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WishLists",
                newName: "WishListId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customers",
                newName: "CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");
        }
    }
}
