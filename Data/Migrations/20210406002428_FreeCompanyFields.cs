using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class FreeCompanyFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstateAddress",
                table: "FreeCompanies",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstateName",
                table: "FreeCompanies",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "FreeCompanies",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstateAddress",
                table: "FreeCompanies");

            migrationBuilder.DropColumn(
                name: "EstateName",
                table: "FreeCompanies");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "FreeCompanies");
        }
    }
}
