using BuildingBlocks.Common;

namespace CursoContext.Domain.Entities;

public class Aula : EntityBase
{
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }

    protected Aula() : base("SYSTEM") { }

    public Aula(string titulo, string conteudo, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título da aula não pode ser vazio.", nameof(titulo));

        if (string.IsNullOrWhiteSpace(conteudo))
            throw new ArgumentException("Conteúdo da aula não pode ser vazio.", nameof(conteudo));

        Titulo = titulo;
        Conteudo = conteudo;
    }
}

