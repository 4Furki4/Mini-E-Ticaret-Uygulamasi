using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Files",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Files",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Files");
        }
    }
}
