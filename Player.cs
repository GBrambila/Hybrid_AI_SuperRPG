using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Atributos
    {
        public int ataque = 1,
            defesa = 0,
            hp = 10,
            hpatual = 10,
            coins = 15,
            morreu = 0,
            dano = 1;
        public List<int> fitness = new List<int> { 0,0,0,0,0};

        public void definirDano(Monstro monstro)
        {
            dano = Utilities.chao(ataque,monstro.defesa);
        }
    }
}
