using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parc_Auto_v3.Migrations
{
    /// <inheritdoc />
    public partial class sec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf55f559-de7f-4375-b6e9-72a4fec91df9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c96abe15-51bc-458e-ad7b-66750967abfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dac8d3b5-451c-4585-9d99-dd68245b5666");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Demandes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c58cbcd-54f2-419d-9e83-c9b284b98c05", null, "agent", "agent" },
                    { "6141d24a-357e-4863-b482-8e0952be660a", null, "utilisateur", "utilisateur" },
                    { "89f26f77-1b86-4bb6-90f7-d8c5e0026a5b", null, "admin", "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demandes_UserId",
                table: "Demandes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_AspNetUsers_UserId",
                table: "Demandes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_AspNetUsers_UserId",
                table: "Demandes");

            migrationBuilder.DropIndex(
                name: "IX_Demandes_UserId",
                table: "Demandes");

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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Demandes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bf55f559-de7f-4375-b6e9-72a4fec91df9", null, "utilisateur", "utilisateur" },
                    { "c96abe15-51bc-458e-ad7b-66750967abfd", null, "admin", "admin" },
                    { "dac8d3b5-451c-4585-9d99-dd68245b5666", null, "agent", "agent" }
                });
        }
    }
}
