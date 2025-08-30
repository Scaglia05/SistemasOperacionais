using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais;

    public class SoBase
    {
    public int MaxNucleos { get; set; }
    public List<Nucleo> Mucleos { get; set; }
    public List<Processo> Processos { get; set; }
    public Memoria MemoriaExec { get; set; }
    public Escalonador EscalonadorProcessos { get; set; }

    public void CriarProcesso() { 

    }

    public void Executar() { 
    }

    public void AddNucleos() { 
    }
}

