using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parc_Auto_v3.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c58cbcd-54f2-419d-9e83-c9b284b98c05");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6141d24a-357e-4863-b482-8e0952be660a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89f26f77-1b86-4bb6-90f7-d8c5e0026a5b");

            migrationBuilder.AddColumn<string>(
                name: "NumeroSerieCarteGrise",
                table: "Voitures",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "NumeroSerieCarteGrise",
                table: "Voitures");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c58cbcd-54f2-419d-9e83-c9b284b98c05", null, "agent", "agent" },
                    { "6141d24a-357e-4863-b482-8e0952be660a", null, "utilisateur", "utilisateur" },
                    { "89f26f77-1b86-4bb6-90f7-d8c5e0026a5b", null, "admin", "admin" }
                });
        }
    }
}
