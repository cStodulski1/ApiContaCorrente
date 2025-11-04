using ApiContaCorrente.IdempotenciaUtils;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Movimentacoes.Commands.Responses;
using MediatR;

namespace ApiContaCorrente.Movimentacoes.Commands.Requests
{
    public class RealizarMovimentacaoRequest : IRequest<RealizarMovimentacaoResponse>, IIdempotentRequest
    {
        public Guid RequestId { get; set; }
        public string NumeroDaConta { get; set; }
        public decimal Valor { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
        public string Token { get; set; }
    }
}
