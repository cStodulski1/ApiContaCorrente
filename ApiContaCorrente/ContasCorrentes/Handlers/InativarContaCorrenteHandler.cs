using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Helpers;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace ApiContaCorrente.ContasCorrentes.Handlers
{
    public class InativarContaCorrenteHandler(IContaCorrenteRepository repo) : IRequestHandler<InativarContaCorrenteRequest, InativarContaCorrenteResponse>
    {
        private readonly IContaCorrenteRepository _repo = repo;
        public async Task<InativarContaCorrenteResponse> Handle(InativarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(request.Token) as JwtSecurityToken;

            string numero = jsonToken.Claims.FirstOrDefault(c => c.Type == "numero").Value;
            var contaCorrenteRequest = _repo.BuscarContaCorrentePorNumeroOuCpf(numero);
            var contaCorrente = contaCorrenteRequest.Data;

            bool senhaCorreta = SenhaEncrypt.VerificarSenha(request.Senha, contaCorrente.HashSenha);

            if (senhaCorreta)
            {
                _repo.Inativar(numero);
            }

            return new InativarContaCorrenteResponse(senhaCorreta);
        }
    }
}
