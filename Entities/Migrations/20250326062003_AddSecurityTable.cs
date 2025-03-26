using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "block_bruteforce",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: "A", comment: "L (Locked): ถูกล็อก\r\nU (UnLock): ไม่ล็อก"),
                    locked_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    unlock_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("block_bruteforce_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    platform = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    os = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    browser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    login_ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    issued_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    session_status = table.Column<string>(type: "character varying(1)", unicode: false, maxLength: 1, nullable: false, defaultValue: "A", comment: "B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว"),
                    token = table.Column<string>(type: "text", nullable: true),
                    refresh_token_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("system_session_pk", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "block_bruteforce");

            migrationBuilder.DropTable(
                name: "system_sessions");
        }
    }
}
