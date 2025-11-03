namespace ApiContaCorrente.IdempotenciaUtils
{
    public interface IIdempotentRequest
    {
        Guid RequestId { get; }
    }
}
