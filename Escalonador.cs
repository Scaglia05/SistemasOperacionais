using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais
{
    public class Escalonador
    {
        public List<Processo> Pronto { get; set; }
        public List<Processo> Executando { get; set; }
        public List<Processo> Bloqueados { get; set; }

        public void AddProcesso() { }
        public void ProximoFifo() { }

        public void ProximoRoundRobin() { }
        public void ProximoSJF() { }
    }
}
