namespace CursoContext.Application.Dto;

public class CursoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }

    public CursoDto(Guid id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }
}