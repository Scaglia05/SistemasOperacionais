using SistemasOperacionais.Model;
using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class Nucleo
    {
        public int Id { get; set; }
        public bool Disponivel => ThreadAtual == null || ThreadAtual.Terminou;
        public ThreadSo ThreadAtual { get; private set; }

        // Atribui um processo ao núcleo
        public void Executar(Processo p)
        {
            if (!Disponivel)
                return;

            ThreadAtual = new ThreadSo(p);
            ThreadAtual.ExecutarCiclo();

            if (ThreadAtual.Terminou)
                Liberar();
        }

        // Libera o núcleo quando o processo termina
        public void Liberar()
        {
            ThreadAtual = null;
        }

        // Continua execução do processo atual (caso já esteja rodando)
        public void ContinuarCiclo()
        {
            if (ThreadAtual != null && !ThreadAtual.Terminou)
                ThreadAtual.ExecutarCiclo();

            if (ThreadAtual != null && ThreadAtual.Terminou)
                Liberar();
        }
    }
}
