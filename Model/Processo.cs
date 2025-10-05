using SistemasOperacionais.Enum;
using System.Collections.Generic;

namespace SistemasOperacionais.Model
{
    public class Processo
    {
        public int Id { get; }
        public string Nome { get; }
        public Estado EstadoProcesso { get; private set; }
        public int[] Registradores { get; private set; } = new int[8];
        public int ContadorPrograma { get; set; }
        public int Prioridade { get; set; }
        public int TempoTotal { get; }
        public int TempoRestante { get; private set; }
        public int MemoriaNecessaria { get; }
        public List<string> ArquivosAbertos { get; private set; } = new();

        // Métricas
        public int TempoEntradaNaFila { get; set; } = -1;
        public int TempoRespostaPrimeiraCPU { get; set; } = -1;
        public int TempoSaidaFinal { get; set; } = -1;
        public int TempoExecutado { get; private set; }

        public Processo(int id, string nome, int tempoExec, int memoria, int prioridade = 1)
        {
            Id = id;
            Nome = nome;
            TempoTotal = tempoExec;
            TempoRestante = tempoExec;
            MemoriaNecessaria = memoria;
            Prioridade = prioridade;
            EstadoProcesso = Estado.Pronto;
        }

        public void ExecutarCiclo(int quantum = 1)
        {
            if (EstadoProcesso == Estado.Bloqueado || EstadoProcesso == Estado.Finalizado)
                return;
            EstadoProcesso = Estado.Executando;

            int dec = System.Math.Min(quantum, TempoRestante);
            TempoRestante -= dec;
            TempoExecutado += dec;
            ContadorPrograma += dec;

            if (TempoRestante <= 0)
            {
                EstadoProcesso = Estado.Finalizado;
                TempoRestante = 0;
            }
        }

        public void AlterarEstado(Estado novo) => EstadoProcesso = novo;
        public void BloquearParaIO() => EstadoProcesso = Estado.Bloqueado;
        public void Desbloquear() { if (EstadoProcesso == Estado.Bloqueado) EstadoProcesso = Estado.Pronto; }

        public void RegistrarEntradaFila(int ciclo) { if (TempoEntradaNaFila == -1) TempoEntradaNaFila = ciclo; }
        public void RegistrarPrimeiraResposta(int ciclo) { if (TempoRespostaPrimeiraCPU == -1) TempoRespostaPrimeiraCPU = ciclo - TempoEntradaNaFila; }
        public void RegistrarSaida(int ciclo) => TempoSaidaFinal = ciclo;
    }
}
