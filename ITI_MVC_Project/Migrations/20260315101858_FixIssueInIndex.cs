using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_MVC_Project.Migrations
{
    /// <inheritdoc />
    public partial class FixIssueInIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 15, 10, 18, 57, 626, DateTimeKind.Utc).AddTicks(6488), "AQAAAAIAAYagAAAAEOS40GxLIAPoc5V/y+9HugZBc8YafiW1FcJ+ucbCBzN3Nrjv0Cs8fCXMUcvpio0YOg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f",
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 15, 10, 4, 33, 50, DateTimeKind.Utc).AddTicks(45), "AQAAAAIAAYagAAAAEB8WMMwJolrtwZ+KnmpkSM7wvaSD7ZRLu9RXMZZjBW+q88GAOl/jwJaEMjglUX0ISg==" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }
    }
}
