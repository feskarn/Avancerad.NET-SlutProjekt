using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Avancerad.NET_SlutProjekt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointId = table.Column<int>(type: "int", nullable: false),
                    ChangedAtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false), // Increased size to 100
                    UserId = table.Column<int>(type: "int", nullable: false)
                },

                            constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointId);
                    table.ForeignKey(
                        name: "FK_Appointments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ChangeLogs",
                columns: new[] { "Id", "Action", "AppointId", "ChangedAtDate", "CompanyId", "CustomerId", "Details" },
                values: new object[,]
                {
                    { 1, "Created", 1, new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6451), 1, 1, "Appointment created for John Wall" },
                    { 2, "Created", 2, new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6454), 2, 2, "Appointment created for Mary Jane" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, true, "password123", "Admin", "admin" },
                    { 2, true, "hejhej456", "User", "user1" },
                    { 3, true, "user2", "User", "user2" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyEmail", "CompanyName", "UserId" },
                values: new object[,]
                {
                    { 1, "info@techsolutions.com", "Tech Solutions Inc.", 1 },
                    { 2, "contact@innovativedesigns.com", "Innovative Designs LLC", 1 }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreationDate", "Email", "FirstName", "LastName", "Phone", "UpdateDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 10, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6391), "john.wall@hotmail.com", "John", "Wall", "+4634567890", new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6397), 2 },
                    { 2, new DateTime(2024, 5, 12, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6399), "mary.jane@hotmail.com", "Mary", "Jane", "+4698765432", new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6399), 3 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointId", "AppointmentDurationMinutes", "CompanyId", "CreationDate", "CustomerId", "Description", "StartingTime", "Title", "UpdateDate" },
                values: new object[,]
                {
                    { 1, 60, 1, new DateTime(2024, 5, 19, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6434), 1, "Meeting to discuss project requirements.", new DateTime(2024, 5, 25, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6433), "Consultation Meeting", new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6435) },
                    { 2, 30, 2, new DateTime(2024, 5, 18, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6437), 2, "Review of the initial design drafts.", new DateTime(2024, 5, 27, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6437), "Design Review", new DateTime(2024, 5, 20, 12, 12, 26, 23, DateTimeKind.Utc).AddTicks(6438) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CompanyId",
                table: "Appointments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CustomerId",
                table: "Appointments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserId",
                table: "Companies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
