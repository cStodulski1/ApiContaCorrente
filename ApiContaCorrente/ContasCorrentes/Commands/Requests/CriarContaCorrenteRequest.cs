using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Requests
{
    public class CriarContaCorrenteRequest : IRequest<CriarContaCorrenteResponse>
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
    }
}
