using System;
using SistemasOperacionais.BaseSo;
using SistemasOperacionais.Enum;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Simulador de SO ===");
        Console.WriteLine("Escolha a política de escalonamento:");
        Console.WriteLine("1 - FIFO");
        Console.WriteLine("2 - Round Robin");
        Console.WriteLine("3 - SJF");
        Console.Write("Opção: ");
        int opc = int.Parse(Console.ReadLine());

        PoliticaEscalonamento politica = opc switch
        {
            1 => PoliticaEscalonamento.Fifo,
            2 => PoliticaEscalonamento.RoundRobin,
            3 => PoliticaEscalonamento.SJF,
            _ => PoliticaEscalonamento.Fifo
        };

        var so = new SoBase(maxNucleos: 2, memoriaTotal: 100, politica: politica);

        // Criando processos de exemplo
        so.CriarProcesso(1, "ProcA", 5, 10);
        so.CriarProcesso(2, "ProcB", 3, 20);
        so.CriarProcesso(3, "ProcC", 7, 15);

        Console.WriteLine($"\nExecutando com política: {politica}");
        Console.WriteLine("Pressione Enter para avançar um ciclo, ou 'q' para sair.");

        string input;
        do
        {
            Console.Clear();
            Console.WriteLine($"=== Ciclo {so.CicloAtual + 1} ===");

            so.ExecutarCiclo();  
            so.MostrarStatus(); 

            Console.WriteLine("\nEnter = próximo ciclo, q = sair");
            input = Console.ReadLine();

        } while (input != "q" && so.ProcessosPendentes());

        Console.WriteLine("\nSimulação finalizada.");
        so.MostrarStatus();
    }
}
