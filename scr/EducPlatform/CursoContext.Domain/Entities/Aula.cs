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
        Titulo = titulo;
        Conteudo = conteudo;
    }

    public void AtualizarConteudo(string novoConteudo, string usuario)
    {
        Conteudo = novoConteudo;
        Atualizar(usuario);
    }
}
