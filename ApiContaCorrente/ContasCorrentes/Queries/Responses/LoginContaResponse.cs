using ApiContaCorrente.Models.Enums;

namespace ApiContaCorrente.ContasCorrentes.Queries.Responses
{
    public class LoginContaResponse
    {
        public string Token { get; set; }
        public TipoDeFalha? TipoDeFalha { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } 
    }
}
