using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaS7.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_DettagliOrdine_ProdottoId",
                table: "DettagliOrdine",
                column: "ProdottoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DettagliOrdine_Prodotti_ProdottoId",
                table: "DettagliOrdine",
                column: "ProdottoId",
                principalTable: "Prodotti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DettagliOrdine_Prodotti_ProdottoId",
                table: "DettagliOrdine");

            migrationBuilder.DropIndex(
                name: "IX_DettagliOrdine_ProdottoId",
                table: "DettagliOrdine");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
