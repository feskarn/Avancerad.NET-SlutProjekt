using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avancerad.NET_SlutProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Fixatlitemedjwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointId",
                keyValue: 1,
                columns: new[] { "CreationDate", "Description", "StartingTime", "Title", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 19, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7737), "Undersökning för att hitta eventuella skador/sjukdomar", new DateTime(2024, 5, 25, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7736), "Undersökning", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7737) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointId",
                keyValue: 2,
                columns: new[] { "CreationDate", "Description", "StartingTime", "Title", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 18, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7741), "Träningspass med våran PT", new DateTime(2024, 5, 27, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7741), "PT-Pass", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7742) });

            migrationBuilder.UpdateData(
                table: "ChangeLogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Action", "ChangedAtDate", "Details" },
                values: new object[] { "Skapad", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7760), "Möte skapad för Anna Andersson" });

            migrationBuilder.UpdateData(
                table: "ChangeLogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "ChangedAtDate", "Details" },
                values: new object[] { "Skapad", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7762), "Möte skapad för Karl Karlsson" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CompanyEmail", "CompanyName" },
                values: new object[] { "fbg@sjukvård.se", "FBG SJUKVÅRD" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2,
                columns: new[] { "CompanyEmail", "CompanyName" },
                values: new object[] { "contact@friskissvettis.se", "Friskis & Svettis" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "CreationDate", "Email", "FirstName", "LastName", "Phone", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 10, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7675), "AnnaAndersson@hotmail.com", "Anna", "Andersson", "0788995544", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7688) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                columns: new[] { "CreationDate", "Email", "FirstName", "LastName", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 12, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7690), "KarlKarlsson@hotmail.com", "Karl", "Karlsson", new DateTime(2024, 5, 20, 16, 53, 8, 340, DateTimeKind.Utc).AddTicks(7690) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "password", "elf" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "password", "peter" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "Role", "Username" },
                values: new object[] { "password", "Customer", "isac" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointId",
                keyValue: 1,
                columns: new[] { "CreationDate", "Description", "StartingTime", "Title", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 19, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3594), "Meeting to discuss project requirements.", new DateTime(2024, 5, 25, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3592), "Consultation Meeting", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3595) });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointId",
                keyValue: 2,
                columns: new[] { "CreationDate", "Description", "StartingTime", "Title", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 18, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3597), "Review of the initial design drafts.", new DateTime(2024, 5, 27, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3597), "Design Review", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3598) });

            migrationBuilder.UpdateData(
                table: "ChangeLogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Action", "ChangedAtDate", "Details" },
                values: new object[] { "Created", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3620), "Appointment created for John Wall" });

            migrationBuilder.UpdateData(
                table: "ChangeLogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Action", "ChangedAtDate", "Details" },
                values: new object[] { "Created", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3622), "Appointment created for Mary Jane" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CompanyEmail", "CompanyName" },
                values: new object[] { "info@techsolutions.com", "Tech Solutions Inc." });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2,
                columns: new[] { "CompanyEmail", "CompanyName" },
                values: new object[] { "contact@innovativedesigns.com", "Innovative Designs LLC" });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                columns: new[] { "CreationDate", "Email", "FirstName", "LastName", "Phone", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 10, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3519), "john.wall@hotmail.com", "John", "Wall", "+4634567890", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3528) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                columns: new[] { "CreationDate", "Email", "FirstName", "LastName", "UpdateDate" },
                values: new object[] { new DateTime(2024, 5, 12, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3530), "mary.jane@hotmail.com", "Mary", "Jane", new DateTime(2024, 5, 20, 15, 49, 49, 767, DateTimeKind.Utc).AddTicks(3531) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "AQAAAAIAAYagAAAAEKZyqfbYTLSLOy5x3AqE+ghAS8vuOdSea0wtFya7/1Ghb6JmZZsoWYkoftIV5f+WKg==", "admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "AQAAAAIAAYagAAAAEPZBqKdXElk35szAwV8ORVOQdP2LgdGubdR20ooCUr+nsE25w3WEyy5FLvAp1Nekpg==", "user1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PasswordHash", "Role", "Username" },
                values: new object[] { "AQAAAAIAAYagAAAAEHKg+4G83eZZeVSE40lGyifQRnqa1PaypToq8mWpvOOJOwug1yEySQ8sAdPpriDEJg==", "User", "user2" });
        }
    }
}
