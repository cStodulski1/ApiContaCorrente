using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.Models.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Requests
{
    public record InativarContaCorrenteRequest : IRequest<Result>
    {
        public string Senha { get; set; }
        public string Token {  get; set; }
    }
}
