using Microsoft.EntityFrameworkCore.Migrations;

namespace Traning.AspNetCore.Microservices.Basket.API.Migrations
{
    public partial class FixBasketProductReleation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBasketProduct_CustomerBaskets_ProductId",
                table: "CustomerBasketProduct");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBasketProduct_ProductId",
                table: "CustomerBasketProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBasketProduct_CustomerBaskets_BasketId",
                table: "CustomerBasketProduct",
                column: "BasketId",
                principalTable: "CustomerBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBasketProduct_CustomerBaskets_BasketId",
                table: "CustomerBasketProduct");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBasketProduct_ProductId",
                table: "CustomerBasketProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBasketProduct_CustomerBaskets_ProductId",
                table: "CustomerBasketProduct",
                column: "ProductId",
                principalTable: "CustomerBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
