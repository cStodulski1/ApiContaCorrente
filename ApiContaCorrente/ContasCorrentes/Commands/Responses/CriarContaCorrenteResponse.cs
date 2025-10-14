using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using System.Net;

namespace ApiContaCorrente.ContasCorrentes.Commands.Responses
{
    public class CriarContaCorrenteResponse
    {
        public CriarContaCorrenteResponse() { }
        public CriarContaCorrenteResponse(string numero)
        {
            string message = $"Conta criada com sucesso! Número pra acesso: {numero}";
            Message = message;
        }
        public string Numero { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TipoDeFalha TipoDeFalha { get; set; }
    }
}
