using SistemasOperacionais.Enum;
using SistemasOperacionais.Model;

public class Nucleo
{
    public int Id { get; }
    public ThreadSo ThreadAtual { get; private set; }
    public bool Disponivel => ThreadAtual == null || ThreadAtual.Terminou;

    public Nucleo(int id) => Id = id;

    public void Atribuir(Processo p)
    {
        if (!Disponivel)
            return;
        ThreadAtual = new ThreadSo(p);
        p.AlterarEstado(Estado.Executando);
    }

    public void ExecutarCiclo(int quantum)
    {
        if (ThreadAtual == null)
            return;
        ThreadAtual.ExecutarCiclo(quantum);
        if (ThreadAtual.Terminou)
            ThreadAtual = null;
    }

    public Processo Preemptar()
    {
        if (ThreadAtual == null)
            return null;
        var p = ThreadAtual.ProcessoPai;
        ThreadAtual = null;
        return p;
    }
}