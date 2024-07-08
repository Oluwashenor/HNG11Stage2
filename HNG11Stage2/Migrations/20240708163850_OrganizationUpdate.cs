using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HNG11Stage2.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Owner",
                table: "UserOrganizations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Organizations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "UserOrganizations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Organizations");
        }
    }
}
