using System;

namespace AgenciaBancaria.Dominio
{
    public class Deposito : Lancamento
    {
        public Deposito(decimal valor, DateTime data, ContaBancaria conta) : base(valor, data, conta) { }
    }
}