namespace ApiContaCorrente.Models
{
    public class ContaCorrente
    {
        public ContaCorrente(string numero, string cpf, string hashSenha)
        {
            Numero = numero;
            Cpf = cpf;
            HashSenha = hashSenha;
            Nome = "DefaultName";
            Ativo = true;
        }
        public ContaCorrente(long id, string numero, string cpf, string nome, long ativo, string hashSenha)
        {
            bool ativoBool;
            if (ativo == 1)
            {
                ativoBool = true;
            }
            else
            {
                ativoBool = false;
            }

            Id = (int)id;
            Numero = numero;
            Cpf = cpf;
            Nome = nome;
            Ativo = ativoBool;
            HashSenha = hashSenha;
        }
        public int Id { get; private set; }
        public string Numero { get; private set; }
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public string HashSenha { get; private set; }

        public void AdicionarId(int id)
        {
            Id = id;
        }
        public void AdicionarNome(string nome)
        {
            Nome = nome;
        }
    }
}
