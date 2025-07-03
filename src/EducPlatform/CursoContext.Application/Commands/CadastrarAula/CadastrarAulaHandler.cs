// CadastrarAulaHandler.cs
using CursoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;
using MediatR;
using CursoContext.Infrastructure.Context;

namespace CursoContext.Application.Commands.CadastrarAula;

public class CadastrarAulaHandler : IRequestHandler<CadastrarAulaComando, Result>
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly ICursoUnityOfWork _unitOfWork;

    public CadastrarAulaHandler(ICursoRepository cursoRepository, IUsuarioContexto usuarioContexto, ICursoUnityOfWork unitOfWork)
    {
        _cursoRepository = cursoRepository;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CadastrarAulaComando comando, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterPorId(comando.CursoId);

        if (curso == null)
            return Result.Fail("Curso não encontrado.");

        var resultado = curso.AdicionarAula(comando.Titulo, comando.Conteudo, _usuarioContexto.ObterUsuario());
        if (!resultado.Sucesso)
            return Result.Fail(resultado.Mensagem!); // converte para Result simples

        var aula = resultado.Dados!;
        await _cursoRepository.AdicionarAula(aula);

        curso.Atualizar(_usuarioContexto.ObterUsuario());

        await _unitOfWork.Commit();

        return Result.Ok("Aula cadastrada com sucesso.");

    }
}
