using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                table: "BE072024_HB_Bookings");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "BE072024_HB_Bookings",
                newName: "ToDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BE072024_HB_Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "BE072024_HB_Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BE072024_HB_Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "BE072024_HB_Bookings",
                type: "bit",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "BE072024_HB_Bookings");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "BE072024_HB_Bookings",
                newName: "BookingDate");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                table: "BE072024_HB_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }
    }
}
