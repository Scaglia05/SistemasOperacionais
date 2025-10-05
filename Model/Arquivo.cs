using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemasOperacionais.Model
{
    public class Arquivo
    {
        public string Nome { get; set; }
        public int Tamanho { get; private set; }
        public bool Leitura { get; set; } = true;
        public bool Escrita { get; set; } = true;
        public DateTime Criacao { get; } = DateTime.Now;
        public DateTime UltimoAcesso { get; private set; } = DateTime.Now;
        public DateTime UltimaModificacao { get; private set; } = DateTime.Now;

        public void Ler() => UltimoAcesso = DateTime.Now;

        public void Escrever(int novoTamanho)
        {
            if (!Escrita)
                throw new Exception("Permissão negada");
            Tamanho = novoTamanho;
            UltimaModificacao = UltimoAcesso = DateTime.Now;
        }
    }

    public class Diretorio
    {
        public string Nome { get; }
        public List<Diretorio> SubDirs { get; private set; } = new();
        public List<Arquivo> Arquivos { get; private set; } = new();

        public Diretorio(string nome) => Nome = nome;
        public void AddArquivo(Arquivo a) => Arquivos.Add(a);
        public void AddSubDir(Diretorio d) => SubDirs.Add(d);
        public void RemoverArquivo(string nome) => Arquivos.RemoveAll(a => a.Nome == nome);
        public void RemoverSubDir(string nome) => SubDirs.RemoveAll(d => d.Nome == nome);

        public List<string> Listar()
        {
            var lista = new List<string>();
            lista.AddRange(SubDirs.Select(d => $"DIR : {d.Nome}"));
            lista.AddRange(Arquivos.Select(a => $"ARQ : {a.Nome} ({a.Tamanho}B)"));
            return lista;
        }
    }

    public class SistemaArquivos
    {
        public Diretorio Raiz { get; } = new Diretorio("root");
        public Dictionary<int, List<Arquivo>> TabelaArquivosPorProcesso { get; } = new();

        public void Abrir(Processo p, Arquivo a)
        {
            if (!TabelaArquivosPorProcesso.ContainsKey(p.Id))
                TabelaArquivosPorProcesso[p.Id] = new List<Arquivo>();
            TabelaArquivosPorProcesso[p.Id].Add(a);
        }

        public void Fechar(Processo p, Arquivo a)
        {
            if (TabelaArquivosPorProcesso.ContainsKey(p.Id))
                TabelaArquivosPorProcesso[p.Id].Remove(a);
        }
    }
}
