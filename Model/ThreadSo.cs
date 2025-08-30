
using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class ThreadSo
    {
        public int ProcessoId { get; private set; }
        public Processo ProcessoAlvo { get; private set; }

        public ThreadSo(Processo p)
        {
            ProcessoAlvo = p;
            ProcessoId = p.Id;
        }

        // Marca início da execução
        public void Start()
        {
            ProcessoAlvo.EstadoProcesso = Estado.Executando;
        }

        // Simula pausa (ex: fim de quantum ou preempção)
        public void Pausar()
        {
            if (ProcessoAlvo.EstadoProcesso == Estado.Executando)
                ProcessoAlvo.EstadoProcesso = Estado.Pronto;
        }

        // Finaliza execução
        public void Finalizar()
        {
            ProcessoAlvo.EstadoProcesso = Estado.Finalizado;
        }

        // Executa um ciclo de CPU
        public void ExecutarCiclo()
        {
            if (ProcessoAlvo.EstadoProcesso != Estado.Executando)
                Start();

            ProcessoAlvo.Executar();

            if (ProcessoAlvo.TempoRestante <= 0)
                Finalizar();
        }
    }
}
