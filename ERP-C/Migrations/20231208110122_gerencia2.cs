using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_C.Migrations
{
    /// <inheritdoc />
    public partial class gerencia2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CentroDeCostos_Gerencias_GerencaId",
                table: "CentroDeCostos");

            migrationBuilder.DropIndex(
                name: "IX_CentroDeCostos_GerencaId",
                table: "CentroDeCostos");

            migrationBuilder.DropColumn(
                name: "GerencaId",
                table: "CentroDeCostos");

            migrationBuilder.CreateIndex(
                name: "IX_CentroDeCostos_GerenciaId",
                table: "CentroDeCostos",
                column: "GerenciaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CentroDeCostos_Gerencias_GerenciaId",
                table: "CentroDeCostos",
                column: "GerenciaId",
                principalTable: "Gerencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CentroDeCostos_Gerencias_GerenciaId",
                table: "CentroDeCostos");

            migrationBuilder.DropIndex(
                name: "IX_CentroDeCostos_GerenciaId",
                table: "CentroDeCostos");

            migrationBuilder.AddColumn<int>(
                name: "GerencaId",
                table: "CentroDeCostos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CentroDeCostos_GerencaId",
                table: "CentroDeCostos",
                column: "GerencaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CentroDeCostos_Gerencias_GerencaId",
                table: "CentroDeCostos",
                column: "GerencaId",
                principalTable: "Gerencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
