using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasOperacionais;
public class Enum
{
    public enum Estado
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
}

