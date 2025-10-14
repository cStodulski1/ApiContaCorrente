using ApiContaCorrente.ContasCorrentes.Queries.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Queries.Requests
{
    public class LoginContaRequest : IRequest<LoginContaResponse>  
    {
        public string NumeroOuCpf { get; set; }
        public string Senha {  get; set; }
    }
}
