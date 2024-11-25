using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableTT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "BE072024_HB_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "BE072024_HB_Users");
        }
    }
}
