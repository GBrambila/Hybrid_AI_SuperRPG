using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{

    class Monstro
    {
        public int ataque;    
        public int defesa;   
        public int hp;       
        public int hpatual;
        public int drop;
        public int dano;
         public Monstro(int ataque, int defesa, int hp, int drop)
        {
            this.ataque = ataque;
            this.defesa = defesa;
            this.hp = hp;
            hpatual = hp;
            this.drop = drop;
        }

        public void definirStatus(Atributos player)
        {
            dano = Utilities.chao(ataque , player.defesa);
            hpatual = hp;
        }
    }
   

}
