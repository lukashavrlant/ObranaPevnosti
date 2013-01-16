using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    class AI : Hrac
    {
        private Manazer.StavPole BarvaHrace;

        private int Obtiznost;
        public int obtiznost
        {
            get { return Obtiznost; }
            set { if(value > 0) Obtiznost = value; }
        }

        public AI(string jmeno, Manazer.StavPole barvaHrace, int Obtiznost)
            : base(jmeno)
        {
            // inicializace prvků
            this.BarvaHrace = barvaHrace;

            // nastavení umělého hráče
            this.UmelaInteligence = true;
            this.obtiznost = Obtiznost;
        }

        /// <summary>
        /// Přepsaná metoda, která vrátí nejlepší vygenerovaný tah minimaxu.
        /// </summary>
        /// <returns></returns>
        /*public override Tah VratTah()
        {
            return Minimax.VratNejlepsiTah(Manazer.Deska, BarvaHrace, obtiznost);
            // return Minimax.NejlepsiTah(Manazer.Deska, BarvaHrace, obtiznost);
        }*/


        /// <summary>
        /// Změní barvu hráče na opačnou.
        /// </summary>
        public void ZmenBarvu()
        {
            if (BarvaHrace == Manazer.StavPole.obrana)
                BarvaHrace = Manazer.StavPole.utok;
            else
                BarvaHrace = Manazer.StavPole.obrana;
        }


        /// <summary>
        /// Náhodně rozestaví obránce do pevnosti.
        /// </summary>
        public String[] VratSouradniceObrancu()
        {
            List<string> seznamObrancu = new List<string>() { "c1", "c2", "c3", "d1", "d2", "d3", "e1", "e2", "e3"};
            Random rand = new Random();

            string prvniObrance = seznamObrancu[rand.Next(seznamObrancu.Count)];
            seznamObrancu.Remove(prvniObrance);
            string druhyObrance = seznamObrancu[rand.Next(seznamObrancu.Count)];

            String[] SeznamPozic = new String[2] { prvniObrance, druhyObrance };
            return SeznamPozic;
        }
    }
}
