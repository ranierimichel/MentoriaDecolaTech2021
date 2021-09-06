using System;
using System.Text;

namespace AgenciaBancaria.Dominio
{
    public class ContaCorrente : ContaBancaria
    {
        public decimal ValorTaxaManutencao { get; private set; }
        public decimal Limite { get; private set; }

        public ContaCorrente(Cliente cliente, decimal limite) : base(cliente)
        {
            ValorTaxaManutencao = 0.05M;
            Limite = limite;
        }

        public override void Sacar(decimal valor, string senha)
        {
            var valorMaximoSaque = Saldo + Limite;
            var saque = new Saque(valor, DateTime.Now, this);

            if (Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta encerrada!");

            if (Senha != senha)
                throw new Exception("Senha invalida!");

            if (valorMaximoSaque < saque.Valor)
                throw new Exception("Saldo insuficiente!");

            Saldo -= saque.Valor;
            Lancamentos.Add(saque);
        }

        public override void Transferir(decimal valor, ContaBancaria conta, string senha)
        {
            var valorMaximoSaque = Saldo + Limite;
            if (Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta encerrada!");

            if (conta.Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta de destino encerrada!");

            if (Senha != senha)
                throw new Exception("Senha invalida!");

            if (valorMaximoSaque < valor)
                throw new Exception("Saldo insuficiente!");

            var saque = new Saque(valor, DateTime.Now, this);
            Saldo -= saque.Valor;
            Lancamentos.Add(saque);

            conta.Depositar(valor);
        }

        public override string VerExtrato()
        {
            var sb = new StringBuilder();

            sb.Append(base.VerExtrato());

            sb.AppendLine($"Limite:         R${Limite}");
            sb.AppendLine($"Saldo + Limite: R$ {Limite + Saldo}");

            return sb.ToString();
        }
    }
}