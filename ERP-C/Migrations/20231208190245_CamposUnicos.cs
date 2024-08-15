using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_C.Migrations
{
    /// <inheritdoc />
    public partial class CamposUnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TipoTelefonos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Gerencias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TipoTelefonos_Nombre",
                table: "TipoTelefonos",
                column: "Nombre",
                unique: true,
                filter: "[Nombre] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_Nombre",
                table: "Posiciones",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_Nombre",
                table: "Gerencias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TipoTelefonos_Nombre",
                table: "TipoTelefonos");

            migrationBuilder.DropIndex(
                name: "IX_Posiciones_Nombre",
                table: "Posiciones");

            migrationBuilder.DropIndex(
                name: "IX_Gerencias_Nombre",
                table: "Gerencias");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "TipoTelefonos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Gerencias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
