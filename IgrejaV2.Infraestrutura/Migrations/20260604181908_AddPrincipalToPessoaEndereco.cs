using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgrejaV2.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class AddPrincipalToPessoaEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Principal",
                table: "pessoas_enderecos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Principal",
                table: "pessoas_enderecos");
        }
    }
}
