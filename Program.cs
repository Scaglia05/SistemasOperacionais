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

            // 1. Criar processos de exemplo (antes de escolher política)
            var processosExemplo = new List<(int id, string nome, int tempo, int memoria)>
{
    (1, "ProcA", 5, 10),
    (2, "ProcB", 3, 30),
    (3, "ProcC", 7, 15)
};

            // 2. Mostrar processos para o usuário
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Processos disponíveis:\n");
            foreach (var p in processosExemplo)
                Console.WriteLine($"[{p.id}] {p.nome} | Tempo total: {p.tempo} | Memória: {p.memoria}");
            Console.ResetColor();

            // 3. Perguntar política de escalonamento
            Console.WriteLine("\nEscolha a política de escalonamento:");
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

            // 4. Criar SO depois da escolha da política
            var so = new SoBase(memoriaCpu: 100, maxNucleos: 2, politica: politica);

            // 5. Criar processos dentro do SO
            foreach (var p in processosExemplo)
                so.CriarProcesso(p.id, p.nome, p.tempo, p.memoria);

            so.InicializarProcessos();
            Console.ResetColor();

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
                MostrarStatus(so);

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
                MostrarStatus(so);
            }

        } while (inputMenu != "q");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\nEncerrando simulador... Até a próxima!");
        Console.ResetColor();
    }

    static void MostrarStatus(SoBase so)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nMemória disponível: {so.Cpu.MemoriaDisponivel}/{so.Cpu.TotalMemoria}");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nFila de prontos:");
        Console.ResetColor();
        var fila = so.EscalonadorProcessos.FilaProntos;
        if (!fila.Any())
            Console.WriteLine("- Vazia");
        else
            foreach (var p in fila)
                Console.WriteLine($"- {p.Nome} | Tempo restante: {p.TempoRestante} | Executado: {p.TempoExecutado}");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nNúcleos:");
        Console.ResetColor();
        foreach (var n in so.Nucleos)
        {
            if (n.Disponivel)
                Console.WriteLine($"Núcleo {n.Id}: Livre");
            else
                Console.WriteLine($"Núcleo {n.Id}: Executando {n.ThreadAtual.ProcessoAlvo.Nome} " +
                                  $"(Restante: {n.ThreadAtual.ProcessoAlvo.TempoRestante}, Executado: {n.ThreadAtual.ProcessoAlvo.TempoExecutado})");
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nProcessos finalizados:");
        Console.ResetColor();
        var finalizados = so.Processos.FindAll(p => p.EstadoProcesso == Estado.Finalizado);
        if (!finalizados.Any())
            Console.WriteLine("- Nenhum");
        else
            foreach (var p in finalizados)
                Console.WriteLine($"- {p.Nome} | Tempo total executado: {p.TempoExecutado}");
    }
}
