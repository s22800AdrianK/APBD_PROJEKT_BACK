using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt.Migrations
{
    public partial class ChangesinOHLCS2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "v",
                table: "oHLCs",
                type: "double",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "v",
                table: "oHLCs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
