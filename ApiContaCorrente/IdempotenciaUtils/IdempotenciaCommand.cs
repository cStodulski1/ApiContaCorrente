using MediatR;

namespace ApiContaCorrente.IdempotenciaUtils
{
    public abstract record IdempotenciaCommand(Guid RequestId) : IRequest;
}
