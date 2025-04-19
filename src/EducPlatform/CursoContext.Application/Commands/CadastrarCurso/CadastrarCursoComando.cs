namespace CursoContext.Application.Commands.CadastrarCurso;

    public class CadastrarCursoComando
    {
        public string Nome { get; }
        public string Descricao { get; }
        public CadastrarCursoComando(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }
    }
