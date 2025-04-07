public class EditarAlunoComando
{
    public Guid Id { get; }
    public string Nome { get; }
    public string Email { get; }

    public EditarAlunoComando(Guid id, string nome, string email)
    {
        Id = id;
        Nome = nome;
        Email = email;
    }
}
