using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWT_Demo.Migrations
{
    /// <inheritdoc />
    public partial class fourthDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiverAccountNumber",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverAccountNumber",
                table: "Transactions");
        }
    }
}
