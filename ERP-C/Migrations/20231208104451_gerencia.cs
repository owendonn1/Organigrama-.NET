using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_C.Migrations
{
    /// <inheritdoc />
    public partial class gerencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Personas_EmpleadoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos");

            migrationBuilder.AddColumn<int>(
                name: "TipoTelefonoId1",
                table: "Telefonos",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Gerencias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Gastos",
                type: "decimal(17,2)",
                precision: 17,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "CentroDeCostos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "GerenciaId",
                table: "CentroDeCostos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos",
                column: "TipoTelefonoId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoTelefonoId1",
                table: "Telefonos",
                column: "TipoTelefonoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Personas_EmpleadoId",
                table: "Gastos",
                column: "EmpleadoId",
                principalTable: "Personas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Telefonos_TipoTelefonos_TipoTelefonoId1",
                table: "Telefonos",
                column: "TipoTelefonoId1",
                principalTable: "TipoTelefonos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Personas_EmpleadoId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Telefonos_TipoTelefonos_TipoTelefonoId1",
                table: "Telefonos");

            migrationBuilder.DropIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos");

            migrationBuilder.DropIndex(
                name: "IX_Telefonos_TipoTelefonoId1",
                table: "Telefonos");

            migrationBuilder.DropColumn(
                name: "TipoTelefonoId1",
                table: "Telefonos");

            migrationBuilder.DropColumn(
                name: "GerenciaId",
                table: "CentroDeCostos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Gerencias",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Gastos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,2)",
                oldPrecision: 17,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "CentroDeCostos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos",
                column: "TipoTelefonoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Personas_EmpleadoId",
                table: "Gastos",
                column: "EmpleadoId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
