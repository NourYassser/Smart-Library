using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BorrowService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class barCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowRecords",
                table: "BorrowRecords");

            migrationBuilder.RenameTable(
                name: "BorrowRecords",
                newName: "Borrow");

            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Borrow",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrow",
                table: "Borrow",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrow",
                table: "Borrow");

            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Borrow");

            migrationBuilder.RenameTable(
                name: "Borrow",
                newName: "BorrowRecords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowRecords",
                table: "BorrowRecords",
                column: "Id");
        }
    }
}
