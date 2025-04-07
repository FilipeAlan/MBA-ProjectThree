using AlunoContext.Domain.Aggregates;

namespace AlunoContext.Testes.Shared.Builders;

public class AlunoBuilder
{
    private string _nome = "AlunoTesteNome";
    private string _email = "AlunoTeste@email.com";
    private string _usuario = "AlunoTeste";

    public static AlunoBuilder Novo()
    {
        return new AlunoBuilder();
    }

    public AlunoBuilder ComNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public AlunoBuilder ComEmail(string email)
    {
        _email = email;
        return this;
    }

    public AlunoBuilder CriadoPor(string usuario)
    {
        _usuario = usuario;
        return this;
    }

    public Aluno Construir()
    {
        return new Aluno(_nome, _email, _usuario);
    }
}
