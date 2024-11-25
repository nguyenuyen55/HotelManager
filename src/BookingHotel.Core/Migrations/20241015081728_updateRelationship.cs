using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class updateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BE072024_HB_RoomDetails_BE072024_HB_Rooms_RoomID",
                table: "BE072024_HB_RoomDetails");

            migrationBuilder.DropIndex(
                name: "IX_BE072024_HB_RoomDetails_RoomID",
                table: "BE072024_HB_RoomDetails");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "BE072024_HB_RoomDetails");

            migrationBuilder.AddColumn<int>(
                name: "RoomDetailID",
                table: "BE072024_HB_Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Rooms_RoomDetailID",
                table: "BE072024_HB_Rooms",
                column: "RoomDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_BE072024_HB_Rooms_BE072024_HB_RoomDetails_RoomDetailID",
                table: "BE072024_HB_Rooms",
                column: "RoomDetailID",
                principalTable: "BE072024_HB_RoomDetails",
                principalColumn: "RoomDetailID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BE072024_HB_Rooms_BE072024_HB_RoomDetails_RoomDetailID",
                table: "BE072024_HB_Rooms");

            migrationBuilder.DropIndex(
                name: "IX_BE072024_HB_Rooms_RoomDetailID",
                table: "BE072024_HB_Rooms");

            migrationBuilder.DropColumn(
                name: "RoomDetailID",
                table: "BE072024_HB_Rooms");

            migrationBuilder.AddColumn<int>(
                name: "RoomID",
                table: "BE072024_HB_RoomDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_RoomDetails_RoomID",
                table: "BE072024_HB_RoomDetails",
                column: "RoomID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BE072024_HB_RoomDetails_BE072024_HB_Rooms_RoomID",
                table: "BE072024_HB_RoomDetails",
                column: "RoomID",
                principalTable: "BE072024_HB_Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
