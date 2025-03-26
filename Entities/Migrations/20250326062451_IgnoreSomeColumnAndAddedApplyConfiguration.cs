using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class IgnoreSomeColumnAndAddedApplyConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phonenumber_confirmed",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "twofactor_enabled",
                table: "accounts");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "accounts",
                newName: "access_failed_count");

            migrationBuilder.InsertData(
                table: "accounts",
                columns: new[] { "id", "access_failed_count", "active", "concurrency_stamp", "created_at", "created_by", "email", "email_confirmed", "first_name", "language", "last_name", "lockout_enabled", "lockout_end", "normalized_email", "normalized_username", "password_hash", "phonenumber", "security_stamp", "Title", "updated_at", "updated_by", "username" },
                values: new object[] { 1, 0, true, "1a1ee6a4-cc3e-480f-b5a1-57340419d337", new DateTime(2025, 3, 26, 6, 24, 50, 242, DateTimeKind.Utc).AddTicks(4883), "system", null, false, "John", "English", "Doe", false, null, null, null, null, null, null, "Mr.", null, null, null });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "active", "concurrency_stamp", "created_at", "created_by", "name", "normalized_name", "role_code", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, false, null, new DateTime(2025, 3, 26, 6, 24, 50, 242, DateTimeKind.Utc).AddTicks(3688), "Configure", "Administrator", "ADMINISTRATOR", "R-001", null, null },
                    { 2, false, null, new DateTime(2025, 3, 26, 6, 24, 50, 242, DateTimeKind.Utc).AddTicks(3709), "Configure", "User", "USER", "R-002", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "accounts",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                table: "accounts",
                newName: "AccessFailedCount");

            migrationBuilder.AddColumn<bool>(
                name: "phonenumber_confirmed",
                table: "accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "twofactor_enabled",
                table: "accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
