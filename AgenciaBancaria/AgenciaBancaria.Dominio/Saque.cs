using System;

namespace AgenciaBancaria.Dominio
{
    public class Saque : Lancamento
    {
        public Saque(decimal valor, DateTime data, ContaBancaria conta) : base(valor, data, conta)
        {
        }
    }
}