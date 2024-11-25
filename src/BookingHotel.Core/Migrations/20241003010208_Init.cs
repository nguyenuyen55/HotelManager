using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingHotel.Core.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BE072024_HB_Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminSpecificInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Admins", x => x.AdminID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_CancellationPolicies",
                columns: table => new
                {
                    CancellationPolicyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancellationPeriod = table.Column<int>(type: "int", nullable: false),
                    RefundPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PenaltyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_CancellationPolicies", x => x.CancellationPolicyID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Hotels",
                columns: table => new
                {
                    HotelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Hotels", x => x.HotelID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_People",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_People", x => x.PersonID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Permission",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Screen_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approve = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true),
                    ServiceType = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken_ExpriredTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Rooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelID = table.Column<int>(type: "int", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomSquare = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Rooms_BE072024_HB_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "BE072024_HB_Hotels",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Staff",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Staff", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Staff_BE072024_HB_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "BE072024_HB_Hotels",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Addresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Addresses", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Addresses_BE072024_HB_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "BE072024_HB_People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerSpecificInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Customers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Customers_BE072024_HB_People_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "BE072024_HB_People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Emails",
                columns: table => new
                {
                    EmailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Emails", x => x.EmailID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Emails_BE072024_HB_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "BE072024_HB_People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Phones",
                columns: table => new
                {
                    PhoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Phones", x => x.PhoneID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Phones_BE072024_HB_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "BE072024_HB_People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Roles", x => x.RoleID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Roles_BE072024_HB_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "BE072024_HB_Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_BedRoom",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    BedID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_BedRoom", x => new { x.RoomID, x.BedID });
                    table.ForeignKey(
                        name: "FK_BE072024_HB_BedRoom_BE072024_HB_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "BE072024_HB_Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_ImageRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFileImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_ImageRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_ImageRooms_BE072024_HB_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "BE072024_HB_Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_RoomDetails",
                columns: table => new
                {
                    RoomDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    RoomFittings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomView = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerNight = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_RoomDetails", x => x.RoomDetailID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_RoomDetails_BE072024_HB_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "BE072024_HB_Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    DepositID = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Bookings_BE072024_HB_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "BE072024_HB_Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Bookings_BE072024_HB_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "BE072024_HB_Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_UserRoles",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_UserRoles", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_UserRoles_BE072024_HB_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "BE072024_HB_Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_UserRoles_BE072024_HB_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "BE072024_HB_Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_BookingDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateService = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_BookingDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_BookingDetails_BE072024_HB_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "BE072024_HB_Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_BookingDetails_BE072024_HB_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "BE072024_HB_Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Deposits",
                columns: table => new
                {
                    DepositID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositAmount = table.Column<int>(type: "int", nullable: false),
                    DepositStatus = table.Column<int>(type: "int", nullable: false),
                    CancellationPolicyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Deposits", x => x.DepositID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Deposits_BE072024_HB_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "BE072024_HB_Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Deposits_BE072024_HB_CancellationPolicies_CancellationPolicyID",
                        column: x => x.CancellationPolicyID,
                        principalTable: "BE072024_HB_CancellationPolicies",
                        principalColumn: "CancellationPolicyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BE072024_HB_Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BE072024_HB_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_BE072024_HB_Invoices_BE072024_HB_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "BE072024_HB_Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BE072024_HB_Roles",
                columns: new[] { "RoleID", "Description", "PermissionId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Administrator role with full permissions", null, "Admin" },
                    { 2, "Regular user with limited permissions", null, "User" },
                    { 3, "Customer role with permissions to book and view hotels", null, "Customer" },
                    { 4, "Staff role with permissions to manage hotel operations", null, "Staff" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Addresses_PersonID",
                table: "BE072024_HB_Addresses",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_BookingDetails_BookingID",
                table: "BE072024_HB_BookingDetails",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_BookingDetails_ServiceID",
                table: "BE072024_HB_BookingDetails",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Bookings_CustomerID",
                table: "BE072024_HB_Bookings",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Bookings_RoomID",
                table: "BE072024_HB_Bookings",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Deposits_BookingID",
                table: "BE072024_HB_Deposits",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Deposits_CancellationPolicyID",
                table: "BE072024_HB_Deposits",
                column: "CancellationPolicyID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Emails_PersonID",
                table: "BE072024_HB_Emails",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_ImageRooms_RoomID",
                table: "BE072024_HB_ImageRooms",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Invoices_BookingID",
                table: "BE072024_HB_Invoices",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Phones_PersonID",
                table: "BE072024_HB_Phones",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Roles_PermissionId",
                table: "BE072024_HB_Roles",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_RoomDetails_RoomID",
                table: "BE072024_HB_RoomDetails",
                column: "RoomID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Rooms_HotelID",
                table: "BE072024_HB_Rooms",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_Staff_HotelID",
                table: "BE072024_HB_Staff",
                column: "HotelID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_UserRoles_RoleID",
                table: "BE072024_HB_UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_BE072024_HB_UserRoles_UserID",
                table: "BE072024_HB_UserRoles",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BE072024_HB_Addresses");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Admins");

            migrationBuilder.DropTable(
                name: "BE072024_HB_BedRoom");

            migrationBuilder.DropTable(
                name: "BE072024_HB_BookingDetails");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Deposits");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Emails");

            migrationBuilder.DropTable(
                name: "BE072024_HB_ImageRooms");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Invoices");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Phones");

            migrationBuilder.DropTable(
                name: "BE072024_HB_RoomDetails");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Staff");

            migrationBuilder.DropTable(
                name: "BE072024_HB_UserRoles");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Services");

            migrationBuilder.DropTable(
                name: "BE072024_HB_CancellationPolicies");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Bookings");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Roles");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Users");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Customers");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Rooms");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Permission");

            migrationBuilder.DropTable(
                name: "BE072024_HB_People");

            migrationBuilder.DropTable(
                name: "BE072024_HB_Hotels");
        }
    }
}
