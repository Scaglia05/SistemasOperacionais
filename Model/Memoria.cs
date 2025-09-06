namespace SistemasOperacionais.Model
{
    public class Memoria
    {
        public int TotalMemoria { get; private set; }
        public int MemoriaDisponivel { get; private set; }

        public Memoria(int total)
        {
            TotalMemoria = total;
            MemoriaDisponivel = total;
        }

        public bool Alocar(Processo processo)
        {
            if (processo.MemoriaNecessaria <= MemoriaDisponivel)
            {
                MemoriaDisponivel -= processo.MemoriaNecessaria;
                return true;
            }
            return false;
        }

        public void Liberar(Processo processo)
        {
            MemoriaDisponivel += processo.MemoriaNecessaria;
            if (MemoriaDisponivel > TotalMemoria)
                MemoriaDisponivel = TotalMemoria; // segurança
        }
    }
}
