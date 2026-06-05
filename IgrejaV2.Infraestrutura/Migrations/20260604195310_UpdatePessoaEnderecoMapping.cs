using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgrejaV2.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePessoaEnderecoMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Principal",
                table: "pessoas_enderecos",
                newName: "principal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "principal",
                table: "pessoas_enderecos",
                newName: "Principal");
        }
    }
}
