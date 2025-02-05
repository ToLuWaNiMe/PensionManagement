using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsValidToContribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Contributions",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Contributions");
        }
    }
}
