using BuildingBlocks.Common;
using BuildingBlocks.Results;
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

    public ResultGeneric<Aula> AdicionarAula(string titulo, string conteudo, string usuario)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return ResultGeneric<Aula>.Fail("Título da aula não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(conteudo))
            return ResultGeneric<Aula>.Fail("Conteúdo da aula não pode ser vazio.");

        var aula = new Aula(titulo, conteudo, usuario);
        Aulas.Add(aula);

        return ResultGeneric<Aula>.Ok(aula);
    }

    public void Atualizar(string novoNome, ConteudoProgramatico novoConteudo, string usuario)
    {
        Nome = novoNome;
        Conteudo = novoConteudo;
        Atualizar(usuario);
    }
}
