namespace SistemasOperacionais.Model
{
    public class Memoria
    {
        public int TotalMemoria { get; set; }
        public int MemoriaDisponivel { get; set; }

        public Memoria(int total)
        {
            TotalMemoria = total;
            MemoriaDisponivel = total;
        }

        // Tenta alocar a memória para um processo
        public bool Alocar(Processo p)
        {
            if (p.TamanhoMemoria <= MemoriaDisponivel)
            {
                MemoriaDisponivel -= p.TamanhoMemoria;
                return true;
            }
            return false;
        }

        public void Liberar(Processo p)
        {
            MemoriaDisponivel += p.TamanhoMemoria;
            if (MemoriaDisponivel > TotalMemoria)
                MemoriaDisponivel = TotalMemoria;
        }

    }
}
