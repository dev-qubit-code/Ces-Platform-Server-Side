using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ces_Platform_Server_Side.Data.Migrations
{
    /// <inheritdoc />
    public partial class _Adding_Teacher_Relations_of_Tests_and_Notes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Teachers_TeacherId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Teachers_TeacherId",
                table: "Tests");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Teachers_TeacherId",
                table: "Notes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Teachers_TeacherId",
                table: "Tests",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Teachers_TeacherId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Teachers_TeacherId",
                table: "Tests");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Teachers_TeacherId",
                table: "Notes",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Teachers_TeacherId",
                table: "Tests",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
