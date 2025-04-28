namespace CursoContext.Application.Commands.CadastrarAula;

public class CadastrarAulaComando
{
    public Guid CursoId { get; }
    public string Titulo { get; }
    public string Conteudo { get; }

    public CadastrarAulaComando(Guid cursoId, string titulo, string conteudo)
    {
        CursoId = cursoId;
        Titulo = titulo;
        Conteudo = conteudo;
    }
}
