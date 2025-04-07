using AlunoContext.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AlunoContext.Infrastructure.Mappings;

public class AlunoMap : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.DataCriacao);
        builder.Property(a => a.DataAtualizacao);

        builder.Property(a => a.UsuarioCriacao)
               .IsRequired();

        builder.Property(a => a.UsuarioAtualizacao)
               .IsRequired();

        builder.HasMany(a => a.Matriculas)
       .WithOne()
       .HasForeignKey("AlunoId")
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Certificados)
       .WithOne()
       .HasForeignKey("AlunoId")
       .OnDelete(DeleteBehavior.Cascade);
    }
}
