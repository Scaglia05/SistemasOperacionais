using System;
using SistemasOperacionais;
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

        var so = new SoBase(maxNucleos: 2, memoriaTotal: 100)
        {
            Politica = politica
        };

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

            so.ExecutarCiclo(); // avançar um ciclo
            MostrarStatus(so);

            Console.WriteLine("\nEnter = próximo ciclo, q = sair");
            input = Console.ReadLine();

        } while (input != "q" && so.Processos.Exists(p => p.EstadoProcesso != Estado.Finalizado));

        Console.WriteLine("\nSimulação finalizada.");
        MostrarStatus(so);
    }

    static void MostrarStatus(SoBase so)
    {
        Console.WriteLine($"\nMemória disponível: {so.MemoriaExec.MemoriaDisponivel}/{so.MemoriaExec.TotalMemoria}");

        Console.WriteLine("\nFila de prontos:");
        if (so.EscalonadorProcessos.Pronto.Count == 0)
            Console.WriteLine("- Vazia");
        else
            foreach (var p in so.EscalonadorProcessos.Pronto)
                Console.WriteLine($"- {p.Nome} (Tempo restante: {p.TempoRestante}, Executado: {p.TempoExecutado})");

        Console.WriteLine("\nNúcleos:");
        foreach (var n in so.Nucleos)
        {
            string status = n.Disponivel
                ? "Livre"
                : $"Executando {n.ThreadAtual.ProcessoAlvo.Nome} (Restante: {n.ThreadAtual.ProcessoAlvo.TempoRestante}, Executado: {n.ThreadAtual.ProcessoAlvo.TempoExecutado})";
            Console.WriteLine($"Núcleo {n.Id}: {status}");
        }

        Console.WriteLine("\nProcessos finalizados:");
        var finalizados = so.Processos.FindAll(p => p.EstadoProcesso == Estado.Finalizado);
        if (finalizados.Count == 0)
            Console.WriteLine("- Nenhum");
        else
            foreach (var p in finalizados)
                Console.WriteLine($"- {p.Nome} | Tempo total executado: {p.TempoExecutado}");
    }
}
