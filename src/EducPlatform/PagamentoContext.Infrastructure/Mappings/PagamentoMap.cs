using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagamentoContext.Domain.Aggregates;

namespace PagamentoContext.Infrastructure.Context.Mappings;

public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.MatriculaId)
            .IsRequired();

        builder.Property(p => p.Valor)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.DataPagamento)
            .IsRequired();

        builder.OwnsOne(p => p.DadosCartao, dc =>
        {
            dc.Property(c => c.Numero)
                .HasColumnName("NumeroCartao")
                .IsRequired();

            dc.Property(c => c.NomeTitular)
                .HasColumnName("NomeTitular")
                .IsRequired();

            dc.Property(c => c.Validade)
                .HasColumnName("ValidadeCartao")
                .IsRequired();

            dc.Property(c => c.CVV)
                .HasColumnName("CVV")
                .IsRequired();
        });
    }
}