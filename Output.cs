using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Output
    {
        List<string> Maximo;
        public int maiorAtualQuant = 0;
        public int maior = 0;
        public int quantMaisRepetido = 0;
        public int denovo = 0;
        public int denovoTabu = 0;
        public Output()
        {
            Maximo = new List<string>();
        }
        public void outputs(List<int> fitness)
        {
            denovo++;
            if (maior < fitness.Max())
            {
                maior = fitness.Max();
                denovo=0;
            }
            maior = fitness.Max();
            maiorAtualQuant = fitness.Count(x => x == maior);
            //quantMaisRepetido = fitnessOfAll.GroupBy(i => i).OrderByDescending(grp => grp.Count())
      //.Select(grp => grp.Count()).First();
            Console.Write("\n" + fitness.Min() + " " + maior + " " + maiorAtualQuant + " " + fitness.Average()+" ");
            
        }
        public void checarAddNovoMax(string escolhas, int geracao)
        {
            //if (quantMax != maiorAtualQuant)
            {
                Maximo.Add(escolhas + " " + geracao);
            }
        }



        public void descricaoCromo(string[] cromo)
        {
            for (int i = 0; i < Program.Constants.QuantFloor; i++)
            {
                string descricao = "";
                if(int.Parse(cromo[i*3])==1)
                {
                    descricao += "Comprar Espada";
                }
                else if(int.Parse(cromo[i*3])==2)
                {
                    descricao += "Comprar Escudo";
                }
                else if(int.Parse(cromo[i*3]) == 3)
                {
                    descricao += "Comprar Espada e Escudo";
                }
                else
                {
                    descricao += "Nada";
                }
                descricao += ", " + cromo[i*3 + 1]+ " Pots e "+cromo[i*3+2]+ " Cristais.";
                Console.WriteLine("\nAndar " + i +": "+descricao);
            }
        }
        
        public static void escreverAtributos(Atributos player, int floor )
        {
            Console.WriteLine(floor+"\t"+player.ataque + "\t" + player.defesa + "\t" + player.coins + "\t" + player.fitness[3] + "\t" + player.hp + "\t" + player.hpatual + "\t" + player.morreu);
        }
    }
}
