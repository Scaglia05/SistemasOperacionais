# 🖥️ Simulador de Sistema Operacional

## 👥 Integrantes
Guilherme Augusto Scaglia - Ra: 111598 => scaglia@alunos.fho.edu.br

João Pedro Denardo - Ra: 113036 => denardo749@alunos.fho.edu.br

Pedro Henrique Oliveira de Souza - Ra: 113364 => pedro1204@alunos.fho.edu.br

---

## 📌 Sobre o Projeto  
O **Simulador de Sistema Operacional** é uma aplicação desenvolvida em **C#** com a finalidade de **demonstrar, comparar e analisar políticas de escalonamento de processos** em um ambiente controlado.  
Ele recria o funcionamento de um **sistema multiprocessado**, considerando fatores como **tempo de CPU, alocação de memória, estados dos processos (pronto, executando e finalizado) e ordem de finalização**.  

Mais do que apenas simular, o projeto busca **aproximar teoria e prática**, permitindo que estudantes e pesquisadores observem de forma clara como cada algoritmo de escalonamento influencia o desempenho, a utilização de recursos e a organização dos processos.  
O simulador oferece uma visão prática de conceitos fundamentais de **Sistemas Operacionais** e **Arquitetura de Computadores**, servindo como **ferramenta didática e acadêmica** para estudos, experimentos e discussões em sala de aula.  
Em suma, ele transforma abstrações teóricas em uma experiência visual e interativa, facilitando a compreensão do comportamento de diferentes políticas de escalonamento em cenários realistas.

---

## 🚀 Funcionalidades
- 🔄 **Políticas de escalonamento**:
  - **FIFO (First In, First Out)** → Executa na ordem de chegada.  
  - **Round Robin** → Escalonamento circular com quantum definido.  
  - **SJF (Shortest Job First)** → Dá prioridade aos processos mais curtos.  
- ⚡ **Execução em múltiplos núcleos simulados**.  
- 💾 **Gerenciamento de memória dinâmica** durante a alocação e liberação.  
- 📊 **Monitoramento do ciclo de execução** com estados atualizados.  
- 🎨 **Interface em console** organizada e colorida para melhor visualização.  

---

## 📂 Estrutura do Projeto
- **Model/** → Classes Auxiliares (CPU, Núcleo, Processo, Thread).
- **BaseSO/** → Classe principal (SoBase).    
- **Enum/** → Definições de estados e políticas de escalonamento.  
- **Program.cs** → Ponto de entrada e interação com o usuário.  

---

## 🛠️ Tecnologias Utilizadas
- **C# .NET**  
- **Console Application**  

---

## 📖 Observações
- O projeto é **acadêmico** e voltado ao aprendizado.  
- Pode ser expandido para novas políticas de escalonamento, métricas de desempenho ou visualização gráfica.  

---

👨‍💻 Desenvolvido no curso de **Engenharia da Computação**.  
