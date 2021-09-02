using Microsoft.EntityFrameworkCore.Migrations;

namespace Teng.Infrastructure.Migrations
{
    public partial class appuseraddproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadPortrait",
                table: "AbpUsers",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadPortrait",
                table: "AbpUsers");
        }
    }
}
