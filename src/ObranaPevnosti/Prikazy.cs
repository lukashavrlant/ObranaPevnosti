using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    public static class Prikazy
    {
        // Příkazy použité v konzolové části
        public const string exit = "exit";
        public const string tisk = "tisk";
        public const string tah = "tah";
        public const string start = "start";
        public const string restart = "restart";
        public const string nacti = "nacti";
        public const string obrance = "obr";
        public const string auto = "auto";
        public const string vymen = "switch";
        public const string help = "help";
        public const string undo = "undo";
        public const string redo = "redo";
        public const string save = "save";
        public const string load = "load";

        // Příkazy použité při vyhodnocení výhry
        public const string nikdo = "nikdo";

        // Výčet reprezentující souřadnice hrací desky
        public enum Souradnice
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g
        }

        /// <summary>
        /// Statická funkce převádějící stringové souřadnice do objektu Pozice
        /// </summary>
        public static Pozice StringNaSouradnice(string souradnice)
        {
            Pozice pole;
            int radek, sloupec;

            radek = Int32.Parse(souradnice.Remove(0, 1)) - 1;
            sloupec = (int)Enum.Parse(typeof(Prikazy.Souradnice), souradnice.Remove(1));

            pole = new Pozice(radek, sloupec);
            return pole;
        }

    }
}
