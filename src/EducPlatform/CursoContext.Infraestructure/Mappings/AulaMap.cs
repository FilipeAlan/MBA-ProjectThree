using CursoContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoContext.Infrastructure.Mappings;

public class AulaMap : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Conteudo)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(a => a.UsuarioCriacao)
            .IsRequired();

        builder.Property(a => a.UsuarioAtualizacao)
            .IsRequired();

        builder.Property(a => a.DataCriacao)
            .IsRequired();

        builder.Property(a => a.DataAtualizacao)
            .IsRequired();
       
    }
}
