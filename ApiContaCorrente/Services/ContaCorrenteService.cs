using ApiContaCorrente.Helpers;
using ApiContaCorrente.Interfaces;
using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Dto;
using ApiContaCorrente.Models.Responses;
using System.Runtime.CompilerServices;

namespace ApiContaCorrente.Services
{
    public class ContaCorrenteService(IContaCorrenteRepository contaCorrenteRepository) : IContaCorrenteService
    {
        private readonly IContaCorrenteRepository _repo = contaCorrenteRepository;
        public async Task<Result> CriarContaCorrente(CriarContaDto contaDto)
        {
            string numeroContaCorrente = GerarNumeroContaCorrente();
            string hashSenha = SenhaEncrypt.EncriptarSenha(contaDto.Senha);
            ContaCorrente contaCorrente = new(numeroContaCorrente, contaDto.Cpf, hashSenha);

            if (!string.IsNullOrEmpty(contaDto.Nome)) contaCorrente.AdicionarNome(contaDto.Nome);

            var result = await _repo.AddContaCorrente(contaCorrente);
            if (!result.IsSuccess)
            {
                return Result<string>.Failure(result.Message);
            }

            return result;
        }

        private static string GerarNumeroContaCorrente()
        {
            Random random = new Random();
            int randomValue = random.Next(0, 100000000);

            return randomValue.ToString("D8");
        }
    }
}
