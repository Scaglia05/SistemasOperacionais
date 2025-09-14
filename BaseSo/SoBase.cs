using SistemasOperacionais.Enum;
using SistemasOperacionais.Model;
using System.Diagnostics;

namespace SistemasOperacionais.BaseSo
{
    public class SoBase
    {
        public int MaxNucleos { get; }
        public List<Nucleo> Nucleos { get; private set; }
        public List<Processo> Processos { get; private set; }
        public CPU Cpu { get; private set; }
        public Escalonador EscalonadorProcessos { get; private set; }
        public PoliticaEscalonamento Politica { get; set; }
        public int CicloAtual { get; private set; } = 0;
        public int Quantum { get; set; } = 2;


        public SoBase(int memoriaCpu, int maxNucleos, PoliticaEscalonamento politica = PoliticaEscalonamento.Fifo)
        {
            MaxNucleos = maxNucleos;
            Politica = politica;

            Nucleos = new List<Nucleo>();
            Processos = new List<Processo>();
            Cpu = new CPU(memoriaCpu);
            EscalonadorProcessos = new Escalonador();

            AdicionarNucleos();
        }

        public void InicializarProcessos()
        {
            foreach (var p in Processos)
            {
                if (!EscalonadorProcessos.FilaProntos.Contains(p) && p.EstadoProcesso == Estado.Pronto)
                    EscalonadorProcessos.AddProcesso(p);
            }
        }

        private void AdicionarNucleos()
        {
            for (int i = 0; i < MaxNucleos; i++)
                Nucleos.Add(new Nucleo { Id = i + 1 });
        }

        public void CriarProcesso(int id, string nome, int tempoExec, int tamanhoMemoria)
        {
            var processo = new Processo(id, nome, tempoExec, tamanhoMemoria);

            // Apenas adiciona à lista e fila de prontos, sem alocar memória ainda
            Processos.Add(processo);
            EscalonadorProcessos.AddProcesso(processo);
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
                        processo.primeiroCiclo = true;
                        if (Cpu.Alocar(processo)) // aloca memória só agora
                        {
                            EscalonadorProcessos.RemoverProcesso(processo);
                            processo.AlterarEstado(Estado.Executando);
                            nucleo.Executar(processo);
                        } else
                        {
                            EscalonadorProcessos.AddProcesso(processo);
                        }
                    }
                } else
                {
                    nucleo.ThreadAtual?.ExecutarCiclo();

                    if (Politica.Equals(PoliticaEscalonamento.RoundRobin) && nucleo.ThreadAtual.ProcessoAlvo.TempoExecutado == Quantum)
                    {
                        nucleo.ThreadAtual.ProcessoAlvo.TempoExecutado = 0; 
                        EscalonadorProcessos.AddProcesso(nucleo.ThreadAtual.ProcessoAlvo);
                        Cpu.Liberar(nucleo.ThreadAtual.ProcessoAlvo); // <<< faltava isso
                        nucleo.Liberar();
                    }

                    if (nucleo.ThreadAtual?.ProcessoAlvo.EstadoProcesso == Estado.Finalizado)
                    {
                        Cpu.Liberar(nucleo.ThreadAtual.ProcessoAlvo); // <<< faltava isso
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

    }
}
