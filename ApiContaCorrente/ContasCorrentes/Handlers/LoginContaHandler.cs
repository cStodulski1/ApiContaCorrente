using ApiContaCorrente.Authentication;
using ApiContaCorrente.ContasCorrentes.Queries.Requests;
using ApiContaCorrente.ContasCorrentes.Queries.Responses;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Helpers;
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

            var requestContaCorrente = _repo.BuscarContaCorrentePorNumeroOuCpf(campoLogin);
            var contaCorrente = requestContaCorrente.Data;

            if(!requestContaCorrente.IsSuccess && requestContaCorrente.Data == null)
            {
                var badResponse = new LoginContaResponse()
                {
                    IsSuccess = false,
                    TipoDeFalha = Models.Enums.TipoDeFalha.INVALID_DOCUMENT,
                    Message = requestContaCorrente.Message
                };

                return badResponse;
            }
            
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
            else
            {
                var badResponse = new LoginContaResponse()
                {
                    IsSuccess = false,
                    TipoDeFalha = Models.Enums.TipoDeFalha.USER_UNAUTHORIZED,
                    Message = "Senha incorreta!"
                };
                return badResponse;
            }
        }
    }
}
