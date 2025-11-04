using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.Movimentacoes.Commands.Responses
{
    public record RealizarMovimentacaoResponse : Result
    {         
        public TipoDeFalha? TipoDeFalha { get; init; }
    }
}
