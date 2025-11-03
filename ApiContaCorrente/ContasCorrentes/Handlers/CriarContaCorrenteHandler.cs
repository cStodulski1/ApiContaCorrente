using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.IdempotenciaUtils;
using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Responses;
using MediatR;

namespace ApiContaCorrente.ContasCorrentes.Handlers
{
    public class CriarContaCorrenteHandler(IContaCorrenteRepository contaCorrenteRepository) : IRequestHandler<CriarContaCorrenteRequest, CriarContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _repo = contaCorrenteRepository;

        public async Task<CriarContaCorrenteResponse> Handle(CriarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            string cpf = request.Cpf;
            bool cpfValido = CpfHelper.IsCpfValid(cpf);
            if (!cpfValido)
            {
                var response = new CriarContaCorrenteResponse
                {
                    IsSuccess = false,
                    TipoDeFalha = Models.Enums.TipoDeFalha.INVALID_DOCUMENT,
                    Message = $"INVALID_DOCUMENT: Cpf: {cpf} é um cpf inválido"
                };

                return response;
            }

            string hashSenha = SenhaEncrypt.EncriptarSenha(request.Senha);
            string numeroDaContaCorrente = GerarNumeroContaCorrente();
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            ContaCorrente novaContaCorrente = new(numeroDaContaCorrente, cpf, hashSenha);

            if (!string.IsNullOrEmpty(request.Nome)) novaContaCorrente.AdicionarNome(request.Nome);

            var resultRepo = await _repo.AddContaCorrente(novaContaCorrente);

            if (!resultRepo.IsSuccess) 
            {
                var response = new CriarContaCorrenteResponse
                {
                    IsSuccess = false,
                    Message = resultRepo.Message
                };
                return response;
            }

            var result = new CriarContaCorrenteResponse(novaContaCorrente.Numero);
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
