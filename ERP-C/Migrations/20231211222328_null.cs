using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_C.Migrations
{
    /// <inheritdoc />
    public partial class @null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Telefonos_TelefonoId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_TelefonoId",
                table: "Empresas");

            migrationBuilder.AlterColumn<int>(
                name: "TelefonoId",
                table: "Empresas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_EmpleadoId",
                table: "Telefonos",
                column: "EmpleadoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonos_Personas_EmpleadoId",
                table: "Telefonos",
                column: "EmpleadoId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Telefonos_Personas_EmpleadoId",
                table: "Telefonos");

            migrationBuilder.DropIndex(
                name: "IX_Telefonos_EmpleadoId",
                table: "Telefonos");

            migrationBuilder.AlterColumn<int>(
                name: "TelefonoId",
                table: "Empresas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_TelefonoId",
                table: "Empresas",
                column: "TelefonoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Telefonos_TelefonoId",
                table: "Empresas",
                column: "TelefonoId",
                principalTable: "Telefonos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
