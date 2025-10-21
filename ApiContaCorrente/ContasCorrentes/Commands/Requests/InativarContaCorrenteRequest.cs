using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Requests
{
    public class InativarContaCorrenteRequest : IRequest<InativarContaCorrenteResponse>
    {
        public string Senha { get; set; }
        public string Token {  get; set; }
    }
}
