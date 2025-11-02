using ApiContaCorrente.Database.Interfaces;
using ApiContaCorrente.Models;
using ApiContaCorrente.Models.Responses;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContaCorrente.Database.Repositories
{
    public class ContaCorrenteRepository(IDbConnection connection) : IContaCorrenteRepository
    {
        private readonly IDbConnection dbConnection = connection;
        public void Init()
        {
            //salvar essas queries em arquivo separado .sql
            string sqlTableContaCorrente = @"CREATE TABLE IF NOT EXISTS ContaCorrente (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Numero TEXT NOT NULL,
                    Cpf TEXT NOT NULL,
                    Nome TEXT NOT NULL,
                    Ativo INTEGER NOT NULL,
                    HashSenha TEXT NOT NULL);";
            string sqlTableMovimento = @"CREATE TABLE IF NOT EXISTS Movimento (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdContaCorrente INTEGER NOT NULL,
                    DataMovimento TEXT NOT NULL,
                    TipoMovimento INTEGER NOT NULL,
                    Valor INTEGER NOT NULL,
                    FOREIGN KEY(IdContaCorrente) REFERENCES ContaCorrente(Id));";
        }

        public async Task<Result> AddContaCorrente(ContaCorrente contaCorrente)
        {
            var validationResult = ValidateContaCorrente(contaCorrente);
            if (!validationResult.IsSuccess)
                return validationResult;

            try
            {
                string queryAddConta = @"INSERT INTO ContaCorrente (Numero, Cpf, Nome, Ativo, HashSenha)
                                                    VALUES (@Numero, @Cpf, @Nome, @Ativo, @HashSenha)";
                dbConnection.Open();
                dbConnection.Execute(queryAddConta, contaCorrente);
                dbConnection.Close();

                return Result.Success($"Conta criada com número: {contaCorrente.Numero}");
            }
            catch (SQLiteException ex)
            {
                return Result.Failure($"Unexpected error: {ex.Message}");
            }
        }

        private Result ValidateContaCorrente(ContaCorrente conta)
        {
            if (conta == null)
                return Result.Failure("Conta corrente não pode ser nulo");

            if (string.IsNullOrWhiteSpace(conta.Numero))
                return Result.Failure("Número da conta esperado");

            return Result.Success();
        }

        public void Inativar(string numeroDaConta)
        {
            string sqlQuery = "UPDATE ContaCorrente SET Ativo = 0 WHERE Numero = @Numero";
            var parametro = new { Numero = numeroDaConta };
            dbConnection.Open();
            int rowsAffected = dbConnection.Execute(sqlQuery, parametro);
            dbConnection.Close();

            if (rowsAffected <= 0) throw new Exception("Houve um problema ao inativar a conta.");
        }

        public Result<ContaCorrente> BuscarContaCorrentePorNumeroOuCpf(string campoLogin)
        {
            string sqlQuery = @"SELECT ID AS id, Numero AS numero, Cpf AS cpf, Nome AS nome, Ativo AS ativo, HashSenha AS hashSenha 
                                FROM ContaCorrente WHERE Numero = @CampoLogin OR Cpf = @CampoLogin";
            var parametro = new { CampoLogin = campoLogin };

            dbConnection.Open();
            var contaCorrente = dbConnection.QueryFirstOrDefault<ContaCorrente>(sqlQuery, parametro);
            dbConnection.Close();

            if (contaCorrente == null)
            {
                return Result<ContaCorrente>.Failure($"Conta corrente com identificação: {campoLogin} não encontrada!");
            }

            return Result<ContaCorrente>.Success(contaCorrente);
        }
    }
}
