using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Tabu
    {
        Populacao[] Vizinhos = new Populacao[Program.Constants.QuantVizinhos];
        int maiorFitAnterior;
        public string[] melhorSol = new string[2]; //{escolhas , fitness}
        public int countToBTMax = 0;
        List<string> ListaTabu = new List<string>();
        List<List<string>> listaVizinhos = new List<List<string>>();
        public double BTMax = Utilities.chao((int)(4500 - 1000 * Math.Log10(Program.Constants.TamanhoPopulacao)), 0,500);

        public Tabu(string escolhas, int fitness)
        {
            ListaTabu.Add(escolhas);
            melhorSol[0] = escolhas;
            melhorSol[1] = fitness.ToString();
            maiorFitAnterior = fitness;
        }

        public void GerarVizinhanca(int increaseMut)
        {
            string[] vizinho = new string[Program.Constants.QuantFloor * 3];
            int andar, item;
            int[] compra;
            for (int j = 0; j < Program.Constants.QuantVizinhos; j++)
            {
                vizinho = ListaTabu.Last().Split(',');
                for (int i = 0; i < increaseMut+1; i++)
                {
                    andar = Utilities.rnd.Next(Program.Constants.QuantFloor);
                    item = Utilities.rnd.Next(3);
                    compra = new int[] { Utilities.rnd.Next(Program.Constants.QuantOptions), Utilities.rnd.Next(Utilities.teto(andar * 7, 100)), Utilities.rnd.Next(Utilities.teto(andar * 4, 100)) };
                    vizinho[andar * 3 + item] = compra[item].ToString();
                }
                Vizinhos[j] = new Populacao(String.Join(",", vizinho));
                Vizinhos[j].definirEscolhas();
            }
        }

        public void AvaliarVizinhanca(AG ag, Game floor) //Atualiza a lista também
        {
            listaVizinhos = Utilities.ordenarListIndex(ag.avaliacao(ref Vizinhos, floor), 1);

            for (int i = 0; i < listaVizinhos[1].Count(); i++)
                if (!ListaTabu.Contains(Vizinhos[int.Parse(listaVizinhos[1][i])].escolhas))
                {
                    ListaTabu.Add(Vizinhos[int.Parse(listaVizinhos[1][i])].escolhas);
                    //Console.WriteLine(listaVizinhos[0][i]);
                    break;
                }

            if (ListaTabu.Count > Program.Constants.QuantFilhos) ListaTabu.RemoveAt(0);
            if (int.Parse(melhorSol[1]) < int.Parse(listaVizinhos[0][0]))
            {
                melhorSol[0] = Vizinhos[int.Parse(listaVizinhos[1][0])].escolhas;
                melhorSol[1] = listaVizinhos[0][0];
                Console.WriteLine(melhorSol[1] + " " + ((int)(countToBTMax * 0.01)+1));
                countToBTMax = 0;
            }
            countToBTMax++;
        }

        public Populacao[] PopulacaoFinal(AG ag, Game floor)
        {
            if (ListaTabu.Contains(melhorSol[0])) ListaTabu.RemoveAt(ListaTabu.IndexOf(melhorSol[0]));
            Populacao[] elite = new Populacao[2];
            elite[0] = new Populacao(melhorSol[0].ToString());
            for (int i = 1; i < 2; i++)
            {
                elite[i] = new Populacao(ListaTabu[Utilities.rnd.Next(ListaTabu.Count)]);
            }
            ag.avaliacao(ref elite, floor);

            if (int.Parse(melhorSol[1]) == maiorFitAnterior)
                countToBTMax = 100000;

            return elite;
        }
    }
}
