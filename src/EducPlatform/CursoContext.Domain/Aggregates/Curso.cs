using BuildingBlocks.Common;
using CursoContext.Domain.ValueObjects;

namespace CursoContext.Domain.Entities;

public class Curso : EntityBase
{
    public string Nome { get; private set; }
    public ConteudoProgramatico Conteudo { get; private set; }

    private readonly List<Aula> _aulas = new();
    public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();
    protected Curso() : base("SYSTEM") { }
    public Curso(string nome, ConteudoProgramatico conteudo, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        Nome = nome;
        Conteudo = conteudo;
    }
    public void AdicionarAula(Aula aula)
    {
        _aulas.Add(aula);
    }
}
