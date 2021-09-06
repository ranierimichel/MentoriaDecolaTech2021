using AgenciaBancaria.Dominio;
using System;

namespace AgenciaBancaria.App
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string senha = "foo12345";
                Endereco endereco = new Endereco("Rua foo", "12345678", "Cidade foo", "MG");

                Cliente cliente = new Cliente("Ranieri", "12345678900", "987654321", endereco);
                ContaCorrente conta = new ContaCorrente(cliente, 100);
                Console.WriteLine($"ContaCorrente criada: {conta.NumeroConta} - Digito Verificador: {conta.DigitoVerificador} - Situação: {conta.Situacao} - Saldo: {conta.Saldo}");


                Cliente cliente1 = new Cliente("Lara", "12345678911", "123456789", endereco);
                ContaPoupança conta1 = new ContaPoupança(cliente1);
                Console.WriteLine($"ContaPoupança criada: {conta1.NumeroConta} - Digito Verificador: {conta1.DigitoVerificador} - Situação: {conta1.Situacao} - Saldo: {conta1.Saldo}");

                conta.Abrir(senha);
                Console.WriteLine($"ContaCorrente Situação: {conta.Situacao}");

                conta1.Abrir(senha);
                Console.WriteLine($"ContaPoupança Situação: {conta1.Situacao}" + Environment.NewLine);

                Random random = new Random();
                var valor1 = random.Next(0, 10);
                var valor2 = random.Next(10, 20);
                var valor3 = random.Next(20, 30);

                Console.WriteLine($"ContaPoupança Depositar R$ {valor3}");
                conta1.Depositar(valor3);
                Console.WriteLine(conta1.VerSaldo() + Environment.NewLine);

                Console.WriteLine($"ContaPoupança Sacar R$ {valor2}");
                conta1.Sacar(valor2, senha);
                Console.WriteLine(conta1.VerSaldo() + Environment.NewLine);

                Console.WriteLine($"ContaPoupança Transferir para {conta.NumeroConta} - R$ {valor1}");
                conta1.Transferir(valor1, conta, senha);
                Console.WriteLine(conta1.VerSaldo() + Environment.NewLine);

                Console.WriteLine($"ContaCorrente Sacar {valor1}");
                conta.Sacar(valor1, senha);
                Console.WriteLine(conta.VerSaldo() + Environment.NewLine);

                Console.WriteLine($"ContaCorrente Depositar {valor3}");
                conta.Depositar(valor3);
                Console.WriteLine(conta.VerSaldo() + Environment.NewLine);

                Console.WriteLine($"ContaCorrente Transferir para {conta1.NumeroConta} - R$ {valor2}");
                conta.Transferir(valor2, conta1, senha);
                Console.WriteLine(conta.VerSaldo() + Environment.NewLine);

                Console.WriteLine(conta1.VerExtrato());
                Console.WriteLine(conta.VerExtrato());

                conta1.Sacar(conta1.Saldo, senha);
                conta1.Encerrar(senha);

                conta.Sacar(conta.Saldo, senha);
                conta.Encerrar(senha);

                //try
                //{
                //    conta1.Depositar(100);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}

                try
                {
                    conta.Depositar(100);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
