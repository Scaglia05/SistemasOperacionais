using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais
{
    public class Nucleo
    {
        public int Id { get; set; }
        public bool Disponivel { get; set; }    
        public ThreadSo ThreadAtual { get; set; }

        public void Executar(Processo p)
        {
        }

        public void Liberar()
        {
        }   
    }
}
