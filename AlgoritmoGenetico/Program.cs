using System;

namespace AlgoritmoGenetico
{
    internal class Program
    {
        private static readonly Random Random = new Random();

        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Alocação de tarefas");
            Console.ResetColor();
            Console.WriteLine();

            var nomesTrabalhadores = new List<string>
            {
                "Steve", "Robert", "Susan", "Greg", "Austin", "Joe", "Frank", "Abu", "Kelly", "Michael"
            };

            var tarefas = AtribuirTarefas();

            var populacao = new List<List<Trabalhador>>();
            var avaliacao = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                var individuo = InicializarPopulacao(nomesTrabalhadores, tarefas);
                populacao.Add(individuo);

                ExibirIndividuo(i + 1, individuo);

                var resultadoAvaliacao = AvaliarPopulacao(individuo);
                Console.WriteLine($"Resultado da avaliação do indivíduo {i + 1}: {resultadoAvaliacao}");
                Console.WriteLine("-----------------------------");

                avaliacao.Add(resultadoAvaliacao);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nIniciando reprodução...");

            Console.WriteLine($"Selecionando pais...");
            var (paiIndex, maeIndex) = SelecionarPais(populacao.Count);

            Console.WriteLine($"Pais selecionados: ID Pai: {paiIndex + 1}, ID Mae: {maeIndex + 1} ");
            Console.ResetColor();

            var (cruzamentoUm, cruzamentoDois) = AplicarCruzamento(populacao[paiIndex], populacao[maeIndex]);
            ExibirIndividuo(1, cruzamentoUm);
            ExibirIndividuo(2, cruzamentoDois);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nSelecionando o melhor indivíduo...");

            var melhorIndividuo = SelecionarMelhorIndividuo(avaliacao, populacao);

            Console.WriteLine($"Indivíduo selecionado: {populacao.IndexOf(melhorIndividuo) + 1}");
            Console.ResetColor();

            var geracao = new List<List<Trabalhador>>
            {
                cruzamentoUm.Select(t => t.Clone()).ToList(),
                cruzamentoDois.Select(t => t.Clone()).ToList(),
                melhorIndividuo.Select(t => t.Clone()).ToList(),
                melhorIndividuo.Select(t => t.Clone()).ToList()
            };

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n\tGeração 01");
            Console.ResetColor();
            ExibirGeracao(geracao);

            //TO DO: MUTAÇÃO

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nRealizando Mutação");
            Console.ResetColor();

            AplicarMutacao(geracao);

            Console.ReadLine();
        }

        private static void AplicarMutacao(List<List<Trabalhador>> geracao)
        {
            var selecaoAleatoria = Random.Next(0, 4);
            var individuoSelecionado = geracao[selecaoAleatoria];
            var trabalhadorIndex = Random.Next(0, 10);

            var trabalhador = individuoSelecionado[trabalhadorIndex];
            var notaAnterior = trabalhador.Nota;

            Console.WriteLine($"Indivíduo selecionado para mutação: {selecaoAleatoria + 1}");
            Console.WriteLine($"Trabalhador selecionado: {trabalhador.Nome} - Nota anterior: {notaAnterior}");

            do
            {
                trabalhador.Nota = Random.Next(1, 11);
            } while (notaAnterior == trabalhador.Nota);

            Console.WriteLine($"Nota após mutação: {trabalhador.Nota}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nMutação:");
            Console.ResetColor();

            ExibirGeracao(geracao);
        }

        private static List<Trabalhador> InicializarPopulacao(List<string> nomesTrabalhadores, List<int> tarefas)
        {
            var trabalhadores = new List<Trabalhador>();
            for (var i = 0; i < 10; i++)
            {
                var notaAleatoria = Random.Next(1, 11);
                var trab = new Trabalhador
                {
                    Nome = nomesTrabalhadores[i],
                    Tarefa = tarefas[i],
                    Nota = notaAleatoria
                };
                trabalhadores.Add(trab);
            }
            return trabalhadores;
        }

        private static int AvaliarPopulacao(List<Trabalhador> trabalhadores)
        {
            return trabalhadores.Sum(trabalhador => trabalhador.Nota);
        }

        private static (int, int) SelecionarPais(int populacaoCount)
        {
            var paiIndex = Random.Next(populacaoCount);
            var maeIndex = Random.Next(populacaoCount);
            while (maeIndex == paiIndex)
            {
                maeIndex = Random.Next(populacaoCount);
            }
            return (paiIndex, maeIndex);
        }

        private static (List<Trabalhador>, List<Trabalhador>) AplicarCruzamento(List<Trabalhador> pai, List<Trabalhador> mae)
        {
            var metadePaiUm = pai.GetRange(0, 5);
            var metadeMaeUm = mae.GetRange(5, 5);

            var cruzamentoUm = metadePaiUm.Concat(metadeMaeUm).ToList();
            var cruzamentoDois = metadeMaeUm.Concat(metadePaiUm).ToList();

            return (cruzamentoUm, cruzamentoDois);
        }

        private static List<Trabalhador> SelecionarMelhorIndividuo(List<int> avaliacao, List<List<Trabalhador>> populacao)
        {
            var indiceMaiorValor = avaliacao.IndexOf(avaliacao.Max());
            return populacao[indiceMaiorValor];
        }

        private static void ExibirIndividuo(int index, List<Trabalhador> individuo)
        {
            Console.WriteLine($"Indíviduo {index}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Nome\t\tTarefa\tNota");
            Console.ResetColor();
            foreach (var t in individuo)
            {
                Console.WriteLine($"{t.Nome}\t\t{t.Tarefa}\t{t.Nota}");
            }
        }

        private static void ExibirGeracao(List<List<Trabalhador>> geracao)
        {
            foreach (var lista in geracao)
            {
                ExibirIndividuoLista(lista);
                Console.WriteLine("Resultado avaliação: {0}", AvaliarPopulacao(lista));
            }
        }

        private static void ExibirIndividuoLista(List<Trabalhador> lista)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Nome\t\tTarefa\tNota");
            Console.ResetColor();
            foreach (var t in lista)
            {
                Console.WriteLine($"{t.Nome}\t\t{t.Tarefa}\t{t.Nota}");
            }
        }

        private static List<int> AtribuirTarefas()
        {
            Random rand = new Random();

            var tarefas = new List<int>();

            var numerosUtilizados = new HashSet<int>();

            while (tarefas.Count < 10)
            {
                int tarefaAleatoria = rand.Next(1, 11);

                if (!numerosUtilizados.Contains(tarefaAleatoria))
                {
                    tarefas.Add(tarefaAleatoria);
                    numerosUtilizados.Add(tarefaAleatoria);
                }
            }

            return tarefas;
        }
    }
}



