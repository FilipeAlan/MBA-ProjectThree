using AlunoContext.Domain.Entities;
using BuildingBlocks.Common;

namespace AlunoContext.Domain.Aggregates;

public class Aluno : EntityBase
{
    public string Nome { get; private set; }
    public string Email { get; private set; }

    private readonly List<Matricula> _matriculas = new();
    public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();

    private readonly List<Certificado> _certificados = new();
    public IReadOnlyCollection<Certificado> Certificados => _certificados.AsReadOnly();

    protected Aluno() : base("SYSTEM") // Para EF Core
    {
    }
    public Aluno(string nome, string email, string usuarioCriacao)
        : base(usuarioCriacao)
    {
        Nome = nome;
        Email = email;
    }
    public void AdicionarMatricula(Matricula matricula)
    {
        _matriculas.Add(matricula);
    }
    public void AdicionarCertificado(Certificado certificado)
    {
        _certificados.Add(certificado);
    }
    public void AtualizarEmail(string novoEmail, string usuario)
    {
        Email = novoEmail;
        Atualizar(usuario);
    }
    public void AtualizarNome(string novoNome, string usuario)
    {
        Nome = novoNome;
        Atualizar(usuario);
    }
}
