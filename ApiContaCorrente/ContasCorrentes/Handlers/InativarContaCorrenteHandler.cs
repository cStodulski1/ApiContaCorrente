using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using ApiContaCorrente.ContasCorrentes.Commands.Responses;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using ApiContaCorrente.Movimentacoes.Commands.Responses;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace ApiContaCorrente.ContasCorrentes.Handlers
{
    public class InativarContaCorrenteHandler(IContaCorrenteRepository repo) : IRequestHandler<InativarContaCorrenteRequest, Result>
    {
        private readonly IContaCorrenteRepository _repo = repo;
        public async Task<Result> Handle(InativarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(request.Token) as JwtSecurityToken;

            string numero = jsonToken.Claims.FirstOrDefault(c => c.Type == "numero").Value;

            var contaCorrenteResponse = _repo.BuscarContaCorrentePorNumeroOuCpf(numero);
            if (!contaCorrenteResponse.IsSuccess)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_ACCOUNT;
                return Result.Failure(tipoDeFalha.ToString() + ": Conta corrente não cadastrada");
            }

            var contaCorrente = contaCorrenteResponse.Data;

            bool senhaCorreta = SenhaEncrypt.VerificarSenha(request.Senha, contaCorrente.HashSenha);

            if (senhaCorreta)
            {
                _repo.Inativar(numero);
            }

            if(senhaCorreta)
            {
                Console.WriteLine($"Conta {numero} inativada com sucesso.");
                return Result.Success();
            }
            else
            {
                Console.WriteLine($"Falha ao inativar a conta {numero}: senha incorreta.");
                return Result.Failure("Senha incorreta");
            }
        }
    }
}
