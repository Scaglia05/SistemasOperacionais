using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais
{
    public class Memoria
    {
        public int TotalMemoria { get; set; }
        public int MemoriaDisponivel { get; set; }

        public void Alocar(Processo p) { 
        }
        public void Liberar(Processo p)
        {
        }
    }
}
