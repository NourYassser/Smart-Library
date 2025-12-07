using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLibrary.Api.Migrations
{
    /// <inheritdoc />
    public partial class userAcc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Borrowed",
                table: "BorrowRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borrowed",
                table: "BorrowRecords");
        }
    }
}
