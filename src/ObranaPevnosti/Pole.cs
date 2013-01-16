using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    public class Pole
    {
        public Pole(int radek, int sloupec)
        {
            this.Radek = radek;
            this.Sloupec = sloupec;
        }

        private int Radek;
        private int Sloupec;

        public bool stejne(Pole pole)
        {
            return pole.Radek == radek && pole.Sloupec == sloupec;
        }

        public int radek
        {
            set
            {
                if(value >= 0 && value <= 7)
                {
                    Radek = value;
                }
                else
                {
                    throw new Exception("Neplatný řádek");
                }
            }
            get
            {
                return Radek;
            }
        }

        public int sloupec
        {
            set
            {
                if(value >= 0 && value <= 7)
                {
                    Sloupec = value;
                }
                else
                {
                    throw new Exception("Neplatný řádek");
                }
            }
            get
            {
                return Sloupec;
            }
        }

        public override string ToString()
        {
            return "[" + Enum.GetName(typeof(Prikazy.Souradnice), sloupec ) + ", " + (radek + 1) +"]";
        }

        public Pole stred(Pole pole)
        {
            return new Pole((pole.Radek + radek) / 2, (pole.Sloupec + sloupec) / 2);
        }
    }
}
