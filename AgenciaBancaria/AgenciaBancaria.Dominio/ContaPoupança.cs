namespace AgenciaBancaria.Dominio
{
    public class ContaPoupança : ContaBancaria
    {
        public decimal PercentualRendimento { get; private set; }
        public ContaPoupança(Cliente cliente) : base(cliente)
        {
            PercentualRendimento = 0.003M;
        }
    }
}