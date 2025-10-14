using ApiContaCorrente.Authentication;
using ApiContaCorrente.ContasCorrentes.Queries.Requests;
using ApiContaCorrente.ContasCorrentes.Queries.Responses;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ApiContaCorrente.ContasCorrentes.Handlers
{
    public class LoginContaHandler(IContaCorrenteRepository contaCorrenteRepository, TokenProvider tokenProvider) : IRequestHandler<LoginContaRequest, LoginContaResponse>
    {
        private readonly IContaCorrenteRepository _repo = contaCorrenteRepository;
        public async Task<LoginContaResponse> Handle(LoginContaRequest request, CancellationToken cancellationToken)
        {
            var campoLogin = request.NumeroOuCpf.Trim().Replace(".", "").Replace("-", "");

            var contaCorrente = _repo.BuscarContaCorrentePorNumeroOuCpf(campoLogin).Data;
            bool senhaCorreta = SenhaEncrypt.VerificarSenha(request.Senha, contaCorrente.HashSenha);

            if (senhaCorreta)
            {
                string token = tokenProvider.Create(contaCorrente);

                var response = new LoginContaResponse()
                {
                    IsSuccess = true,
                    Token = token
                };

                return response;
            }

            var badResponse = new LoginContaResponse()
            {
                IsSuccess = false,
                TipoDeFalha = Models.Enums.TipoDeFalha.USER_UNAUTHORIZED,
                Message = "tá liberado não irmão"
            };
            return badResponse;
        }
    }
}
