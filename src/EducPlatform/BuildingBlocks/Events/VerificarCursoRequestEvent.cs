
    namespace BuildingBlocks.Events;

public record VerificarCursoRequestEvent(Guid CursoId);
public record VerificarCursoResponseEvent(Guid CursoId, bool Existe);
