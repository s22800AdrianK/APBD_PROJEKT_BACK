using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt.Migrations
{
    public partial class OHLCs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "oHLCs",
                columns: table => new
                {
                    WatchedElementTicker = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    t = table.Column<long>(type: "bigint", nullable: false),
                    c = table.Column<double>(type: "double", nullable: false),
                    h = table.Column<double>(type: "double", nullable: false),
                    l = table.Column<double>(type: "double", nullable: false),
                    n = table.Column<long>(type: "bigint", nullable: false),
                    o = table.Column<double>(type: "double", nullable: false),
                    vw = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oHLCs", x => new { x.WatchedElementTicker, x.t });
                    table.ForeignKey(
                        name: "FK_oHLCs_WatchedElement_WatchedElementTicker",
                        column: x => x.WatchedElementTicker,
                        principalTable: "WatchedElement",
                        principalColumn: "ticker",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "oHLCs");
        }
    }
}
