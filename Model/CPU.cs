using SistemasOperacionais.Model;

public class CPU
{
    public int TotalMemoria { get; }
    public int MemoriaDisponivel { get; private set; }

    public CPU(int total)
    {
        TotalMemoria = total;
        MemoriaDisponivel = total;
    }

    public bool Alocar(Processo p)
    {
        if (p.MemoriaNecessaria <= MemoriaDisponivel)
        {
            MemoriaDisponivel -= p.MemoriaNecessaria;
            return true;
        }
        return false;
    }

    public void Liberar(Processo p)
    {
        MemoriaDisponivel += p.MemoriaNecessaria;
        if (MemoriaDisponivel > TotalMemoria)
            MemoriaDisponivel = TotalMemoria;
    }
}
