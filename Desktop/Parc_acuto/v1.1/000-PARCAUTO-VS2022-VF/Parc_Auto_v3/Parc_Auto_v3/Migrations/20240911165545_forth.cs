using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parc_Auto_v3.Migrations
{
    /// <inheritdoc />
    public partial class forth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "507245e6-a072-4242-892e-1908868eee0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97f75d2b-4dc6-4634-9e6c-a3160687d74e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7c693ee-f736-4327-9ddc-c7108db7dfc7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02fe13a1-8e02-4f3d-b5cd-428f65c34c15", null, "agent", "agent" },
                    { "c59968ab-f979-4adc-903d-f78bc5d041e5", null, "admin", "admin" },
                    { "ccf8789f-2f15-4c30-92e2-0353ad87693f", null, "superadmin", "superadmin" },
                    { "f96fc7d1-f477-4dd0-a124-9b809e154ff5", null, "utilisateur", "utilisateur" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02fe13a1-8e02-4f3d-b5cd-428f65c34c15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c59968ab-f979-4adc-903d-f78bc5d041e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ccf8789f-2f15-4c30-92e2-0353ad87693f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f96fc7d1-f477-4dd0-a124-9b809e154ff5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "507245e6-a072-4242-892e-1908868eee0d", null, "admin", "admin" },
                    { "97f75d2b-4dc6-4634-9e6c-a3160687d74e", null, "utilisateur", "utilisateur" },
                    { "d7c693ee-f736-4327-9ddc-c7108db7dfc7", null, "agent", "agent" }
                });
        }
    }
}
