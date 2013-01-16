using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObranaPevnosti
{
    public static class Minimax
    {
        public static Tah vyslednyTah = null;
        public static Action<Tah> AkcePoNalezeniTahu;
        public static Thread vlakno;

        /*public static Tah VratNejlepsiTah(HraciDeska hraciDeska, Manazer.StavPole hrac, int hloubka)
        {
            GenerujVsechnyTahy(hraciDeska, hrac, hloubka, true);
            return vyslednyTah;
        }*/

        public static void NejlepsiTah(HraciDeska hraciDeska, Manazer.StavPole hrac, int hloubka)
        {
            vlakno = new Thread(() => { GenerujVsechnyTahy(hraciDeska, hrac, hloubka, true);
            AkcePoNalezeniTahu(vyslednyTah);
            });
            vlakno.Start();
        }

        /// <summary>
        /// Vygeneruje všechny možné situace hry do zadané hloubky a následně do
        /// slotu "vyslednyTah" uloží nejlepší pozici.
        /// </summary>
        private static int GenerujVsechnyTahy(HraciDeska hraciDeska, Manazer.StavPole hrac, 
            int hloubka, bool prvniFunkce)
        {
            if(hloubka == 0)
                return Ohodnoceni.Ohodnot(hraciDeska);

            List<List<Tah>> validniTahy = Rozhodci.VratVsechnyPlatneTahy(hrac, hraciDeska);
            List<int> ohodnoceni = new List<int>();
            Manazer.StavPole dalsiHrac;

            if(hrac == Manazer.StavPole.obrana)
                dalsiHrac = Manazer.StavPole.utok;
            else
                dalsiHrac = Manazer.StavPole.obrana;

            for(int i = 0; i < validniTahy.Count; i++)
            {
                for(int j = 0; j < validniTahy[i].Count; j++)
                {
                    HraciDeska novaDeska = hraciDeska.Copy();
                    // HraciDeska novaDeska = (HraciDeska) hraciDeska.Clone();
                    novaDeska.Tahni(validniTahy[i][j]);
                    novaDeska.OdstranPreskoceneKameny(validniTahy[i][j]);
                    ohodnoceni.Add(GenerujVsechnyTahy(novaDeska, dalsiHrac, hloubka - 1, false));
                }
            }

            int pomoc;

            if(hrac == Manazer.StavPole.utok)
            {
                pomoc = Int32.MinValue;
            }
            else
            {
                pomoc = Int32.MaxValue;
            }


            foreach(int cislo in ohodnoceni)
            {
                if(hrac == Manazer.StavPole.utok)
                {
                    if(cislo > pomoc)
                        pomoc = cislo;
                }
                else
                {
                    if(cislo < pomoc)
                        pomoc = cislo;
                }
            }

            if(prvniFunkce)
            {
                int index = ohodnoceni.IndexOf(pomoc);
                int i, j, citac;

                citac = 0;

                for(i = 0; i < validniTahy.Count; i++)
                {
                    for(j = 0; j < validniTahy[i].Count; j++)
                    {
                        if(citac++ == index)
                            vyslednyTah = validniTahy[i][j];
                    }
                }

                return 0;
            }

            return pomoc;
        }
    }
}
