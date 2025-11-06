using ApiContaCorrente.ContasCorrentes.Queries.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Queries.Requests
{
    public class ConsultarSaldoRequest : IRequest<ConsultarSaldoResponse>
    {
        public string Token { get; set; }
    }
}
