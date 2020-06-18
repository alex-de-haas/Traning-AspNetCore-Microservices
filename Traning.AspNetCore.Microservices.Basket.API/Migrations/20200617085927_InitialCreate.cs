using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Traning.AspNetCore.Microservices.Basket.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerBaskets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBaskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBasketProduct",
                columns: table => new
                {
                    BasketId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBasketProduct", x => new { x.BasketId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CustomerBasketProduct_CustomerBaskets_ProductId",
                        column: x => x.ProductId,
                        principalTable: "CustomerBaskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBasketProduct_ProductId",
                table: "CustomerBasketProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerBasketProduct");

            migrationBuilder.DropTable(
                name: "CustomerBaskets");
        }
    }
}
