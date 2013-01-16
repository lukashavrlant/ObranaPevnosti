using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    public class HraciDeska/* : ICloneable*/
    {
        public Manazer.StavPole[,] deska
        {
            get;
            private set;
        }

        public int rozmer
        {
            get;
            set;
        }

        public Pozice aktualniTah
        {
            get;
            set;
        }

        public short PocetObrancu
        {
            get;
            private set;
        }

        public HraciDeska()
        {
            Restart();   
        }

        /*public Object Clone()
        {
            HraciDeska NovaHraciDeska = (HraciDeska) MemberwiseClone();
            Pozice NovaPozice = new Pozice(aktualniTah.radek, aktualniTah.sloupec);
            NovaHraciDeska.aktualniTah = NovaPozice;

            return NovaHraciDeska;
        }*/

        /// <summary>
        /// Nastaví hrací desku do počátečního stavu.
        /// </summary>
        public void Restart()
        {
            // Rozměr hrací desky
            rozmer = 7;

            // Nastavíme hrací desku
            deska = new Manazer.StavPole[rozmer, rozmer];

            // Nejprve naplníme desku volnymi pozicemi
            for(int i = 0; i < deska.GetLength(0); i++)
            {
                for(int j = 0; j < deska.GetLength(1); j++)
                {
                    deska[i, j] = Manazer.StavPole.volne;
                }
            }


            // Umístíme útočníky
            for(int i = 0; i < deska.GetLength(0); i++)
            {
                for(int j = 0; j < deska.GetLength(1); j++)
                {
                    if(!Pevnost(i, j))
                    {
                        deska[i, j] = Manazer.StavPole.utok;
                    }
                }
            }


            // Nyní vysekáme rohy hrací desky, protože v rozích nehrajeme
            // Prostě tam střelíme -1: mimo hrací desku
            for(int i = 0; i < deska.GetLength(0); i++)
            {
                for(int j = 0; j < deska.GetLength(1); j++)
                {
                    if(Rohova(i, j))
                    {
                        deska[i, j] = Manazer.StavPole.mimo;
                    }
                }
            }

            // zrušíme aktuální tah
            aktualniTah = null;

            // Vynulujeme počet obránců
            PocetObrancu = 0;

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Projde celou hrací desku a vrátí počet útočníků.
        /// </summary>
        /// <returns>Počet útočníků.</returns>
        public int PocetUtocniku()
        {
            int pocet = 0;

            for (int i = 0; i < rozmer; i++)
                for (int j = 0; j < rozmer; j++)
                {
                    if (deska[i, j] == Manazer.StavPole.utok)
                    {
                        pocet++;
                    }
                }

            return pocet;
        }

        /// <summary>
        /// Zjišťuje, zda útočníci obsadili pevnost.
        /// </summary>
        /// <returns>True: útočníci obsadili pevnost; False: útočníci neobsadili celou pevnost.</returns>
        public bool ObsazenaPevnost()
        {
            for (int i = 0; i < rozmer; i++)
                for (int j = 0; j < rozmer; j++)
                {
                    if (Pevnost(i, j) && deska[i, j] != Manazer.StavPole.utok)
                        return false;
                }

            return true;
        }

        public void NastavObrance(Pozice Obrance)
        {
            if (PocetObrancu < 2)
            {
                if (Pevnost(Obrance.Radek, Obrance.Sloupec))
                {
                    if (Volne(Obrance.Radek, Obrance.Sloupec))
                    {
                        NastavKamen(Obrance, Manazer.StavPole.obrana);
                        PocetObrancu++;
                    }
                    else
                    {
                        throw new Exception("Obránci jsou na stejném místě");
                    }
                }
                else
                {
                    throw new Exception("Obránci nejsou v pevnosti");
                }
            }
            else
            {
                throw new Exception("Všichni obránci již byli nastaveni");
            }
        }


        /// <summary>
        /// Provede tah na desce.
        /// </summary>
        /// <param name="hracuvTah">Instance třídy Tah</param>
        public void Tahni(Tah hracuvTah)
        {
            int i;

            for(i = 1; i < hracuvTah.PocetTahu(); i++)
            {
                Tahni(hracuvTah.seznamTahu[i - 1], hracuvTah.seznamTahu[i]);
            }

            aktualniTah = hracuvTah.seznamTahu[i - 1];
        }

        /// <summary>
        /// Provede tah naopak, tj. vrátí uvedený tah zpátky
        /// </summary>
        public void VratZpatkyTah(Tah hracuvTah)
        {
            if(hracuvTah.PocetTahu() == 1)
            {
                PocetObrancu--;
                NastavKamen(hracuvTah.seznamTahu[0], Manazer.StavPole.volne);
                return;
            }

            int i;

            hracuvTah.seznamTahu.Reverse();

            for(i = 1; i < hracuvTah.PocetTahu(); i++)
            {
                Tahni(hracuvTah.seznamTahu[i - 1], hracuvTah.seznamTahu[i]);
            }

            aktualniTah = hracuvTah.seznamTahu[i - 1];
        }

        /// <summary>
        /// Provede tah na desce.
        /// </summary>
        private void Tahni(Pozice odkud, Pozice kam)
        {
            Tahni(odkud.Radek, odkud.Sloupec, kam.Radek, kam.Sloupec);
        }

        
        /// <summary>
        /// Provede tah na desce se základní kontrolou (je souřadnice opravdu na desce? je políčko volné?)
        /// </summary>
        /// <param name="radekOdkud">Souřadnice řádku, odkud táhneme (číselná souřadnice)</param>
        /// <param name="sloupecOdkud">Souřadnice sloupce, odkud táhneme číselná souřadnice</param>
        /// <param name="radekKam">Souřadnice řádku, kam táhneme číselná souřadnice</param>
        /// <param name="sloupecKam">Souřadnice sloupce, kam táhneme číselná souřadnice</param>
        private void Tahni(int radekOdkud, int sloupecOdkud, int radekKam, int sloupecKam)
        {
            // Jsou obě políčka na desce?
            if(NaDesce(radekKam, sloupecKam) && !Rohova(radekKam, sloupecKam) ||
                NaDesce(radekOdkud, sloupecOdkud) && !Rohova(radekOdkud, sloupecOdkud))
            {
                // Je pole, kam chceme táhnout, volné?
                if(Volne(radekKam, sloupecKam))
                {
                    // Je na políčku, odkud táhneme vůbec nějaký kámen?
                    if(Volne(radekOdkud, sloupecOdkud))
                    {
                        throw new Exception("Pole, odkud chcete táhnout, je prázdné");
                    }
                    else
                    {
                        // Prohodíme kameny
                        NastavKamen(radekKam, sloupecKam, deska[radekOdkud, sloupecOdkud]);
                        NastavKamen(radekOdkud, sloupecOdkud, Manazer.StavPole.volne);
                    }
                }
                else
                {
                    throw new Exception("Pole je obsazené");
                }
            }
            else
            {
                throw new Exception("Pole je mimo hrací desku");
            }
        }

       
        /// <summary>
        /// Bez kontroly nastaví kámen na desku.
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <param name="hrac">Barva hráče, který táhne</param>
        private void NastavKamen(int radek, int sloupec, Manazer.StavPole hrac)
        {
            if(NaDesce(radek, sloupec) && !Rohova(radek, sloupec))
            {
                deska[radek, sloupec] = hrac;
            }
            else
            {
                throw new Exception("Nejsme na hrací desce");
            }
        }

        
        /// <summary>
        /// Bez kontroly nastaví kámen na desku.
        /// </summary>
        /// <param name="pole">Souřadnice, kam chceme kámen položit</param>
        /// <param name="hrac">Barva hráče, který táhne</param>
        private void NastavKamen(Pozice pole, Manazer.StavPole hrac)
        {
            NastavKamen(pole.Radek, pole.Sloupec, hrac);
        }

        
        /// <summary>
        /// Jaký hráč stojí na pozici?
        /// </summary>
        /// <param name="pole">Souřadnice pole</param>
        /// <returns>Barva hráče</returns>
        public Manazer.StavPole Hrac(Pozice pole)
        {
            return Hrac(pole.Radek, pole.Sloupec);
        }

        
        /// <summary>
        /// Jaký hráč stojí na pozici?
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns>Barva hráče</returns>
        public Manazer.StavPole Hrac(int radek, int sloupec)
        {
            return deska[radek, sloupec];
        }
        

        /// <summary>
        /// Odebere kámen z desky.
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        public void OdeberKamen(int radek, int sloupec)
        {
            deska[radek, sloupec] = Manazer.StavPole.volne;
        }


        /// <summary>
        /// Odebere kámen z desky, namísto něj vloží Hraci.volne.
        /// </summary>
        /// <param name="pole">Souřadnice pole</param>
        public void OdeberKamen(Pozice pole)
        {
            OdeberKamen(pole.Radek, pole.Sloupec);
        }

        
        /// <summary>
        /// Je daná pozice rohová (tj. mimo hrací desku?)
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns></returns>
        public static bool Rohova(int radek, int sloupec)
        {
            return (radek < 2 && sloupec < 2) || (radek < 2 && sloupec > 4) || 
                (radek > 4 && sloupec < 2) || (radek > 4 && sloupec > 4);
        }


        /// <summary>
        /// Nacházíme se v pevnosti?
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns></returns>
        public static bool Pevnost(int radek, int sloupec)
        {
            return radek >= 0 && radek < 3 && sloupec > 1 && sloupec < 5;
        }

        
        /// <summary>
        /// Nacházíme se vůbec na desce?
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns></returns>
        private bool NaDesce(int radek, int sloupec)
        {
            return radek >= 0 && sloupec >= 0 && radek < deska.GetLength(0) && radek < deska.GetLength(1);
        }


        /// <summary>
        /// Je dané políčko volné?
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns></returns>
        private bool Volne(int radek, int sloupec)
        {
            return deska[radek, sloupec] == Manazer.StavPole.volne;
        }


        /// <summary>
        /// Můžeme na dané políčko táhnout?
        /// </summary>
        /// <param name="radek">Souřadnice řádku</param>
        /// <param name="sloupec">Souřadnice sloupce</param>
        /// <returns></returns>
        public bool MuzemeTahnout(int radek, int sloupec)
        {
            return Volne(radek, sloupec) && NaDesce(radek, sloupec) && !Rohova(radek, sloupec);
        }


        /// <summary>
        /// Vyhledá na hrací desce obránce.
        /// </summary>
        /// <returns>Seznam dvou obránců.</returns>
        public List<Pozice> VratPoziceObrancu()
        {
            List<Pozice> obranci = new List<Pozice>();

            for(int i = 0; i < rozmer; i++)
                for(int j = 0; j < rozmer; j++)
                    if(deska[i, j] == Manazer.StavPole.obrana)
                        obranci.Add(new Pozice(i, j));
                
            return obranci;
        }

        public override string ToString()
        {
            //return VratPoziceObrancu()[0].ToString() + VratPoziceObrancu()[1].ToString();
            return GetHashCode().ToString();
        }

        /// <summary>
        /// Vrací všechny pozice daného hráče na hrací desce.
        /// </summary>
        /// <param name="barvaHrace"></param>
        /// <returns></returns>
        public List<Pozice> VratPoziceHrace(Manazer.StavPole barvaHrace)
        {
            List<Pozice> hraci = new List<Pozice>();

            for(int i = 0; i < rozmer; i++)
                for(int j = 0; j < rozmer; j++)
                    if(deska[i, j] == barvaHrace)
                        hraci.Add(new Pozice(i, j));

            return hraci;
        }

        /// <summary>
        /// Zkopíruje hrací desku.
        /// </summary>
        /// <returns>Kopie desky.</returns>
        public HraciDeska Copy()
        {
            HraciDeska novaDeska = new HraciDeska();

            for(int i = 0; i < rozmer; i++)
                for(int j = 0; j < rozmer; j++)
                    novaDeska.deska[i, j] = deska[i, j];

            novaDeska.aktualniTah = aktualniTah;

            return novaDeska;
        }


        /// <summary>
        /// Odstraní přeskočené kameny.
        /// </summary>
        /// <param name="tah">Validní tah.</param>
        public void OdstranPreskoceneKameny(Tah tah)
        {
            for(int i = 1; i < tah.PocetTahu(); i++)
            {
                if((Math.Abs(tah.seznamTahu[i - 1].Radek - tah.seznamTahu[i].Radek) == 2) ||
                    (Math.Abs(tah.seznamTahu[i - 1].Sloupec - tah.seznamTahu[i].Sloupec) == 2))
                {
                    OdeberKamen(Pozice.Stred(tah.seznamTahu[i - 1], tah.seznamTahu[i]));
                }
            }
        }


        /// <summary>
        /// Vrátí zpět kameny, které byly původně přeskočené
        /// </summary>
        /// <param name="tah"></param>
        public void PridejPreskoceneKameny(Tah tah)
        {
            for(int i = 1; i < tah.PocetTahu(); i++)
            {
                if((Math.Abs(tah.seznamTahu[i - 1].Radek - tah.seznamTahu[i].Radek) == 2) ||
                    (Math.Abs(tah.seznamTahu[i - 1].Sloupec - tah.seznamTahu[i].Sloupec) == 2))
                {
                    NastavKamen(Pozice.Stred(tah.seznamTahu[i - 1], tah.seznamTahu[i]),
                        Manazer.StavPole.utok);
                }
            }
        }
    }
}
