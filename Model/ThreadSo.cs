using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class ThreadSo
    {
        public Processo ProcessoAlvo { get; private set; }

        public ThreadSo(Processo processo)
        {
            ProcessoAlvo = processo;
        }

        // Executa um ciclo do processo
        public void ExecutarCiclo()
        {
            if (ProcessoAlvo.EstadoProcesso == Estado.Pronto)
                ProcessoAlvo.AlterarEstado(Estado.Executando);

            // Executa 1 ciclo de CPU
            ProcessoAlvo.ExecutarCiclo();
        }

        // Retorna se o processo terminou
        public bool Terminou => ProcessoAlvo.EstadoProcesso == Estado.Finalizado;
    }
}
