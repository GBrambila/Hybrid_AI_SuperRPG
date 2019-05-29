using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperRPG__CSharp_
{
    class Game
    {
        public Monstro[] monstro = new Monstro[200];
        Atributos playerFloor;
        public bool alcancou = false;
        public double totalTempo=0.0;
        public int[] weapon = new int[] { 2, 3, 4, 5, 6, 7, 8, 8, 10, 10, 12, 12, 14, 14, 16, 16, 18, 20, 20, 20, 30,30,30,30,30,30,30,30 };
        public int[] pW = new int[] { 10, 15, 20, 30, 40, 50, 100, 100, 180, 180, 250, 250, 400, 400, 600, 600, 1000, 1500, 1500, 1500, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000 };
        public int[] shield = new int[] { 1, 2, 3, 4, 5, 6, 6, 8, 8, 10, 10, 12, 12, 14, 14, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 };
        public int[] pS = new int[] { 10, 15, 20, 30, 40, 50, 50, 100, 100, 180, 180, 250, 250, 400, 400, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600, 600 };
        public int[] pot = new int[] { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
        public int[] pP = new int[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25 };
        public int[] crystal = new int[] { 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
        public int[] pC = new int[] { 0, 0, 0, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 };
        
        //"Começa do", "quant de creat","quant de creat"
        // "Slime", "ASlimes", "Demons", "ADemons","Skelectons","ASkelectons","Spider Swarms"

        public Game(int rows)
        {
        }
        public void Shop(int floor, ref int[] compra, Atributos player)
        {
            
            playerFloor = player;
            int coins = playerFloor.coins, ataque = playerFloor.ataque;
            //Parte da compra, dependente da escolha
            do
            {
                playerFloor.coins = coins;
                playerFloor.ataque = ataque;
                switch (compra[0])
                {
                    case 0:
                        break;
                    case 1: //Comprar weapon
                        comprarEquip(floor, ref playerFloor.ataque, weapon[floor], pW[floor]);
                        break;
                    case 2: //Comprar shield
                        comprarEquip(floor, ref playerFloor.defesa, shield[floor], pS[floor]);
                        break;
                    case 3: //Comprar weapon e shield
                        comprarEquip(floor, ref playerFloor.ataque, weapon[floor], pW[floor]);
                        comprarEquip(floor, ref playerFloor.defesa, shield[floor], pS[floor]);
                        break;
                }
                if (playerFloor.coins < 0)
                    compra[0] = Utilities.rnd.Next(4);
            } while (playerFloor.coins < 0);

            if (compra[1] > potsPossiveis(floor))
                compra[1] = potsPossiveis(floor);
            for (int j = 0; j < compra[1]; j++)
            {
                comprarPot(floor);
            }
            if (compra[2] > crystalsPossiveis(floor))
                compra[2] = crystalsPossiveis(floor);
            for (int k = 0; k < compra[2]; k++)
            {
                comprarCrystal(floor);
            }
            playerFloor.fitness[4] -= (coins - playerFloor.coins) / (playerFloor.morreu + 5);

            Turn(floor);
        }

        public void Turn(int floor)
        {
            for (int ordinal = 0; ordinal < 3 && playerFloor.morreu < 3; ordinal++)
            {
                int indMonstro = floor * 3+ordinal;
                playerFloor.coins += monstro[indMonstro].drop;
                monstro[indMonstro].definirStatus(playerFloor);
                playerFloor.definirDano(monstro[indMonstro]);

                if (floor == Program.Constants.AndarMin && playerFloor.morreu < 1)
                {
                    alcancou = true;
                    // Func.tempo();
                }

               /* if (dano > 0)
                {
                    int quantAtaque = hpC / dano;
                    quantAtaque += Utilities.checarResto(hpC, dano);
                    int quantAtaqueM;
                    if (danoM > 0)
                    {
                        quantAtaqueM = playerFloor.hpatual / danoM;
                        quantAtaqueM += Utilities.checarResto(playerFloor.hpatual, danoM);
                    }
                    else
                        quantAtaqueM = 10000;
                    if (quantAtaque <= quantAtaqueM)
                    {
                        playerFloor.fitness[0] += quantAtaque * dano;
                        playerFloor.fitness[1] -= (quantAtaque - 1) * danoM / (playerFloor.morreu + 2);
                        playerFloor.hpatual -= (quantAtaque - 1) * danoM;
                    }
                    else
                    {
                        playerFloor.fitness[0] += quantAtaqueM * dano;
                        playerFloor.morreu += (int)(quantAtaque / quantAtaqueM);
                        if(!Convert.ToBoolean( Utilities.checarResto(quantAtaque, quantAtaqueM)))
                            playerFloor.morreu-=1;
                        playerFloor.fitness[1] -= quantAtaqueM * danoM / (playerFloor.morreu + 2);

                    }
                }
                else
                {
                    playerFloor.fitness[3] -= playerFloor.morreu * 500;
                    playerFloor.morreu++;
                    continue;
                }
                playerFloor.fitness[2] += 1000;
                playerFloor.fitness[3] -= playerFloor.morreu * 500;*/
                
                while (monstro[indMonstro].hpatual > 0)
                {
                    if (playerFloor.dano > 0)
                    {
                        playerFloor.fitness[0] += playerFloor.dano;
                        monstro[indMonstro].hpatual -= playerFloor.dano;
                    }
                    if (monstro[indMonstro].hpatual > 0 && monstro[indMonstro].dano > 0)
                    {
                        playerFloor.fitness[1] -= monstro[indMonstro].dano / (playerFloor.morreu+2);
                        playerFloor.hpatual -= monstro[indMonstro].dano;

                    }
                    if (playerFloor.hpatual <= 0 || playerFloor.dano <= 0)
                    {
                        playerFloor.hpatual = playerFloor.hp;
                        playerFloor.morreu++;
                        if (playerFloor.dano <= 0)
                        {
                            break;
                        }
                    }
                }

                if (monstro[indMonstro].hpatual <= 0)
                {
                    playerFloor.fitness[2] += 1000;
                    playerFloor.fitness[3] -= playerFloor.morreu * 500;
                }
                
            }
        }

        public void comprarEquip(int floor, ref int atributo, int atriItem, int preco)
        {
            if (atributo == atriItem || playerFloor.coins < preco)
            {
                playerFloor.coins -= 10000;
            }
            else
            {
                playerFloor.coins -= preco;
                atributo = atriItem;
            }
        }

        public int potsPossiveis(int floor)
        {
            int hpFaltante = playerFloor.hp - playerFloor.hpatual;
            int quantPot = hpFaltante / pot[floor];
            
            quantPot += Utilities.checarResto(hpFaltante, pot[floor]);

            int quantPotByPrice = playerFloor.coins / pP[floor];

            if (quantPot <= quantPotByPrice)
                return quantPot;
            else
                return quantPotByPrice;
        }

        public void comprarPot(int floor)
        {
            playerFloor.coins -= pP[floor];
            playerFloor.hpatual += pot[floor];
            if (playerFloor.hpatual > playerFloor.hp)
                playerFloor.hpatual = playerFloor.hp;
        }

        public int crystalsPossiveis(int floor)
        {
            if (floor < 3)
                return 0;
            int quantCrystals = playerFloor.coins / pC[floor];
            return quantCrystals;
        }

        public void comprarCrystal(int floor)
        {
            playerFloor.coins -= pC[floor];
            playerFloor.hp += crystal[floor];
            playerFloor.hpatual += crystal[floor];
        }
    }

}
