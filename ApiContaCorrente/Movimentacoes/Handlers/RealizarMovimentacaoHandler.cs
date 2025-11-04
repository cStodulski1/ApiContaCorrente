using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using ApiContaCorrente.Movimentacoes.Commands.Requests;
using ApiContaCorrente.Movimentacoes.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiContaCorrente.Movimentacoes.Handlers
{
    public class RealizarMovimentacaoHandler(
        IMovimentacaoRepository movimentacaoRepository,
        IContaCorrenteRepository contaCorrenteRepository
        ) : IRequestHandler<RealizarMovimentacaoRequest, RealizarMovimentacaoResponse>
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository = movimentacaoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository = contaCorrenteRepository;
        public async Task<RealizarMovimentacaoResponse> Handle(RealizarMovimentacaoRequest request, CancellationToken cancellationToken)
        {
            string numeroDaConta = request.NumeroDaConta;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(request.Token) as JwtSecurityToken;
            var numeroDaContaDoToken = jsonToken.Claims.FirstOrDefault(c => c.Type == "numero").Value;

            if(string.IsNullOrEmpty(numeroDaConta) || numeroDaConta == "string")
            {
                numeroDaConta = numeroDaContaDoToken;
            }

            var contaCorrenteResponse = _contaCorrenteRepository.BuscarContaCorrentePorNumeroOuCpf(numeroDaConta);

            if(!contaCorrenteResponse.IsSuccess)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_ACCOUNT;
                return new RealizarMovimentacaoResponse
                {
                    IsSuccess = false,
                    Message = "Conta corrente não cadastrada.",
                    TipoDeFalha = tipoDeFalha
                };
            }

            var contaCorrente = contaCorrenteResponse.Data;

            if(!contaCorrente.Ativo)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INACTIVE_ACCOUNT;
                return new RealizarMovimentacaoResponse
                {
                    IsSuccess = false,
                    Message = "Conta corrente inativa.",
                    TipoDeFalha = tipoDeFalha
                };
            }

            if(request.Valor < 0)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_VALUE;
                return new RealizarMovimentacaoResponse
                {
                    IsSuccess = false,
                    Message = "Valor da movimentação inválido.",
                    TipoDeFalha = tipoDeFalha
                };
            }

            bool tipoMovimentacaoValida = request.TipoMovimento == TipoMovimento.CREDITO ||
                                           request.TipoMovimento == TipoMovimento.DEBITO;

            if (!tipoMovimentacaoValida)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_TYPE;
                return new RealizarMovimentacaoResponse
                {
                    IsSuccess = false,
                    Message = "Tipo de movimentação inválida",
                    TipoDeFalha = tipoDeFalha
                };
            }

            if(request.TipoMovimento == TipoMovimento.DEBITO && numeroDaConta != numeroDaContaDoToken)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_TYPE;
                return new RealizarMovimentacaoResponse
                {
                    IsSuccess = false,
                    Message = "Movimentação do tipo débito só pode ser feita pelo titular da conta.",
                    TipoDeFalha = tipoDeFalha
                };
            }

            var movimentacao = new Movimentacao(request, contaCorrente.Id);
            var movimentacaoResponse = await _movimentacaoRepository.AdicionarMovimentacao(movimentacao);

            if(!movimentacaoResponse.IsSuccess)
            {
                return new RealizarMovimentacaoResponse
                { 
                    IsSuccess = movimentacaoResponse.IsSuccess,
                    Message = movimentacaoResponse?.Message,
                };
            }

            return new RealizarMovimentacaoResponse{ IsSuccess = true };
        }
    }
}
