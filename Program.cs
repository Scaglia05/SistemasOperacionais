using System;
using SistemasOperacionais.BaseSo;
using SistemasOperacionais.Enum;

class Program
{
    static void Main()
    {
        string inputMenu;
        Console.Title = "Simulador de Sistema Operacional";

        do
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("======================================");
            Console.WriteLine("        SIMULADOR DE SO");
            Console.WriteLine("======================================");
            Console.ResetColor();

            Console.WriteLine("Escolha a política de escalonamento:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("1 - FIFO");
            Console.WriteLine("2 - Round Robin");
            Console.WriteLine("3 - SJF");
            Console.ResetColor();
            Console.WriteLine("q - Sair");
            Console.Write("Opção: ");
            inputMenu = Console.ReadLine();

            if (inputMenu == "q")
                break;

            PoliticaEscalonamento politica = inputMenu switch
            {
                "1" => PoliticaEscalonamento.Fifo,
                "2" => PoliticaEscalonamento.RoundRobin,
                "3" => PoliticaEscalonamento.SJF,
                _ => PoliticaEscalonamento.Fifo
            };

            var so = new SoBase(maxNucleos: 2, memoriaTotal: 100, politica: politica);

            // Criando processos de exemplo
            so.CriarProcesso(1, "ProcA", 5, 10);
            so.CriarProcesso(2, "ProcB", 3, 20);
            so.CriarProcesso(3, "ProcC", 7, 15);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nExecutando com política: {politica}");
            Console.ResetColor();
            Console.WriteLine("Pressione Enter para avançar um ciclo, 'r' para reiniciar, ou 'q' para sair.");

            string input;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n=== CICLO {so.CicloAtual + 1} ===");
                Console.ResetColor();

                so.ExecutarCiclo();
                so.MostrarStatus();

                Console.WriteLine("\n[Enter] próximo ciclo   |   [r] reiniciar   |   [q] sair");
                input = Console.ReadLine();

                if (input == "r")
                    break; // reinicia (volta para o menu principal)

            } while (input != "q" && so.ProcessosPendentes());

            if (input != "r" && input != "q")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n>>> Simulação finalizada. <<<");
                Console.ResetColor();
                so.MostrarStatus();
            }

        } while (inputMenu != "q");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nEncerrando simulador... Até a próxima!");
        Console.ResetColor();
    }
}
