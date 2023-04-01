using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WasteZero.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    Code = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    IsGlutenFree = table.Column<bool>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    ProductTypeID = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeID",
                        column: x => x.ProductTypeID,
                        principalTable: "ProductTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    ProductID = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Weight = table.Column<float>(type: "REAL", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductID",
                table: "ProductDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeID",
                table: "Products",
                column: "ProductTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
