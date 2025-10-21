using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Dto;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.Interfaces
{
    public interface IContaCorrenteRepository
    {
        public void Init();
        public Task<Result> AddContaCorrente(ContaCorrente contaCorrente);
        public void Inativar(string numeroDaConta);
        public Result<ContaCorrente> BuscarContaCorrentePorNumeroOuCpf(string campoLogin);
    }
}
