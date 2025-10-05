using System;
using System.Collections.Generic;
using SistemasOperacionais.Enum;

namespace SistemasOperacionais.Model
{
    public class Dispositivo
    {
        public string Nome { get; }
        public int TempoServico { get; }
        private Queue<Processo> fila = new();
        private int ciclo = 0;
        public event Action<Processo> Interrupcao;

        public Dispositivo(string nome, int tempo)
        {
            Nome = nome;
            TempoServico = tempo;
        }

        public void AdicionarPedido(Processo p, bool bloqueante = true)
        {
            if (bloqueante)
                p.BloquearParaIO();
            fila.Enqueue(p);
        }

        public void ExecutarCiclo()
        {
            if (fila.Count == 0)
                return;
            ciclo++;
            var p = fila.Peek();
            if (ciclo >= TempoServico)
            {
                fila.Dequeue();
                ciclo = 0;
                p.Desbloquear();
                Interrupcao?.Invoke(p);
            }
        }
    }
}
