using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Utilities //Funcionalidades
    {
        public static Random rnd = new Random();
        static double tempoInicial = System.DateTime.Now.TimeOfDay.TotalSeconds;
        public static void definirRandom()
        {
            rnd = new Random(9864);
        }

        public static double tempo()
        {
            return System.DateTime.Now.TimeOfDay.TotalSeconds - tempoInicial;

        }

        public static List<List<string>> ordenarListIndex(List<int> original, int order = 0)
        {

            if (order == 0) //crescente
            {
                var sorted = original
                    .Select((x, i) => new KeyValuePair<int, int>(x, i))
                    .OrderBy(x => x.Key)
                    .ToList();
                List<int> B = sorted.Select(x => x.Key).ToList();
                List<int> idx = sorted.Select(x => x.Value).ToList();
                List<List<string>> Matrix = new List<List<string>>();
                Matrix.Add(B.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
                Matrix.Add(idx.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
                return Matrix;
            }
            else
            {
                var sorted = original
                    .Select((x, i) => new KeyValuePair<int, int>(x, i))
                    .OrderByDescending(x => x.Key)
                    .ToList();
                List<int> B = sorted.Select(x => x.Key).ToList();
                List<int> idx = sorted.Select(x => x.Value).ToList();
                List<List<string>> Matrix = new List<List<string>>();
                Matrix.Add(B.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
                Matrix.Add(idx.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
                return Matrix;
            }
            
        }

        public static int chao(int a, int b,int chao=0)
        {
            int min = a - b;
            if (min < 0)
                min = 0;
            return min;
        }

        public static int teto(int value, int max)
        {
            if (value > max)
                return max;
            return value;
        }
        public static string inverterString(string str)
        {
            char[] c = str.ToCharArray();
            Array.Reverse(c);
            string invertida = new String(c);

            return invertida;
        }

        public static long trocarBase(int numero, int novaBase)
        {
            string total = "";
            while (numero >= novaBase)
            {
                total += numero % novaBase;
                numero /= novaBase;
            }
            total += numero;
            total = inverterString(total);
            return long.Parse(total);
        }
        public static string colorir(float dimensao)
        {
            //   Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(dimensao);
            Console.ResetColor();
            return "";
        }
        public static int checarResto(int dividendo, int divisor)
        {
            if (dividendo % divisor != 0)
                return 1;
            return 0;
        }
        public static string[] strTostrArr(string str)
        {
            string[] strArr = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                strArr[i] = str.ToString()[i].ToString();
            }
            return strArr;
        }



        /*public static void definirRanks(List<int> list,Populacao []pop)
        {
            for (int i = 0; i < pop.Length; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] == pop[i].fitness)
                    {
                        pop[i].rank = j;
                        list[j] = -1000;
                    } 
                }
            }
        }*/
    }
}
