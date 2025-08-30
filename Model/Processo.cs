
using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class Processo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Estado EstadoProcesso { get; set; }
        public int TamanhoExec { get; set; }     // tempo total necessário
        public int TempoRestante { get; set; }   // tempo que falta
        public int TamanhoMemoria { get; set; }  // memória necessária para o processo
        public int TempoExecutado { get; private set; } = 0;



        public Processo(int id, string nome, int tempoExec, int tamanhoMemoria)
        {
            Id = id;
            Nome = nome;
            TamanhoExec = tempoExec;
            TempoRestante = tempoExec;
            TamanhoMemoria = tamanhoMemoria;
            EstadoProcesso = Estado.Pronto;
        }

        public void Executar()
        {
            if (EstadoProcesso != Estado.Executando)
                return;

            if (TempoRestante > 0)
            {
                TempoRestante--;
                TempoExecutado++;
                if (TempoRestante == 0)
                    EstadoProcesso = Estado.Finalizado;
            }
        }
    }
}
