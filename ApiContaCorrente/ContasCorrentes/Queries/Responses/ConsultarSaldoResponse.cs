using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.ContasCorrentes.Queries.Responses
{
    public record ConsultarSaldoResponse : Result
    {
        public ConsultarSaldoResponse() { }
        public ConsultarSaldoResponse(string numero, string nome, decimal saldo)
        {
            NumeroDaConta = numero;
            NomeDoTitular = nome;
            DataEHora = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm");
            ValorSaldoAtual = saldo.ToString("F2");
            IsSuccess = true;
        }
        public string NumeroDaConta {  get; set; }
        public string NomeDoTitular { get; set; }
        public string DataEHora { get; set; }
        public string ValorSaldoAtual { get; set; }
        public TipoDeFalha? TipoDeFalha { get; set; }
    }
}
