using SistemasOperacionais.Enum;
using System.Collections.Generic;

namespace SistemasOperacionais.Model
{
    public class ThreadSo
    {
        private static int NextId = 1;
        public int Id { get; }
        public Processo ProcessoPai { get; private set; }
        public Stack<int> Pilha { get; private set; } = new();

        public Estado EstadoThread => ProcessoPai.EstadoProcesso;
        public bool Terminou => ProcessoPai.EstadoProcesso == Estado.Finalizado;

        public ThreadSo(Processo processo)
        {
            Id = NextId++;
            ProcessoPai = processo;
        }

        public void ExecutarCiclo(int quantum)
        {
            if (ProcessoPai.EstadoProcesso == Estado.Bloqueado)
                return;
            if (ProcessoPai.TempoRespostaPrimeiraCPU == -1)
                ProcessoPai.RegistrarPrimeiraResposta(quantum);
            ProcessoPai.ExecutarCiclo(quantum);
        }

        public void Preemptar() { } // Apenas simulação
        public void ResetPilha() => Pilha.Clear();
    }
}
