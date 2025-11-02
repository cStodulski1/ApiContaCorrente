namespace ApiContaCorrente.Models
{
    public class Idempotencia(Guid requestId, string response)
    {
        public Guid RequestId { get; private set; } = requestId;
        public string Response { get; private set; } = response;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
