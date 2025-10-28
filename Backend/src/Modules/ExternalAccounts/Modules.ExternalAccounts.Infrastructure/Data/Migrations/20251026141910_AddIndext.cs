using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.ExternalAccounts.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalAccountId_Value",
                schema: "external_accounts",
                table: "Accounts",
                newName: "ExternalAccountValue");

            migrationBuilder.RenameColumn(
                name: "ExternalAccountId_Kind",
                schema: "external_accounts",
                table: "Accounts",
                newName: "ExternalAccountKind");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalAccountValue",
                schema: "external_accounts",
                table: "Accounts",
                newName: "ExternalAccountId_Value");

            migrationBuilder.RenameColumn(
                name: "ExternalAccountKind",
                schema: "external_accounts",
                table: "Accounts",
                newName: "ExternalAccountId_Kind");
        }
    }
}
