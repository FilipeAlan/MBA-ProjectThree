using CursoContext.Domain.Aggregates;
using CursoContext.Domain.ValueObjects;

namespace CursoContext.Tests.Shared.Builders;

public class CursoBuilder
{
    private string _nome = "Curso Padrão";
    private string _descricao = "Conteúdo padrão";
    private string _objetivos = "Objetivos padrão";
    private string _usuario = "TDD";

    public static CursoBuilder Novo() => new();

    public CursoBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }
    public CursoBuilder ComDescricao(string descricao)
    {
        _descricao = descricao;
        return this;
    }
    public CursoBuilder ComObjetivos(string objetivos)
    {
        _objetivos = objetivos;
        return this;
    }
    public CursoBuilder ComUsuario(string usuario)
    {
        _usuario = usuario;
        return this;
    }
    public Curso Construir()
    {
        var conteudo = new ConteudoProgramatico(_descricao, _objetivos);
        return new Curso(_nome, conteudo, _usuario);
    }
}
