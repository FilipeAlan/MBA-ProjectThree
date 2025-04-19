using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.CadastrarAluno;

public class CadastrarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto , IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;        
    }

    public async Task<Result> Handle(CadastrarAlunoComando comando)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return Result.Fail("O nome do aluno é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Email) || !comando.Email.Contains('@'))
            return Result.Fail("O e-mail informado é inválido.");

        var aluno = new Aluno(comando.Nome, comando.Email, _usuarioContexto.ObterUsuario());
        
        await _repositorio.Adicionar(aluno);
        
        await _unitOfWork.Commit();

        return Result.Ok("Aluno cadastrado com sucesso.");
    }

}
