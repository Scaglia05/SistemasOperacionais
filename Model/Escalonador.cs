using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Model
{
    public class Escalonador
    {
        private List<Processo> filaProntos = new();

        public void AddProcesso(Processo processo)
        {
            filaProntos.Add(processo);
        }
        public void RemoverProcesso(Processo p)
        {
            if (FilaProntos.Contains(p))
                filaProntos.Remove(p);
        }
        public Processo ProximoFifo()
        {
            if (!filaProntos.Any())
                return null;
            var processo = filaProntos.First();
            filaProntos.RemoveAt(0);
            return processo;
        }

        public Processo ProximoRoundRobin()
        {
            if (!filaProntos.Any())
                return null;
            var processo = filaProntos.First();
            filaProntos.RemoveAt(0);
            filaProntos.Add(processo); // volta para o fim da fila
            return processo;
        }

        public Processo ProximoSJF()
        {
            if (!filaProntos.Any())
                return null;
            var processo = filaProntos.OrderBy(p => p.TempoRestante).First();
            filaProntos.Remove(processo);
            return processo;
        }


        public IReadOnlyList<Processo> FilaProntos => filaProntos.AsReadOnly();
    }
}
