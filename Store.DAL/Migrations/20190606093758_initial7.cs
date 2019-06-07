using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.DAL.Migrations
{
    public partial class initial7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Groups_GroupId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_GroupId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Items");

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    ItemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemGroups_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_GroupId",
                table: "ItemGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_ItemId",
                table: "ItemGroups",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_GroupId",
                table: "Items",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Groups_GroupId",
                table: "Items",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
