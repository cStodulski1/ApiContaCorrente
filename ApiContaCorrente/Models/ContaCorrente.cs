namespace ApiContaCorrente.Models
{
    public class ContaCorrente(string numero, string cpf, string hashSenha)
    {
        public int Id { get; private set; }
        public string Numero { get; private set; } = numero;
        public string Cpf { get; private set; } = cpf;
        public string Nome { get; private set; } = "DefaultValue";
        public bool Ativo { get; private set; } = true;
        public string HashSenha { get; private set; } = hashSenha;

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
