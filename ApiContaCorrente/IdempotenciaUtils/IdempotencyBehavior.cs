using MediatR;
using System.Text.Json;

namespace ApiContaCorrente.IdempotenciaUtils
{
    public class IdempotencyBehavior<TRequest, TResponse>(IIdempotencyService idempotencyService) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IIdempotentRequest, IRequest<TResponse>
    {
        private readonly IIdempotencyService _idempotencyService = idempotencyService;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(await _idempotencyService.RequestExistsAsync(request.RequestId))
            {
                var existingResponse = await _idempotencyService.RetrieveExistingResponseAsync(request.RequestId);
                return JsonSerializer.Deserialize<TResponse>(existingResponse.Response)!;
            }

            var response = await next();

            var idempotenciaRecord = new Models.Idempotencia(request.RequestId, JsonSerializer.Serialize(response));
            await _idempotencyService.CreateRequestAsync(idempotenciaRecord);

            return response;
        }
    }
}
