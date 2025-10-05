using System;
using System.Collections.Generic;

namespace SistemasOperacionais.Model
{
    public class Memoria
    {
        public int TamanhoTotal { get; }
        public int TamanhoPagina { get; }
        public int NumPaginas => TamanhoTotal / TamanhoPagina;
        private bool[] Molduras;
        public Dictionary<int, List<int>> TabelaPaginas { get; private set; } = new();
        public int FaltaPagina { get; private set; }
        public int HitsTLB { get; private set; }
        public int MissTLB { get; private set; }
        private Dictionary<int, int> TLB = new();

        public Memoria(int total, int pagina)
        {
            TamanhoTotal = total;
            TamanhoPagina = pagina;
            Molduras = new bool[NumPaginas];
        }

        public bool Alocar(Processo p)
        {
            int paginas = (int)Math.Ceiling((double)p.MemoriaNecessaria / TamanhoPagina);
            int consecutivas = 0, inicio = -1;

            for (int i = 0; i < NumPaginas; i++)
            {
                if (!Molduras[i])
                {
                    if (consecutivas == 0)
                        inicio = i;
                    consecutivas++;
                    if (consecutivas >= paginas)
                    {
                        var list = new List<int>();
                        for (int j = inicio; j < inicio + paginas; j++)
                        { Molduras[j] = true; list.Add(j); }
                        TabelaPaginas[p.Id] = list;
                        return true;
                    }
                } else
                    consecutivas = 0;
            }
            FaltaPagina++;
            return false;
        }

        public void Liberar(Processo p)
        {
            if (!TabelaPaginas.ContainsKey(p.Id))
                return;
            foreach (var idx in TabelaPaginas[p.Id])
                Molduras[idx] = false;
            TabelaPaginas.Remove(p.Id);
            TLB.Remove(p.Id);
        }

        public bool AcessarPagina(Processo p, int endereco)
        {
            int pagina = endereco / TamanhoPagina;
            if (TLB.TryGetValue(p.Id, out var ultima) && ultima == pagina)
            {
                HitsTLB++;
                return true;
            }
            if (TabelaPaginas.TryGetValue(p.Id, out var lista) && lista.Contains(pagina))
            {
                MissTLB++;
                TLB[p.Id] = pagina;
                return true;
            }
            FaltaPagina++;
            MissTLB++;
            return false;
        }
    }
}
