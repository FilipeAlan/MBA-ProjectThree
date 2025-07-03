namespace BuildingBlocks.Messagings
{
    public class RequestEnvelope<T>
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
        public T Payload { get; set; } = default!;
    }
}
