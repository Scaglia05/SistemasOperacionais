namespace SistemasOperacionais.Model
{
    public class Escalonador
    {
        public List<Processo> Pronto { get; set; } = new();
        public List<Processo> Executando { get; set; } = new();
        public List<Processo> Bloqueados { get; set; } = new();

        // Adiciona processo novo na fila de prontos
        public void AddProcesso(Processo p)
        {
            Pronto.Add(p);
        }

        // Estratégia Round Robin
        public Processo ProximoRoundRobin()
        {
            if (Pronto.Count == 0)
                return null;
            var p = Pronto.First();
            Pronto.RemoveAt(0);
            Pronto.Add(p); // volta para o fim da fila
            return p;
        }

        // Estratégia Shortest Job First
        public Processo ProximoSJF()
        {
            if (Pronto.Count == 0)
                return null;
            var p = Pronto.OrderBy(x => x.TempoRestante).First();
            Pronto.Remove(p);
            return p;
        }

        public Processo ProximoFifo()
        {
            if (Pronto.Count == 0)
                return null;
            var p = Pronto.First();
            Pronto.RemoveAt(0);
            Executando.Add(p);
            return p;
        }

    }
}
