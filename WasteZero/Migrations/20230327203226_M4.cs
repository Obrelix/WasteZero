using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteZero.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumedDetails",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    ProductID = table.Column<Guid>(type: "TEXT", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Weight = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConsumedDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedDetails_ProductID",
                table: "ConsumedDetails",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedDetails");
        }
    }
}
