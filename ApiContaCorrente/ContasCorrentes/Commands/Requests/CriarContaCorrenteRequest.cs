using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.IdempotenciaUtils;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Requests
{
    public class CriarContaCorrenteRequest : IRequest<CriarContaCorrenteResponse>, IIdempotentRequest
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public Guid RequestId { get; set; }
    }
}
