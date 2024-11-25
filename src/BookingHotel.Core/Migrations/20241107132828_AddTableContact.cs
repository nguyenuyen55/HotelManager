using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTableContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BE072024_HB_Bookings_BE072024_HB_Customers_CustomerID",
                table: "BE072024_HB_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "BE072024_HB_Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ContactID",
                table: "BE072024_HB_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "BE072024_HB_Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "BE072024_HB_Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Contacts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BE072024_HB_Bookings_BE072024_HB_Customers_CustomerID",
                table: "BE072024_HB_Bookings",
                column: "CustomerID",
                principalTable: "BE072024_HB_Customers",
                principalColumn: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BE072024_HB_Bookings_BE072024_HB_Customers_CustomerID",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Contacts");

            migrationBuilder.DropColumn(
                name: "ContactID",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "BE072024_HB_Bookings");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "BE072024_HB_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "BE072024_HB_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BE072024_HB_Bookings_BE072024_HB_Customers_CustomerID",
                table: "BE072024_HB_Bookings",
                column: "CustomerID",
                principalTable: "BE072024_HB_Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
