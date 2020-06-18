using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Traning.AspNetCore.Microservices.Basket.API.Migrations
{
    public partial class CustomerBasketEmailColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerBaskets");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "CustomerBaskets",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "CustomerBaskets");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "CustomerBaskets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
