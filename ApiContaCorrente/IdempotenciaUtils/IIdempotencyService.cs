using ApiContaCorrente.Models;

namespace ApiContaCorrente.IdempotenciaUtils
{
    public interface IIdempotencyService
    {
        Task<bool> RequestExistsAsync(Guid requestId);
        Task CreateRequestAsync(Models.Idempotencia idempotencia);
        Task<Idempotencia> RetrieveExistingResponseAsync(Guid requestId);
    }
}
