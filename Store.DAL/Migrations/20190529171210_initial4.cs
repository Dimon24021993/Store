using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DAL.Migrations
{
    public partial class initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Href",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Groups",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Articles",
                type: "decimal(12,10)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Href",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Groups");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Articles",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,10)");
        }
    }
}
