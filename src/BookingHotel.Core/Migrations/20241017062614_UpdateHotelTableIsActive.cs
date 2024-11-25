using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHotelTableIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "BE072024_HB_Hotels",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "BE072024_HB_Hotels");
        }
    }
}
