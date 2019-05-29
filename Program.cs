using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace SuperRPG__CSharp_
{
    class Program
    {
        public static class Constants
        {
            public const int QuantOptions = 4;
            public const int QuantFloor = 27; //NUNCA COLOQUE ACIMA DE 27
            public const int QuantGenes = QuantFloor * 3;
            public const int AndarMin = 24;
            public const int TamanhoPopulacao = 1000;
            public const float TaxadeCruzamento = 0.2f;
            public const float TaxaMutacao = 0.08f;
            public const int QuantFilhos = (int)(TamanhoPopulacao * TaxadeCruzamento);
            public const int QuantVizinhos = (int)(TamanhoPopulacao * TaxadeCruzamento);
            public const float QuantMutTabu = 1;
            public const float PotencialCatastrofe = 3.0f;
        }

        public static void Main(string[] args)
        {
            Output saida = new Output();

            var excel = new Excel.Application();
            var workbook = excel.Workbooks.Open(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Monstros.xlsx"));

            Excel._Worksheet xlWorksheet = workbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;


            Game floor = new Game(rowCount);
            
            for (int i = 1; i <= rowCount; i++)
            {
                floor.monstro[i - 1] = new Monstro((int)xlRange.Cells[i, 1].Value2,
                    (int)xlRange.Cells[i, 2].Value2, (int)xlRange.Cells[i, 3].Value2, (int)xlRange.Cells[i, 4].Value2);
            }
            workbook.Close();

            List<int> fitnessTotal;
            AG ag = new AG();
            Tabu tabu = null;
            bool done = false;
            Populacao[] pop = new Populacao[Constants.TamanhoPopulacao];
            Populacao[] filho = new Populacao[Constants.QuantFilhos];
            ag.popular(ref pop, Constants.TamanhoPopulacao);
            ag.popular(ref filho, Constants.QuantFilhos);
            double LimiteSemMelhora = 500;
        fitnessTotal = ag.avaliacao(ref pop, floor);

            for (int i = 0; done == false; i++)
            {
                ag.classificacao(ref pop, saida, fitnessTotal);

                ag.recombinacao(ref pop, ref filho);

                ag.avaliacao(ref filho, floor);

                ag.sobrevivem(ref pop, filho, saida, i, ref fitnessTotal);

                filho = new Populacao[Constants.QuantFilhos];

                ag.popular(ref filho, Constants.QuantFilhos);

                if (saida.denovo >= LimiteSemMelhora)
                {
                    saida.denovo = 0;
                    Console.WriteLine("\n\nTABU");
                    tabu = new Tabu(pop[fitnessTotal.IndexOf(saida.maior)].escolhas, pop[fitnessTotal.IndexOf(saida.maior)].fitnessSum);

                    do
                    {
                        tabu.GerarVizinhanca((int)(tabu.countToBTMax * 0.01));

                        tabu.AvaliarVizinhanca(ag, floor);

                    } while (tabu.countToBTMax < tabu.BTMax);

                    ag.sobrevivem(ref pop, tabu.PopulacaoFinal(ag, floor), saida, i, ref fitnessTotal);

                    if(tabu.countToBTMax== 100000 && floor.alcancou==true)
                    {
                        done = true;
                        break;
                    }
                    else if(tabu.countToBTMax==100000)
                    {
                        ag.catastrofe(ref pop, floor, ref fitnessTotal);

                    }
                
                }
            }
            saida.descricaoCromo(pop[fitnessTotal.IndexOf(fitnessTotal.Max())].gene);
            Console.WriteLine("\n\n" + pop[fitnessTotal.IndexOf(fitnessTotal.Max())].escolhas);
            Console.ReadLine();
        }
    }
}
