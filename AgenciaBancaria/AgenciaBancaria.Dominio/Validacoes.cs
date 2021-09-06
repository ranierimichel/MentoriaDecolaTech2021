using System;

namespace AgenciaBancaria.Dominio
{
    public static class Validacoes
    {
        public static string ValidaStringVazia(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? throw new Exception("Propriedade deve estar preenchida!") : str;
        }
    }
}