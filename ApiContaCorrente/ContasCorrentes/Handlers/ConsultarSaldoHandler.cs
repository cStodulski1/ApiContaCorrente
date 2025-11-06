using ApiContaCorrente.ContasCorrentes.Queries.Requests;
using ApiContaCorrente.ContasCorrentes.Queries.Responses;
using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace ApiContaCorrente.ContasCorrentes.Handlers
{
    public class ConsultarSaldoHandler(
        IContaCorrenteRepository contaCorrenteRepository,
        IMovimentacaoRepository movimentacaoRepository) : IRequestHandler<ConsultarSaldoRequest, ConsultarSaldoResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository = contaCorrenteRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository = movimentacaoRepository;
        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoRequest request, CancellationToken cancellationToken)
        {
            string numero = TokenHandler.GetClaimValue(request.Token, "numero");

            var contaCorrenteResponse = _contaCorrenteRepository.BuscarContaCorrentePorNumeroOuCpf(numero);

            if (!contaCorrenteResponse.IsSuccess)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INVALID_ACCOUNT;
                var badResponse = new ConsultarSaldoResponse
                {
                    IsSuccess = false,
                    TipoDeFalha = tipoDeFalha,
                    Message = ": Você precisa ter uma conta cadastrada para consultar o saldo."
                };
                return badResponse;
            }

            var contaCorrente = contaCorrenteResponse.Data;
            if (!contaCorrente.Ativo)
            {
                TipoDeFalha tipoDeFalha = TipoDeFalha.INACTIVE_ACCOUNT;
                var badResponse = new ConsultarSaldoResponse 
                {
                    IsSuccess = false,
                    TipoDeFalha = tipoDeFalha,
                    Message = ": Não é possível consultar o saldo de uma conta inativa."
                };
                return badResponse;
            }

            int contaId = contaCorrente.Id;
            var tipoMovimentoCredito = TipoMovimento.CREDITO;
            var tipoMovimentoDebito = TipoMovimento.DEBITO;

            var valorTotalDebitos =
                await _movimentacaoRepository.BuscarValorDeTodasMovimentacoesSomadasPorContaIdETipoMovimento(contaId, tipoMovimentoDebito);

            var valorTotalCreditos =
                await _movimentacaoRepository.BuscarValorDeTodasMovimentacoesSomadasPorContaIdETipoMovimento(contaId, tipoMovimentoCredito);

            var saldo = valorTotalCreditos - valorTotalDebitos;

            var response = new ConsultarSaldoResponse(contaCorrente.Numero, contaCorrente.Nome, saldo);

            return response;
        }
    }
}
