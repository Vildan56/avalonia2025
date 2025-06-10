using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project2025.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId1",
                table: "PropertyObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyObjects_PropertyTypeId1",
                table: "PropertyObjects",
                column: "PropertyTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyObjects_PropertyTypes_PropertyTypeId1",
                table: "PropertyObjects",
                column: "PropertyTypeId1",
                principalTable: "PropertyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyObjects_PropertyTypes_PropertyTypeId1",
                table: "PropertyObjects");

            migrationBuilder.DropIndex(
                name: "IX_PropertyObjects_PropertyTypeId1",
                table: "PropertyObjects");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId1",
                table: "PropertyObjects");
        }
    }
}
