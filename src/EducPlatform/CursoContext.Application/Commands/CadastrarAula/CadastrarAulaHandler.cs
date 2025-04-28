using BuildingBlocks.Common;
using BuildingBlocks.Results;
using CursoContext.Domain.Entities;
using CursoContext.Domain.Repositories;

namespace CursoContext.Application.Commands.CadastrarAula;

public class CadastrarAulaHandler
{
    private readonly ICursoRepository _cursoRepositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarAulaHandler(ICursoRepository cursoRepositorio, IUsuarioContexto usuarioContexto, IUnitOfWork unitOfWork)
    {
        _cursoRepositorio = cursoRepositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CadastrarAulaComando comando)
    {
        var curso = await _cursoRepositorio.ObterPorId(comando.CursoId);
        if (curso == null)
            return Result.Fail("Curso não encontrado");

        if (string.IsNullOrWhiteSpace(comando.Titulo))
            return Result.Fail("Título da aula não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(comando.Conteudo))
            return Result.Fail("Conteúdo da aula não pode ser vazio.");

        var aula = new Aula(comando.Titulo, comando.Conteudo, _usuarioContexto.ObterUsuario());

        curso.Aulas.Add(aula);
        await _cursoRepositorio.AdicionarAula(aula);
        await _unitOfWork.Commit();

        return Result.Ok("Aula cadastrada com sucesso");
    }
}
