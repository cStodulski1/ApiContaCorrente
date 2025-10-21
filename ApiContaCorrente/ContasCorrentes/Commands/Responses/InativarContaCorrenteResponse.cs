using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Commands.Responses
{
    public class InativarContaCorrenteResponse(bool sucesso)
    {
        public bool IsSuccess { get; set; } = sucesso;
    }
}
