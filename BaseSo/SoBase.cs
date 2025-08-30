using SistemasOperacionais.Enum;
using SistemasOperacionais.Model;

namespace SistemasOperacionais.BaseSo
{
    public class SoBase
    {
        public int MaxNucleos { get; set; }
        public List<Nucleo> Nucleos { get; set; } = new();
        public List<Processo> Processos { get; set; } = new();
        public Memoria MemoriaExec { get; set; }
        public Escalonador EscalonadorProcessos { get; set; } = new();
        public PoliticaEscalonamento Politica { get; set; } = PoliticaEscalonamento.Fifo;
        public int CicloAtual { get; private set; } = 0;



        public SoBase(int maxNucleos, int memoriaTotal)
        {
            MaxNucleos = maxNucleos;
            MemoriaExec = new Memoria(memoriaTotal);
            AddNucleos();
        }

        public void ExecutarCiclo()
        {
            CicloAtual++;

            foreach (var nucleo in Nucleos)
            {
                if (nucleo.Disponivel)
                {
                    Processo proc = null;
                    switch (Politica)
                    {
                        case PoliticaEscalonamento.Fifo:
                            proc = EscalonadorProcessos.ProximoFifo();
                            break;
                        case PoliticaEscalonamento.RoundRobin:
                            proc = EscalonadorProcessos.ProximoRoundRobin();
                            break;
                        case PoliticaEscalonamento.SJF:
                            proc = EscalonadorProcessos.ProximoSJF();
                            break;
                    }

                    if (proc != null && proc.EstadoProcesso != Estado.Finalizado)
                    {
                        proc.EstadoProcesso = Estado.Executando;
                        nucleo.Executar(proc); // roda 1 ciclo
                    }
                } else
                {
                    // núcleos já ocupados continuam executando ciclo
                    nucleo.ThreadAtual?.ExecutarCiclo();
                    if (nucleo.ThreadAtual?.ProcessoAlvo.EstadoProcesso == Estado.Finalizado)
                        nucleo.Liberar();
                }
            }
        }

        public void CriarProcesso(int id, string nome, int tempoExec, int tamanhoMemoria)
        {
            var p = new Processo(id, nome, tempoExec, tamanhoMemoria);

            if (MemoriaExec.Alocar(p))
            {
                Processos.Add(p);
                EscalonadorProcessos.AddProcesso(p);
            } else
            {
                Console.WriteLine($"[SO] Memória insuficiente para criar processo {nome}.");
            }
        }

        public void Executar()
        {
            foreach (var nucleo in Nucleos)
            {
                if (nucleo.Disponivel)
                {
                    Processo proc = null;
                    switch (Politica)
                    {
                        case PoliticaEscalonamento.Fifo:
                            proc = EscalonadorProcessos.ProximoFifo();
                            break;
                        case PoliticaEscalonamento.RoundRobin:
                            proc = EscalonadorProcessos.ProximoRoundRobin();
                            break;
                        case PoliticaEscalonamento.SJF:
                            proc = EscalonadorProcessos.ProximoSJF();
                            break;
                    }

                    if (proc != null && proc.EstadoProcesso != Estado.Finalizado)
                    {
                        proc.EstadoProcesso = Estado.Executando;
                        nucleo.Executar(proc);
                    }
                }
            }
        }

        public void AddNucleos()
        {
            for (int i = 0; i < MaxNucleos; i++)
            {
                Nucleos.Add(new Nucleo { Id = i + 1 });
            }
        }

        public void MostrarStatus()
        {
            Console.WriteLine($"\nCiclo: {CicloAtual}");
            Console.WriteLine($"Memória disponível: {MemoriaExec.MemoriaDisponivel}/{MemoriaExec.TotalMemoria}");

            Console.WriteLine("Fila de Prontos:");
            foreach (var p in EscalonadorProcessos.Pronto)
                Console.WriteLine($"- {p.Nome} (TempoRestante: {p.TempoRestante})");

            Console.WriteLine("Núcleos:");
            foreach (var n in Nucleos)
            {
                if (n.ThreadAtual != null)
                    Console.WriteLine($"- Núcleo {n.Id}: {n.ThreadAtual.ProcessoAlvo.Nome} (Restante: {n.ThreadAtual.ProcessoAlvo.TempoRestante})");
                else
                    Console.WriteLine($"- Núcleo {n.Id}: Livre");
            }

            Console.WriteLine("Processos Finalizados:");
            foreach (var p in Processos.Where(p => p.EstadoProcesso == Estado.Finalizado))
                Console.WriteLine($"- {p.Nome} | Tempo Executado: {p.TempoExecutado}");
        }
    }
}
