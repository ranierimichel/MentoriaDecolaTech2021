using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AgenciaBancaria.Dominio
{
    public abstract class ContaBancaria
    {
        public int NumeroConta { get; init; }
        public int DigitoVerificador { get; init; }
        public decimal Saldo { get; protected set; }
        public DateTime? DataAbertura { get; private set; }
        public DateTime? DataEncerramento { get; private set; }
        public SituacaoConta Situacao { get; private set; }
        protected string Senha { get; private set; }
        public Cliente Cliente { get; init; }

        public List<Lancamento> Lancamentos { get; private set; }

        public ContaBancaria(Cliente cliente)
        {
            Random random = new Random();
            NumeroConta = random.Next(50000, 100000);
            DigitoVerificador = random.Next(0, 9);

            Situacao = SituacaoConta.Criada;

            Cliente = cliente ?? throw new Exception("Cliente deve ser informado!");
        }

        public void Abrir(string senha)
        {
            SetaSenha(senha);

            Situacao = SituacaoConta.Aberta;
            DataAbertura = DateTime.Now;
            Lancamentos = new List<Lancamento>();
        }

        public void Encerrar(string senha)
        {
            if (Senha != senha)
                throw new Exception("Senha invalida!");

            if (Saldo != 0)
                throw new Exception("A conta deve estar com saldo zerado!");

            DataEncerramento = DateTime.Now;
            Situacao = SituacaoConta.Encerrada;
        }

        private void SetaSenha(string senha)
        {
            senha = senha.ValidaStringVazia();

            // Minimum eight characters, at least one letter and one number
            if (!Regex.IsMatch(senha, @"^(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
            {
                throw new Exception("Senha invalida!");
            }

            Senha = senha;
        }

        public void Depositar(decimal valor)
        {
            if (Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta encerrada!");

            var deposito = new Deposito(valor, DateTime.Now, this);
            Saldo += deposito.Valor;
            Lancamentos.Add(deposito);
        }

        public virtual void Sacar(decimal valor, string senha)
        {
            if (Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta encerrada!");

            if (Senha != senha)
                throw new Exception("Senha invalida!");

            if (Saldo < valor)
                throw new Exception("Saldo insuficiente!");

            var saque = new Saque(valor, DateTime.Now, this);

            Saldo -= saque.Valor;
            Lancamentos.Add(saque);
        }

        public virtual void Transferir(decimal valor, ContaBancaria conta, string senha)
        {
            if (Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta encerrada!");

            if (conta.Situacao == SituacaoConta.Encerrada)
                throw new Exception("Conta de destino encerrada!");

            if (Senha != senha)
                throw new Exception("Senha invalida!");

            if (Saldo < valor)
                throw new Exception("Saldo insuficiente!");

            var saque = new Saque(valor, DateTime.Now, this);
            Saldo -= saque.Valor;
            Lancamentos.Add(saque);

            conta.Depositar(valor);
        }

        public string VerSaldo()
        {
            return $"Saldo atual: R$ {Saldo}";
        }

        public virtual string VerExtrato()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"** Extrato - Lançamentos - {this.Cliente.Nome} **");

            foreach (var lancamento in Lancamentos)
            {
                sb.Append(lancamento.GetType().Name + " -->  ");
                sb.Append(lancamento.Data.ToString("dd/MM/yyyy hh:mm:ss" + "  -->  "));

                if (lancamento is Saque)
                    sb.Append(" - ");

                if (lancamento is Deposito)
                    sb.Append(" + ");

                sb.Append("R$ ");
                sb.AppendLine(lancamento.Valor.ToString());
            }

            sb.AppendLine($"Saldo final: R${Saldo}");

            return sb.ToString();
        }
    }
}

