using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Populacao
    {
        public int fitnessSum = 0;
        public List<int> fitness = new List<int>();
        public int vitorias = 0;
        public string escolhas;
        public string[] gene;
        public bool alcancado=false;
        public Populacao(string cromo = "")
        {
            gene = new string[Program.Constants.QuantFloor*3];
            if (cromo != "")
            {
                gene = cromo.Split(',');
            }
            else
                for (int i = 0; i < Program.Constants.QuantFloor; i++)
                {
                    gene[i*3] = Utilities.rnd.Next(Program.Constants.QuantOptions).ToString();
                    gene[i * 3 + 1] = Utilities.rnd.Next(Utilities.teto(i * 7, 100)).ToString();
                    gene[i * 3 + 2] = Utilities.rnd.Next(Utilities.teto(i*4,100)).ToString();
                }

        }

        public void definirEscolhas()
        {
            escolhas = String.Join(",",gene);
        }

    }
}
