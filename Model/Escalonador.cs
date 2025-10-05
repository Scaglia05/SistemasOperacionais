using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Model
{
    public class Escalonador
    {
        private List<Processo> fila = new();

        public void Adicionar(Processo p)
        {
            if (!fila.Contains(p))
                fila.Add(p);
        }

        public void Remover(Processo p) => fila.Remove(p);

        public Processo ProximoFIFO()
        {
            if (!fila.Any())
                return null;
            var p = fila.First();
            fila.RemoveAt(0);
            return p;
        }

        public Processo ProximoRoundRobin(int quantum)
        {
            if (!fila.Any())
                return null;
            var p = fila.First();
            fila.RemoveAt(0);
            fila.Add(p);
            return p;
        }

        public Processo ProximoPrioridade(bool preemptivo = false)
        {
            if (!fila.Any())
                return null;
            var p = fila.OrderByDescending(x => x.Prioridade).First();
            fila.Remove(p);
            return p;
        }

        public IReadOnlyList<Processo> Fila => fila.AsReadOnly();
    }
}
