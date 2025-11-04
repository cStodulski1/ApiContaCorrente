using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using System.Net;

namespace ApiContaCorrente.ContasCorrentes.Commands.Responses
{
    public record CriarContaCorrenteResponse : Result
    {
        public CriarContaCorrenteResponse() { }
        public CriarContaCorrenteResponse(string numero)
        {
            string message = $"Conta criada com sucesso! Número pra acesso: {numero}";
            Message = message;
            IsSuccess = true;
            TipoDeFalha = TipoDeFalha.SEM_FALHA;
        }
        public string Numero { get; set; }
        public TipoDeFalha TipoDeFalha { get; set; }
    }
}
