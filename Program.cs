using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SistemasOperacionais.Enum;
using SistemasOperacionais.Model;

class Program
{
    static void Main()
    {
        Console.Title = "Simulador de Sistema Operacional";
        string inputMenu;

        do
        {
            Console.Clear();
            WriteCenteredBlock(new[]
            {
                "==============================================",
                "             SIMULADOR DE SO                  ",
                "=============================================="
            }, ConsoleColor.Cyan);

            // Exemplo de processos
            var processosExemplo = new List<(int id, string nome, int tempo, int memoria)>
            {
                (1, "ProcA", 6, 10),
                (2, "ProcB", 4, 30),
                (3, "ProcC", 3, 75)
            };

            WriteCenteredBlock(new[] { "", "Processos disponíveis:" }, ConsoleColor.Yellow);
            foreach (var p in processosExemplo)
                WriteCenteredLine($"[{p.id}] {p.nome,-6} | Tempo: {p.tempo,-3} | Memória: {p.memoria,-3}", ConsoleColor.DarkCyan);
            WriteCenteredLine("");

            WriteCenteredBlock(new[]
            {
                "Escolha a política de escalonamento:",
                "1 - FIFO",
                "2 - Round Robin",
                "3 - Prioridade",
                "q - Sair"
            }, ConsoleColor.Yellow);

            inputMenu = ReadCentered("Opção: ");
            if (inputMenu == "q")
                break;

            string politicaEscolhida = inputMenu switch
            {
                "1" => "fifo",
                "2" => "rr",
                "3" => "prioridade",
                _ => "fifo"
            };

            string modo = ReadCentered("Deseja rodar automaticamente (a cada 5s)? (s/n): ");
            bool modoAutomatico = modo?.Trim().ToLower() == "s";

            // Cria o sistema operacional
            var so = new SoBase(
                totalMemoria: 100,
                tamPagina: 10,
                qtdNucleos: 2,
                algoritmoEscalonamento: politicaEscolhida,
                quantumConfig: 1);

            // Cria processos
            foreach (var p in processosExemplo)
            {
                var proc = new Processo(p.id, p.nome, p.tempo, p.memoria);
                so.CriarProcesso(proc);
            }

            // Cria dispositivo de exemplo
            so.CriarDispositivo("Disco1", 5);

            WriteCenteredLine($"Executando com política: {politicaEscolhida}", ConsoleColor.Green);
            WriteCenteredLine(modoAutomatico
                ? "Simulação automática: um ciclo a cada 5 segundos."
                : "Simulação manual: pressione Enter para avançar um ciclo.", ConsoleColor.Yellow);

            string input;
            do
            {
                WriteCenteredLine($"================== CICLO {so.CicloAtual} ==================", ConsoleColor.Magenta);

                so.ExecutarCiclo();
                MostrarStatus(so);
                WriteCenteredLine("");

                if (modoAutomatico)
                {
                    Thread.Sleep(5000);
                    input = "";
                } else
                {
                    input = ReadCentered("[Enter] próximo ciclo   |   [r] reiniciar   |   [q] sair: ");
                    if (input == "r")
                    {
                        so.Finalizar();
                        break;
                    }
                }

            } while (input != "q" && so.Processos.Any(p => p.EstadoProcesso != Estado.Finalizado));

            if (input != "q")
            {
                WriteCenteredLine(">>> Simulação finalizada. <<<", ConsoleColor.Red);
                so.Finalizar();
            }

        } while (inputMenu != "q");

        WriteCenteredLine("Encerrando simulador... Até a próxima!", ConsoleColor.DarkCyan);
    }

    // MostrarStatus
    static void MostrarStatus(SoBase so)
    {
        // Memória
        int livres = so.MemoriaSistema.NumPaginas - so.MemoriaSistema.TabelaPaginas.Count; // simplificação
        WriteCenteredLine($"Memória disponível: {livres}/{so.MemoriaSistema.NumPaginas}", ConsoleColor.Cyan);
        WriteCenteredLine("");

        // Fila de prontos
        WriteCenteredLine("--------- Fila de prontos ---------", ConsoleColor.Yellow);
        var fila = so.EscalonadorProcessos.Fila;
        if (!fila.Any())
            WriteCenteredLine("- Vazia");
        else
            foreach (var p in fila)
                WriteCenteredLine($"| {p.Nome,-6} | Restante: {p.TempoRestante,-3} | Executado: {p.TempoExecutado,-3} |");

        WriteCenteredLine("");
        WriteCenteredLine("------------- Núcleos --------------", ConsoleColor.Green);
        foreach (var n in so.Nucleos)
        {
            string line = n.Disponivel
                ? $"| Núcleo {n.Id,-2} | Livre"
                : $"| Núcleo {n.Id,-2} | {n.ThreadAtual.ProcessoPai.Nome,-6} | Restante: {n.ThreadAtual.ProcessoPai.TempoRestante,-3} | Executado: {n.ThreadAtual.ProcessoPai.TempoExecutado,-3} |";
            WriteCenteredLine(line);
        }

        WriteCenteredLine("");
        WriteCenteredLine("--------- Processos finalizados ---------", ConsoleColor.Red);
        var finalizados = so.Processos.Where(p => p.EstadoProcesso == Estado.Finalizado).ToList();
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
        int pad = (Console.WindowWidth - text.Length) / 2;
        Console.WriteLine(new string(' ', Math.Max(pad, 0)) + text);
        Console.ResetColor();
    }

    static void WriteCenteredBlock(string[] lines, ConsoleColor? color = null)
    {
        foreach (var line in lines)
            WriteCenteredLine(line, color);
    }

    static string ReadCentered(string prompt)
    {
        int pad = (Console.WindowWidth - prompt.Length) / 2;
        Console.Write(new string(' ', Math.Max(pad, 0)));
        return Console.ReadLine() ?? "";
    }
}
