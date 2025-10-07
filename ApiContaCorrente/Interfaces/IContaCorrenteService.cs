using ApiContaCorrente.Models.Dto;
using ApiContaCorrente.Models.Responses;

namespace ApiContaCorrente.Interfaces
{
    public interface IContaCorrenteService
    {
        public Task<Result> CriarContaCorrente(CriarContaDto contaDto);
    }
}
