namespace CursoContext.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Descricao { get; private set; }
    public string Objetivos { get; private set; }

    protected ConteudoProgramatico() { }

    public ConteudoProgramatico(string descricao, string objetivos)
    {
        Descricao = descricao;
        Objetivos = objetivos;
    }

    public ConteudoProgramatico Atualizar(string novaDescricao, string novosObjetivos)
    {
        return new ConteudoProgramatico(novaDescricao, novosObjetivos);
    }
}
