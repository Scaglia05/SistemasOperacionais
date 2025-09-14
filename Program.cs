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
            WriteCenteredBlock(new[]
            {
                "==============================================",
                "             SIMULADOR DE SO                  ",
                "=============================================="
            }, ConsoleColor.Cyan);

            var processosExemplo = new List<(int id, string nome, int tempo, int memoria)>
            {
                (1, "ProcA", 5, 10),
                (2, "ProcB", 3, 30),
                (3, "ProcC", 7, 15)
            };


            // Título do bloco
            WriteCenteredBlock(new[]
            {
                "",
                "Processos disponíveis:"
            }, ConsoleColor.Yellow);

            // Lista de processos em outra cor
            foreach (var p in processosExemplo)
            {
                WriteCenteredLine($"[{p.id}] {p.nome,-6} | Tempo: {p.tempo,-3} | Memória: {p.memoria,-3}", ConsoleColor.DarkCyan);
            }


            WriteCenteredLine(""); // espaçamento

            WriteCenteredBlock(new[]
            {
                "Escolha a política de escalonamento:"
            }, ConsoleColor.White);

            WriteCenteredBlock(new[]
            {
                "1 - FIFO",
                "2 - Round Robin",
                "3 - SJF",
                "q - Sair"
            }, ConsoleColor.Yellow);

            WriteCenteredLine(""); // espaçamento
            inputMenu = ReadCentered("Opção: ");

            if (inputMenu == "q")
                break;

            PoliticaEscalonamento politica = inputMenu switch
            {
                "1" => PoliticaEscalonamento.Fifo,
                "2" => PoliticaEscalonamento.RoundRobin,
                "3" => PoliticaEscalonamento.SJF,
                _ => PoliticaEscalonamento.Fifo
            };

            var so = new SoBase(memoriaCpu: 100, maxNucleos: 2, politica: politica);
            foreach (var p in processosExemplo)
                so.CriarProcesso(p.id, p.nome, p.tempo, p.memoria);

            so.InicializarProcessos();

            WriteCenteredLine(""); // espaçamento
            WriteCenteredLine($"Executando com política: {politica}", ConsoleColor.Green);
            WriteCenteredLine("Pressione Enter para avançar um ciclo, 'r' para reiniciar, ou 'q' para sair.");
            WriteCenteredLine(""); // espaçamento

            string input;
            do
            {
                WriteCenteredLine($"================== CICLO {so.CicloAtual + 1} ==================", ConsoleColor.Magenta);
                so.ExecutarCiclo();
                MostrarStatus(so);
                WriteCenteredLine(""); // espaçamento

                input = ReadCentered("[Enter] próximo ciclo   |   [r] reiniciar   |   [q] sair: ");
                if (input == "r")
                    break;

            } while (input != "q" && so.ProcessosPendentes());

            if (input != "r" && input != "q")
            {
                WriteCenteredLine(">>> Simulação finalizada. <<<", ConsoleColor.Red);
                MostrarStatus(so);
                WriteCenteredLine(""); // espaçamento
            }

        } while (inputMenu != "q");

        WriteCenteredLine("Encerrando simulador... Até a próxima!", ConsoleColor.DarkCyan);
    }

    static void MostrarStatus(SoBase so)
    {
        WriteCenteredLine($"Memória disponível: {so.Cpu.MemoriaDisponivel}/{so.Cpu.TotalMemoria}", ConsoleColor.Cyan);
        WriteCenteredLine(""); // espaçamento

        WriteCenteredLine("--------- Fila de prontos ---------", ConsoleColor.Yellow);
        var fila = so.EscalonadorProcessos.FilaProntos;
        if (!fila.Any())
            WriteCenteredLine("- Vazia");
        else
            foreach (var p in fila)
                WriteCenteredLine($"| {p.Nome,-6} | Restante: {p.TempoRestante,-3} | Executado: {p.TempoExecutado,-3} |");

        WriteCenteredLine(""); // espaçamento
        WriteCenteredLine("------------- Núcleos --------------", ConsoleColor.Green);
        foreach (var n in so.Nucleos)
        {
            string line = n.Disponivel
                ? $"| Núcleo {n.Id,-2} | Livre"
                : $"| Núcleo {n.Id,-2} | {n.ThreadAtual.ProcessoAlvo.Nome,-6} | Restante: {n.ThreadAtual.ProcessoAlvo.TempoRestante,-3} | Executado: {n.ThreadAtual.ProcessoAlvo.TempoExecutado,-3} |";
            WriteCenteredLine(line);
        }

        WriteCenteredLine(""); // espaçamento
        WriteCenteredLine("--------- Processos finalizados ---------", ConsoleColor.Red);
        var finalizados = so.Processos.FindAll(p => p.EstadoProcesso == Estado.Finalizado);
        if (!finalizados.Any())
            WriteCenteredLine("- Nenhum");
        else
            foreach (var p in finalizados)
                WriteCenteredLine($"| {p.Nome,-6} | Tempo total executado: {p.TempoExecutado,-3} |");
    }

    static void WriteCenteredLine(string text, ConsoleColor? color = null)
    {
        if (color.HasValue)
            Console.ForegroundColor = color.Value;
        int width = Console.WindowWidth;
        int pad = (width - text.Length) / 2;
        if (pad < 0)
            pad = 0;
        Console.WriteLine(new string(' ', pad) + text);
        Console.ResetColor();
    }

    static void WriteCenteredBlock(string[] lines, ConsoleColor? color = null)
    {
        foreach (var line in lines)
            WriteCenteredLine(line, color);
    }

    static string ReadCentered(string prompt)
    {
        int width = Console.WindowWidth;
        int pad = (width - prompt.Length) / 2;
        if (pad < 0)
            pad = 0;
        Console.Write(new string(' ', pad));
        return Console.ReadLine();
    }
}
