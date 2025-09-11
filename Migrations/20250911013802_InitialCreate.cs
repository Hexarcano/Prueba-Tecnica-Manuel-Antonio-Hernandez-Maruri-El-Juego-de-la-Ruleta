using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ruleta.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gamblers",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Funds = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamblers", x => x.Name);
                    table.CheckConstraint("CK_Gambler_Funds_Positive", "\"Funds\" >= 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gamblers_Name",
                table: "Gamblers",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gamblers");
        }
    }
}
