using AlunoContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlunoContext.Infrastructure.Mappings
{
    public class CertificadoMap : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.ToTable("Certificados");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CodigoVerificacao)
                .IsRequired();

            builder.Property(c => c.DataEmissao)
                .IsRequired();

            builder.Property(c => c.MatriculaId)
                .IsRequired();

            builder.Property(c => c.AlunoId)
                .IsRequired();

            builder.HasOne(c => c.Aluno)
                .WithMany(a => a.Certificados)
                .HasForeignKey(c => c.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
