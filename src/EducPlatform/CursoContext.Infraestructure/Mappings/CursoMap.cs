using CursoContext.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoContext.Infrastructure.Mappings;

public class CursoMap : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.OwnsOne(c => c.Conteudo, conteudo =>
        {
            conteudo.Property(p => p.Descricao)
                .HasColumnName("ConteudoDescricao")
                .IsRequired()
                .HasMaxLength(1000);

            conteudo.Property(p => p.Objetivos)
                .HasColumnName("ConteudoObjetivos")
                .IsRequired()
                .HasMaxLength(1000);
        });

        builder.HasMany(c => c.Aulas)
            .WithOne()
            .HasForeignKey("CursoId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.DataCriacao);
        builder.Property(c => c.DataAtualizacao);
        builder.Property(c => c.UsuarioCriacao).IsRequired();
        builder.Property(c => c.UsuarioAtualizacao).IsRequired();
    }
}
