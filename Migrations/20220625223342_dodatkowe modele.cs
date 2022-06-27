using Microsoft.EntityFrameworkCore.Migrations;

namespace Projekt.Migrations
{
    public partial class dodatkowemodele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WatchedElement",
                columns: table => new
                {
                    ticker = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedElement", x => x.ticker);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WatchedList",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    ticker = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedList", x => new { x.ticker, x.IdUser });
                    table.ForeignKey(
                        name: "FK_WatchedList_Client_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Client",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedList_WatchedElement_ticker",
                        column: x => x.ticker,
                        principalTable: "WatchedElement",
                        principalColumn: "ticker",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedList_IdUser",
                table: "WatchedList",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedList");

            migrationBuilder.DropTable(
                name: "WatchedElement");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");
        }
    }
}
