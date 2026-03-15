using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITI_MVC_Project.Migrations
{
    /// <inheritdoc />
    public partial class InsertAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d", "f1a2b3c4-d5e6-f7a8-b9c0-d1e2f3a4b5c6", "Customer", "CUSTOMER" },
                    { "f7e8b5d5-3a2c-4e1f-9b6d-0a1b2c3d4e5f", "e1f2a3b4-c5d6-e7f8-a9b0-c1d2e3f4a5b6", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f", 0, "Cairo, Egypt", "Cairo", "d1e2f3a4-b5c6-d7e8-f9a0-b1c2d3e4f5a6", new DateTime(2026, 3, 11, 18, 50, 37, 947, DateTimeKind.Utc).AddTicks(6452), "admin@iti.com", true, "Admin", "User", false, null, "ADMIN@ITI.COM", "ADMIN@ITI.COM", "AQAAAAIAAYagAAAAEPtq0AmRu9RvVuq1CEU975e1hr9FxWQpwz0HNe2lIwDU9+E0ccH1ydgoQKx/VhmyOw==", null, false, "F1E2D3C4B5A69788F1E2D3C4B5A69788", false, "admin@iti.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f7e8b5d5-3a2c-4e1f-9b6d-0a1b2c3d4e5f", "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f7e8b5d5-3a2c-4e1f-9b6d-0a1b2c3d4e5f", "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7e8b5d5-3a2c-4e1f-9b6d-0a1b2c3d4e5f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c1d2e3f4-5a6b-7c8d-9e0f-1a2b3c4d5e6f");
        }
    }
}
