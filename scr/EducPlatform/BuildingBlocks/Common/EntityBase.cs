namespace BuildingBlocks.Common;

public abstract class EntityBase
{
    public Guid Id { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public string UsuarioCriacao { get; private set; }
    public string UsuarioAtualizacao { get; private set; }

    protected EntityBase(string usuarioCriacao)
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        DataAtualizacao = DateTime.UtcNow;        
        UsuarioCriacao = usuarioCriacao;
        UsuarioAtualizacao = usuarioCriacao;
    }

    public void Atualizar(string usuario)
    {
        UsuarioAtualizacao = usuario;
        DataAtualizacao = DateTime.UtcNow;
    }

    public override bool Equals(object obj)
    {
        if (obj is not EntityBase other) return false;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityBase a, EntityBase b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Id == b.Id;
    }

    public static bool operator !=(EntityBase a, EntityBase b)
    {
        return !(a == b);
    }
}
