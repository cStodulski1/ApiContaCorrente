namespace ApiContaCorrente.Models
{
    public class ContaCorrente
    {
        public Guid Id { get; private set; }
        public string Numero {  get; private set; }
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public string HashSenha { get; private set; }
        public string Salt { get; private set; }
    }
}
