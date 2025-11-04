using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.Database.Interfaces
{
    public interface IMovimentacaoRepository
    {
        public Task<Result> AdicionarMovimentacao(Movimentacao movimentacao);
    }
}
