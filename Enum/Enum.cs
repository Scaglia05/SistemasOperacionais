using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais.Enum;

public enum Estado : sbyte
{
    [Description("Pronto")]
    Pronto,

    [Description("Executando")]
    Executando,

    [Description("Bloqueado")]
    Bloqueado,

    [Description("Finalizado")]
    Finalizado
}
public enum PoliticaEscalonamento : sbyte
{
    [Description("FIFO")]
    Fifo,
    [Description("Round Robin")]
    RoundRobin,
    [Description("SJF")]
    SJF
}



