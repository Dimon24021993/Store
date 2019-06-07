using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DAL.Migrations
{
    public partial class initial6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Items",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Articles",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Items",
                type: "decimal(12,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Articles",
                type: "decimal(12,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");
        }
    }
}
