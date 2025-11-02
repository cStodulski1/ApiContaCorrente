using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContaCorrente.Database.Interfaces
{
    public interface IContaCorrenteRepository
    {
        public void Init();
        public Task<Result> AddContaCorrente(ContaCorrente contaCorrente);
        public void Inativar(string numeroDaConta);
        public Result<ContaCorrente> BuscarContaCorrentePorNumeroOuCpf(string campoLogin);
    }
}
