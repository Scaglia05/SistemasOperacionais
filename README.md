# 🖥️ Simulador de Sistema Operacional

## 👥 Integrantes
Guilherme Augusto Scaglia - Ra: 111598 => scaglia@alunos.fho.edu.br

João Pedro Denardo - Ra: 113036 => denardo749@alunos.fho.edu.br

Pedro Henrique Oliveira de Souza - Ra: 113364 => pedro1204@alunos.fho.edu.br

---

## 📌 Sobre o Projeto
O **Simulador de Sistema Operacional** é uma aplicação desenvolvida em **C#** com o objetivo de **ilustrar e comparar políticas de escalonamento de processos**.  
Ele recria o comportamento de um ambiente multiprocessado, considerando **tempo de CPU, consumo de memória e estados de processos**.  

A proposta é oferecer uma ferramenta acadêmica e didática que auxilie na compreensão prática de conceitos fundamentais de **Sistemas Operacionais**.

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
- **BaseSo/** → Classes principais (CPU, Núcleo, Processo, Thread).  
- **Enum/** → Definições de estados e políticas de escalonamento.  
- **Program.cs** → Ponto de entrada e interação com o usuário.  

---

## 🛠️ Tecnologias Utilizadas
- **C# .NET**  
- **Console Application**  

---

## 🎯 Objetivo
Este simulador foi criado com a finalidade de **aproximar a teoria da prática**, mostrando de forma clara como cada política de escalonamento impacta a execução de processos.  
É uma ferramenta de apoio para estudantes e pesquisadores interessados em **arquitetura de computadores** e **sistemas operacionais**.

---

## 📖 Observações
- O projeto é **acadêmico** e voltado ao aprendizado.  
- Pode ser expandido para novas políticas de escalonamento, métricas de desempenho ou visualização gráfica.  

---

👨‍💻 Desenvolvido no curso de **Engenharia da Computação**.  
