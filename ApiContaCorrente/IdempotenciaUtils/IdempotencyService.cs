using ApiContaCorrente.Models;
using StackExchange.Redis;

namespace ApiContaCorrente.IdempotenciaUtils
{
    public class IdempotencyService(IDatabase redis) : IIdempotencyService
    {
        private readonly IDatabase _redis = redis;
        public async Task CreateRequestAsync(Idempotencia idempotencia)
        {
            await _redis.StringSetAsync(idempotencia.RequestId.ToString(), idempotencia.Response, TimeSpan.FromHours(12));
        }

        public async Task<bool> RequestExistsAsync(Guid requestId)
        {
            var value = await _redis.StringGetAsync(requestId.ToString());

            if (value.HasValue)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public async Task<Idempotencia> RetrieveExistingResponseAsync(Guid requestId)
        {
            var value = await _redis.StringGetAsync(requestId.ToString());
            if (value.HasValue)
            {
                var idempotencia = new Idempotencia(requestId, value.ToString());
                return idempotencia;
            }

            return null;
        }
    }
}
