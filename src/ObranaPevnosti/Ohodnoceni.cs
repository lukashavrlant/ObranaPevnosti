using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    public static class Ohodnoceni
    {
        /// <summary>
        /// Ohodonotí danou pozici na hrací desce z pohledu útočníka.
        /// Tedy čím více bodů, tím lépe pro útočníka (a naopak pro obránce).
        /// </summary>
        /// <returns>Ohodnocení pozice vyjádřené v celých bodech (int).</returns>
        public static int Ohodnot(HraciDeska hraciDeska)
        {
            int stupen;
            Random rand = new Random();

            // Za každého živého útočníku +100 bodů
            stupen = hraciDeska.PocetUtocniku() * 100;

            int PocetUtocnikuVPevnosti = 0;

            for(int i = 0; i < hraciDeska.rozmer; i++)
            {
                for(int j = 0; j < hraciDeska.rozmer; j++)
                {
                    if(hraciDeska.Hrac(i, j) == Manazer.StavPole.utok)
                    {
                        

                        if(HraciDeska.Pevnost(i, j))
                        {
                            // Pokud jsou všichni útočníci v pevnosti, jistě je to nejlepší tah
                            if (++PocetUtocnikuVPevnosti == 9)
                            {
                                return Int16.MaxValue;
                            }

                            // Za každého panáčka v horním řádku pevnosti +65 bodů
                            if(i == 0)
                                stupen += 65;

                            // Za každého panáčka na levém či pravém boku pevnosti +55 bodů
                            if((j == 2 || j == 4) && (i != 2))
                                stupen += 55;

                            // Za každého panáčka v penosti +20 bodů
                            stupen += 35;
                        }

                        // Za každý pohyb o pole dopředu +10 bodů
                        stupen += ((7 - i) * 10);

                        // Přidáme náhodné číslo od 0 do 9, aby počítač netáhl vždy stejně
                        stupen += rand.Next(10);
                    }
                    else if(hraciDeska.Hrac(i, j) == Manazer.StavPole.obrana)
                    {
                        // Čím dál je obránce od pevnosti, tím lépe
                        stupen += 25 * i;
                    }
                }
            }

            return stupen;
        }
    }
}
