namespace SistemasOperacionais.Model
{
    public class Nucleo
    {
        public int Id { get; set; }
        public bool Disponivel { get; private set; } = true;
        public ThreadSo ThreadAtual { get; private set; }

        public void Executar(Processo p)
        {
            if (!Disponivel)
                return;

            // aloca thread para rodar o processo
            ThreadAtual = new ThreadSo(p);
            Disponivel = false;

            // simula execução de 1 ciclo (ou quantum)
            ThreadAtual.ExecutarCiclo();

            // se o processo terminou, libera núcleo
            if (p.TempoRestante <= 0)
                Liberar();
        }

        public void Liberar()
        {
            ThreadAtual = null;
            Disponivel = true;
        }
    }
}
