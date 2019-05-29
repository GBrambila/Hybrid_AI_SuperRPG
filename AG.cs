using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class AG
    {
        List<int> paisInd;
        public AG()
        {
            paisInd = new List<int>();
        }
        public void popular(ref Populacao[] populacao, int popSize)
        {
            for (int i = 0; i < populacao.Length; i++)
            {
                populacao[i] = new Populacao();
            }
        }

        public List<int> avaliacao(ref Populacao[] populacao, Game floor)
        {
            int[] compra;
            List<int> fitness = new List<int>();
            for (int i = 0; i < populacao.Length; i++)
            {
                Atributos player = new Atributos();

                for (int f = 0; f < Program.Constants.QuantFloor; f++)
                {
                    compra = new int[3] { int.Parse(populacao[i].gene[f * 3]), int.Parse(populacao[i].gene[f * 3 + 1]), int.Parse(populacao[i].gene[f * 3 + 2]) };
                    floor.Shop(f, ref compra, player);  //Execução da compra e do turno (batalhas)
                    populacao[i].gene[f * 3] = compra[0].ToString();
                    populacao[i].gene[f * 3 + 1] = compra[1].ToString();
                    populacao[i].gene[f * 3 + 2] = compra[2].ToString();
                }
                populacao[i].fitness = player.fitness;
                populacao[i].fitnessSum = player.fitness.Sum();
                fitness.Add(populacao[i].fitnessSum);
                populacao[i].definirEscolhas();
            }
            return fitness;
        }

        public void selecao(ref Populacao[] populacao, Output saida, List<int> fitness)
        {
            Populacao cromo1, cromo2;

            int winner, index1, index2;
            do
            {
                index1 = Utilities.rnd.Next(populacao.Length);
                do index2 = Utilities.rnd.Next(populacao.Length);
                while (index1 == index2);
                cromo1 = populacao[index1];
                cromo2 = populacao[index2];
                winner = Utilities.rnd.Next(2); //Caso de empate, esse rnd vai decidir
                winner = cromo1.fitnessSum > cromo2.fitnessSum ? index1 : cromo2.fitnessSum > cromo1.fitnessSum ? index2 : winner == 0 ? index1 : index2;
                if (paisInd.Count == 0) paisInd.Add(winner);
                else if (!paisInd.Exists(x => x == winner)) paisInd.Add(winner);
            } while (paisInd.Count < Program.Constants.QuantFilhos);
        }

        public void classificacao(ref Populacao[] populacao, Output saida, List<int> fitness)
        {
            int S, P;
            int chosenOne = 0;
            List<List<string>> classificacao = new List<List<string>>();
            classificacao = Utilities.ordenarListIndex(fitness);
            S = classificacao[1].Sum(x => int.Parse(x) + 1);
            do
            {
                P = Utilities.rnd.Next(S);
                for (int i = 0; S >= P; i++)
                {
                    P += i + 1;
                    if (S < P)
                    {
                        chosenOne = int.Parse(classificacao[1][i - 1]);
                    }
                }
                if (paisInd.Count == 0) paisInd.Add(chosenOne);
                else if (!paisInd.Exists(x => x == chosenOne)) paisInd.Add(chosenOne);
            } while (paisInd.Count < Program.Constants.QuantFilhos);
        }

        public void recombinacao(ref Populacao[] populacao, ref Populacao[] filho)
        {
            int particao, particao2;
            for (int j = 0; j < filho.Length; j += 2)
            {

                particao = Utilities.rnd.Next(1, Program.Constants.QuantFloor - 2) * 3;
                particao2 = Utilities.rnd.Next(particao / 3, Program.Constants.QuantFloor - 1) * 3;
                for (int i = 0; i < Program.Constants.QuantFloor * 3; i++)
                {
                    if (i < particao)
                    {
                        filho[j].gene[i] = populacao[paisInd[j]].gene[i];
                        filho[j + 1].gene[i] = populacao[paisInd[j + 1]].gene[i];
                    }
                    else if (i < particao2)
                    {
                        filho[j].gene[i] = populacao[paisInd[j + 1]].gene[i];
                        filho[j + 1].gene[i] = populacao[paisInd[j]].gene[i];
                    }
                    else
                    {
                        filho[j].gene[i] = populacao[paisInd[j]].gene[i];
                        filho[j + 1].gene[i] = populacao[paisInd[j + 1]].gene[i];
                    }
                }
                mutacao(ref filho[j]);
                mutacao(ref filho[j + 1]);
            }
        }
        void mutacao(ref Populacao filho)
        {
            for (int i = 0; i < Program.Constants.QuantFloor*3; i++)
            {
                if (Program.Constants.TaxaMutacao > Utilities.rnd.NextDouble())
                    if (i % 3 == 0)
                        filho.gene[i] = Utilities.rnd.Next(4).ToString(); //Escolha de armas e escudos vai até 3
                    else if (i % 3 == 1)
                        filho.gene[i] = Utilities.rnd.Next(Utilities.teto((int)(i/3) * int.Parse(filho.gene[i]), 100)).ToString(); //Quantidade de pots no gene vai crescer conforme passa os andares e também depedendo da quantidade atual do gene,com um maximo de 100
                    else
                        filho.gene[i] = Utilities.rnd.Next(Utilities.teto((int)(i / 3)* int.Parse(filho.gene[i]), 100)).ToString(); //Mesma lógica pros pingentes

            }
        }

        public void sobrevivem(ref Populacao[] populacao, Populacao[] filho, Output saida, int geracao, ref List<int> fitness)
        {
            int decisao;
            bool repetido = false;
            int menor = fitness.IndexOf(fitness.Min());
            int fitMin = fitness.Min();
            int filhoFit;
            for (int i = 0; i < filho.Length; i++)
            {
                filhoFit = filho[i].fitnessSum;
                decisao = Utilities.rnd.Next(2);
                if (fitMin < filhoFit || fitMin == filhoFit && decisao == 1)
                {
                    for (int j = 0; j < populacao.Length; j++)
                    {
                        if (populacao[j].escolhas == filho[i].escolhas)
                        {
                            repetido = true;
                            break;
                        }

                    }
                    if (repetido != true)
                    {
                        populacao[menor] = filho[i];
                        fitness[menor] = filho[i].fitnessSum;
                        menor = fitness.IndexOf(fitness.Min());
                        fitMin = fitness.Min();

                    }
                    else
                    {
                        repetido = false;
                    }
                }

            }
            /* for (int j = 0; j < Program.Constants.TamanhoPopulacao; j++)
             {
                 Console.WriteLine(pop[i].escolhas + " " + pop[i].fitness + " " + pop[i].vitorias);
             }*/
            saida.outputs(fitness);
            populacao[fitness.IndexOf(saida.maior)].fitness.ForEach(i => Console.Write("\t{0}", i));
            //Console.Write("    \t" + populacao[fitnessOfAll.IndexOf(fitnessOfAll.Max())].escolhas);
        }

        public void catastrofe(ref Populacao[] populacao, Game floor, ref List<int> fitnessOfAll)
        {
            int chance;
            Populacao[] novosMembros = new Populacao[(int)(Program.Constants.TamanhoPopulacao * Program.Constants.PotencialCatastrofe)];
            for (int i = 0; i < novosMembros.Length; i++)
            {
                novosMembros[i] = new Populacao();
            }
            avaliacao(ref novosMembros, floor);
            for (int i = 0; i < novosMembros.Length; i++)
            {
                do
                {
                    chance = Utilities.rnd.Next(Program.Constants.TamanhoPopulacao);
                } while (chance == fitnessOfAll.IndexOf(fitnessOfAll.Max()));
                populacao[chance] = novosMembros[i];
                fitnessOfAll[chance] = novosMembros[i].fitnessSum;

            }
        }
    }
}
