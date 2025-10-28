using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ExternalAccounts.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "external_accounts");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "external_accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character(26)", fixedLength: true, maxLength: 26, nullable: false),
                    OwnerId = table.Column<string>(type: "character(26)", fixedLength: true, maxLength: 26, nullable: false),
                    LinkedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSyncedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SyncInterval = table.Column<long>(type: "bigint", nullable: false),
                    ExternalAccountId_Kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ExternalAccountId_Value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "external_accounts");
        }
    }
}
