namespace CursoContext.Application.Commands.EditarCurso;

public class EditarCursoComando
{
    public Guid Id { get; }
    public string NovoNome { get; }
    public string NovaDescricao { get; }

    public EditarCursoComando(Guid id, string novoNome, string novaDescricao)
    {
        Id = id;
        NovoNome = novoNome;
        NovaDescricao = novaDescricao;
    }
}
