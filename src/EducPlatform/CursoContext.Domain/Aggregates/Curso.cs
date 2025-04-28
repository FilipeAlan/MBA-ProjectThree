using BuildingBlocks.Common;
using CursoContext.Domain.Entities;
using CursoContext.Domain.ValueObjects;

namespace CursoContext.Domain.Aggregates;

public class Curso : EntityBase
{
    public string Nome { get; private set; }
    public ConteudoProgramatico Conteudo { get; private set; }  
    public List<Aula> Aulas { get; private set; } = new();
    protected Curso() : base("SYSTEM") { }

    public Curso(string nome, ConteudoProgramatico conteudo, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        Nome = nome;
        Conteudo = conteudo;
    }

    public void AdicionarAula(string titulo, string conteudo, string usuario)
    {
        var aula = new Aula(titulo, conteudo, usuario);
        Aulas.Add(aula);
        Atualizar(usuario);
    }

    public void Atualizar(string novoNome, ConteudoProgramatico novoConteudo, string usuario)
    {
        Nome = novoNome;
        Conteudo = novoConteudo;
        Atualizar(usuario);
    }
}
