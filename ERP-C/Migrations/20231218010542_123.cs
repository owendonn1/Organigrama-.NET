using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_C.Migrations
{
    /// <inheritdoc />
    public partial class _123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaId",
                table: "Gerencias");

            migrationBuilder.RenameColumn(
                name: "GerenciaId",
                table: "Gerencias",
                newName: "GerenciaSuperiorId");

            migrationBuilder.RenameIndex(
                name: "IX_Gerencias_GerenciaId",
                table: "Gerencias",
                newName: "IX_Gerencias_GerenciaSuperiorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaSuperiorId",
                table: "Gerencias",
                column: "GerenciaSuperiorId",
                principalTable: "Gerencias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaSuperiorId",
                table: "Gerencias");

            migrationBuilder.RenameColumn(
                name: "GerenciaSuperiorId",
                table: "Gerencias",
                newName: "GerenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Gerencias_GerenciaSuperiorId",
                table: "Gerencias",
                newName: "IX_Gerencias_GerenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaId",
                table: "Gerencias",
                column: "GerenciaId",
                principalTable: "Gerencias",
                principalColumn: "Id");
        }
    }
}
