using AlunoContext.Application.Common;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.DeletarAluno
{
    public class DeletarAlunoHandler
    {
        private readonly IAlunoRepository _repositorio;
        private readonly IUsuarioContexto _usuarioContexto;
        public DeletarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto)
        {
            _repositorio = repositorio;
            _usuarioContexto = usuarioContexto;
        }
        public Result Handle(DeletarAlunoComando comando)
        {
            var aluno = _repositorio.ObterPorId(comando.Id);
            if (aluno == null)
                return Result.Fail("Aluno não encontrado.");
            _repositorio.Excluir(aluno);
            return Result.Ok("Aluno removido com sucesso.");
        }
    }
}
