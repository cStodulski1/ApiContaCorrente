using ApiContaCorrente.Models.Enums;

namespace ApiContaCorrente.Models
{
    public class Movimentacao
    {
        public Movimentacao(long id, long idContaCorrente, string dataMovimento, long tipoMovimento, long valorEmCentavos) 
        {
            Id = (int)id;
            IdContaCorrente = (int)idContaCorrente;
            DataMovimento = DateTime.Parse(dataMovimento);
            TipoMovimento = (TipoMovimento)tipoMovimento;
            Valor = valorEmCentavos / 100;
        }
        public int Id { get; private set; }
        public int IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public TipoMovimento TipoMovimento { get; private set; }
        public decimal Valor {  get; private set; }
    }
}
