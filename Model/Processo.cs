using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class Processo
    {
        public int Id { get; }
        public string Nome { get; }
        public Estado EstadoProcesso { get; private set; }
        public int TempoTotal { get; }
        public int TempoRestante { get; private set; }
        public int MemoriaNecessaria { get; }
        public int TempoExecutado { get; set; }
        public bool primeiroCiclo { get; set; } = true;
        public Processo(int id, string nome, int tempoExec, int memoriaNecessaria)
        {
            Id = id;
            Nome = nome;
            TempoTotal = tempoExec;
            TempoRestante = tempoExec;
            MemoriaNecessaria = memoriaNecessaria;
            EstadoProcesso = Estado.Pronto;
            TempoExecutado = 0;
        }

        // Executa 1 ciclo de CPU
        public void ExecutarCiclo()
        {
            if (primeiroCiclo)
            {
                if (EstadoProcesso == Estado.Pronto)
                    EstadoProcesso = Estado.Executando;

                primeiroCiclo = false;
                return; // não decrementa tempo ainda
            }

            // Executa de fato
            if (EstadoProcesso == Estado.Executando)
            {
                TempoRestante--;
                TempoExecutado++;

                if (TempoRestante <= 0)
                {
                    TempoRestante = 0;
                    EstadoProcesso = Estado.Finalizado;
                }
            }
        }
        public void AlterarEstado(Estado novoEstado)
        {
            EstadoProcesso = novoEstado;
        }

    }
}
