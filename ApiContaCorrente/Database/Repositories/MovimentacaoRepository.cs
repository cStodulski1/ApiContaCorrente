using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace ApiContaCorrente.Database.Repositories
{
    public class MovimentacaoRepository(IDbConnection dbConnection) : IMovimentacaoRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        public Task<Result> AdicionarMovimentacao(Movimentacao movimentacao)
        {
            string query = @"INSERT INTO Movimento (IdContaCorrente, DataMovimento, TipoMovimento, ValorEmCentavos)
                             VALUES (@IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            _dbConnection.Open();
            using var command = _dbConnection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SQLiteParameter("@IdContaCorrente", movimentacao.IdContaCorrente));
            command.Parameters.Add(new SQLiteParameter("@DataMovimento", movimentacao.DataMovimento.ToString("yyyy-MM-dd HH:mm:ss")));
            command.Parameters.Add(new SQLiteParameter("@TipoMovimento", (int)movimentacao.TipoMovimento));
            command.Parameters.Add(new SQLiteParameter("@Valor", (int)(movimentacao.Valor * 100)));
            var affectedRows = command.ExecuteNonQuery();
            _dbConnection.Close();

            if(affectedRows == 0)
            {
                return Task.FromResult(Result.Failure("Falha ao adicionar a movimentação."));
            }

            return Task.FromResult(Result.Success("Movimentação adicionada com sucesso."));
        }

        public Task<decimal> BuscarValorDeTodasMovimentacoesSomadasPorContaIdETipoMovimento(int idContaCorrente, TipoMovimento tipoMovimento)
        {
            string query = @"SELECT ID as id, IdContaCorrente as idContaCorrente, 
                            DataMovimento as dataMovimento, TipoMovimento as tipoMovimento, ValorEmCentavos as valorEmCentavos
                            FROM Movimento WHERE IdContaCorrente = @IdContaCorrente AND TipoMovimento = @TipoMovimento";

            var tipoMovimentoParametro = (int)tipoMovimento;
            var parametro = new { IdContaCorrente = idContaCorrente, TipoMovimento = tipoMovimentoParametro};

            dbConnection.Open();
            var movimentacoes = dbConnection.Query<Movimentacao>(query, parametro);
            dbConnection.Close();

            decimal valorTotal = 0;

            foreach (var movimentacao in movimentacoes)
            {
                valorTotal += movimentacao.Valor;
            }

            return Task.FromResult(valorTotal);
        }

        public Task<Result<decimal>> BuscarValorDeTodosDebitosSomadosPorContaId(int idContaCorrente)
        {
            throw new NotImplementedException();
        }
    }
}
