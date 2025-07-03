using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagamentoContext.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pagamentoInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    NumeroCartao = table.Column<string>(type: "TEXT", nullable: true),
                    NomeTitular = table.Column<string>(type: "TEXT", nullable: true),
                    ValidadeCartao = table.Column<string>(type: "TEXT", nullable: true),
                    CVV = table.Column<string>(type: "TEXT", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
