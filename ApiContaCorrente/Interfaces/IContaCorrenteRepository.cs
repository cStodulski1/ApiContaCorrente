using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.Interfaces
{
    public interface IContaCorrenteRepository
    {
        public Task<Result> AddContaCorrente(ContaCorrente contaCorrente);
    }
}
