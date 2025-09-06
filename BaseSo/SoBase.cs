using SistemasOperacionais.Enum;
using SistemasOperacionais.Model;

namespace SistemasOperacionais.BaseSo
{
    public class SoBase
    {
        public int MaxNucleos { get; }
        public List<Nucleo> Nucleos { get; private set; }
        public List<Processo> Processos { get; private set; }
        public Memoria MemoriaExec { get; private set; }
        public Escalonador EscalonadorProcessos { get; private set; }
        public PoliticaEscalonamento Politica { get; set; }
        public int CicloAtual { get; private set; } = 0;

        public SoBase(int maxNucleos, int memoriaTotal, PoliticaEscalonamento politica = PoliticaEscalonamento.Fifo)
        {
            MaxNucleos = maxNucleos;
            Politica = politica;

            Nucleos = new List<Nucleo>();
            Processos = new List<Processo>();
            MemoriaExec = new Memoria(memoriaTotal);
            EscalonadorProcessos = new Escalonador();

            AdicionarNucleos();
        }

        private void AdicionarNucleos()
        {
            for (int i = 0; i < MaxNucleos; i++)
                Nucleos.Add(new Nucleo { Id = i + 1 });
        }

        public void CriarProcesso(int id, string nome, int tempoExec, int tamanhoMemoria)
        {
            var processo = new Processo(id, nome, tempoExec, tamanhoMemoria);

            if (MemoriaExec.Alocar(processo))
            {
                Processos.Add(processo);
                EscalonadorProcessos.AddProcesso(processo);
            } else
            {
                Console.WriteLine($"[SO] Memória insuficiente para criar processo {nome}.");
            }
        }

        // Avança 1 ciclo de execução do SO
        public void ExecutarCiclo()
        {
            CicloAtual++;

            foreach (var nucleo in Nucleos)
            {
                if (nucleo.Disponivel)
                {
                    var processo = SelecionarProcesso();
                    if (processo != null)
                    {
                        processo.AlterarEstado(Estado.Executando);
                        nucleo.Executar(processo);
                    }
                } else
                {
                    nucleo.ThreadAtual?.ExecutarCiclo();

                    if (nucleo.ThreadAtual?.ProcessoAlvo.EstadoProcesso == Estado.Finalizado)
                    {
                        MemoriaExec.Liberar(nucleo.ThreadAtual.ProcessoAlvo); // <<< faltava isso
                        nucleo.Liberar();
                    }
                }
            }
        }

        private Processo SelecionarProcesso()
        {
            return Politica switch
            {
                PoliticaEscalonamento.Fifo => EscalonadorProcessos.ProximoFifo(),
                PoliticaEscalonamento.RoundRobin => EscalonadorProcessos.ProximoRoundRobin(),
                PoliticaEscalonamento.SJF => EscalonadorProcessos.ProximoSJF(),
                _ => null
            };
        }

        // Verifica se ainda existem processos não finalizados
        public bool ProcessosPendentes()
        {
            return Processos.Exists(p => p.EstadoProcesso != Estado.Finalizado);
        }

        public void MostrarStatus()
        {
            Console.WriteLine($"\nMemória disponível: {MemoriaExec.MemoriaDisponivel}/{MemoriaExec.TotalMemoria}");

            Console.WriteLine("\nFila de prontos:");
            var fila = EscalonadorProcessos.FilaProntos;
            if (fila.Count == 0)
                Console.WriteLine("- Vazia");
            else
                foreach (var p in fila)
                    Console.WriteLine($"- {p.Nome} | Tempo restante: {p.TempoRestante} | Executado: {p.TempoExecutado}");

            Console.WriteLine("\nNúcleos:");
            foreach (var n in Nucleos)
            {
                if (n.Disponivel)
                    Console.WriteLine($"Núcleo {n.Id}: Livre");
                else
                    Console.WriteLine($"Núcleo {n.Id}: Executando {n.ThreadAtual.ProcessoAlvo.Nome} " +
                                      $"(Restante: {n.ThreadAtual.ProcessoAlvo.TempoRestante}, Executado: {n.ThreadAtual.ProcessoAlvo.TempoExecutado})");
            }

            Console.WriteLine("\nProcessos finalizados:");
            var finalizados = Processos.FindAll(p => p.EstadoProcesso == Estado.Finalizado);
            if (finalizados.Count == 0)
                Console.WriteLine("- Nenhum");
            else
                foreach (var p in finalizados)
                    Console.WriteLine($"- {p.Nome} | Tempo total executado: {p.TempoExecutado}");
        }

    }
}
