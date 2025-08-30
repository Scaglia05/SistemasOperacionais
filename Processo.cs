using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SistemasOperacionais.Enum;

namespace SistemasOperacionais
{
    public class Processo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Estado EstadoProcesso { get; set; }
        public int TamanhoExec { get; set; }
        public int TempoRestante { get; set; }

        public void Executar() { }
    }
}
