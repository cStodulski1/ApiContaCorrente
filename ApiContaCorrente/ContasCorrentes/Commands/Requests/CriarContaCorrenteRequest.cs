using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.IdempotenciaUtils;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Requests
{
    public record CriarContaCorrenteRequest : IRequest<CriarContaCorrenteResponse>
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
    }
}
