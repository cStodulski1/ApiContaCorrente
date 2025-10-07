namespace ApiContaCorrente.Models.Dto
{
    public class ContaCorrenteDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string Senha { get; set; }
    }
}
