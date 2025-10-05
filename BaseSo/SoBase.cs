using System;
using System.Collections.Generic;
using System.Linq;
using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class SoBase
    {
        private int ciclo = 0;
        private int quantum;
        private string algoritmo;
        private CPU cpu;
        private List<Nucleo> nucleos;
        private Escalonador escalonador;
        private Memoria memoria;
        private List<Dispositivo> dispositivos = new();
        private SistemaArquivos fs;

        // Métricas
        private int trocasContexto = 0;
        private int processosFinalizados = 0;
        private List<Processo> todosProcessos = new(); 
        public IReadOnlyList<Processo> Processos => todosProcessos.AsReadOnly();
        public IReadOnlyList<Nucleo> Nucleos => nucleos.AsReadOnly();
        public Memoria MemoriaSistema => memoria;
        public Escalonador EscalonadorProcessos => escalonador;
        public int CicloAtual => ciclo;


        public SoBase(
            int totalMemoria,
            int tamPagina,
            int qtdNucleos,
            string algoritmoEscalonamento,
            int quantumConfig = 1)
        {
            cpu = new CPU(totalMemoria);
            memoria = new Memoria(totalMemoria, tamPagina);
            nucleos = Enumerable.Range(0, qtdNucleos).Select(i => new Nucleo(i)).ToList();
            escalonador = new Escalonador();
            algoritmo = algoritmoEscalonamento.ToLower();
            quantum = quantumConfig;
            fs = new SistemaArquivos();
        }

        // Cria processo no sistema
        public void CriarProcesso(Processo p)
        {
            if (cpu.Alocar(p) && memoria.Alocar(p))
            {
                escalonador.Adicionar(p);
                todosProcessos.Add(p);
                Log($"Processo {p.Id} criado e adicionado à fila.");
            } else
            {
                Log($"Falha ao criar processo {p.Id} (memória insuficiente).");
            }
        }
        public void CriarDispositivo(string nome, int tempo)
        {
            var d = new Dispositivo(nome, tempo);
            RegistrarDispositivo(d);
        }

        // Adiciona dispositivo
        public void RegistrarDispositivo(Dispositivo d)
        {
            dispositivos.Add(d);
            d.Interrupcao += p =>
            {
                escalonador.Adicionar(p);
                Log($"Interrupção: processo {p.Id} voltou para pronto.");
            };
        }

        // Loop principal
        public void ExecutarCiclo()
        {
            ciclo++;
            Log($"=== CICLO {ciclo} ===");

            // Executa dispositivos de E/S
            foreach (var d in dispositivos)
                d.ExecutarCiclo();

            // Libera núcleos e escalona novos processos
            foreach (var nucleo in nucleos)
            {
                if (nucleo.Disponivel)
                {
                    var proximo = SelecionarProcesso();
                    if (proximo != null)
                    {
                        nucleo.Atribuir(proximo);
                        trocasContexto++;
                        Log($"Núcleo {nucleo.Id} atribuiu processo {proximo.Id}.");
                    }
                }
            }

            // Executa ciclos de cada núcleo
            foreach (var nucleo in nucleos)
            {
                nucleo.ExecutarCiclo(quantum);
                if (nucleo.ThreadAtual == null)
                    continue;

                var proc = nucleo.ThreadAtual.ProcessoPai;
                if (proc.EstadoProcesso == Estado.Finalizado)
                {
                    proc.RegistrarSaida(ciclo);
                    cpu.Liberar(proc);
                    memoria.Liberar(proc);
                    processosFinalizados++;
                    Log($"Processo {proc.Id} finalizado.");
                } else if (algoritmo == "rr") // round robin
                {
                    var preemptado = nucleo.Preemptar();
                    if (preemptado != null && preemptado.EstadoProcesso != Estado.Finalizado)
                    {
                        preemptado.AlterarEstado(Estado.Pronto);
                        escalonador.Adicionar(preemptado);
                        Log($"Processo {preemptado.Id} preemptado e devolvido à fila.");
                    }
                }
            }
        }

        // Seleciona processo do escalonador
        private Processo SelecionarProcesso()
        {
            return algoritmo switch
            {
                "fifo" => escalonador.ProximoFIFO(),
                "rr" => escalonador.ProximoRoundRobin(quantum),
                "prioridade" => escalonador.ProximoPrioridade(),
                _ => escalonador.ProximoFIFO()
            };
        }

        // Finaliza simulação
        public void Finalizar()
        {
            Log("=== SIMULAÇÃO ENCERRADA ===");
            Log($"Total ciclos: {ciclo}");
            Log($"Processos finalizados: {processosFinalizados}/{todosProcessos.Count}");
            Log($"Trocas de contexto: {trocasContexto}");
            Log($"Taxa de falta de página: {memoria.FaltaPagina}");
            Log($"Hits TLB: {memoria.HitsTLB} | Miss TLB: {memoria.MissTLB}");
        }

        // Log simples
        private void Log(string msg) => Console.WriteLine($"[Ciclo {ciclo}] {msg}");
    }
}
