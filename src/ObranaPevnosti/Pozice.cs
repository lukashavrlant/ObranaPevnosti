using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    public class Pozice : ICloneable
    {
        public Pozice(int radek, int sloupec)
        {
            this.Radek = radek;
            this.Sloupec = sloupec;
        }

        public readonly int Radek;
        public readonly int Sloupec;

        public override bool Equals(Object pole)
        {
            if (pole is Pozice)
                return ((Pozice)pole).Radek == Radek && ((Pozice)pole).Sloupec == Sloupec;
            else
                return base.Equals(pole);
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public static bool operator == (Pozice pozice1, Pozice pozice2)
        {
            if(Object.ReferenceEquals(pozice1, pozice2))
                return true;

            if(((object) pozice1 == null) || ((object) pozice2 == null))
            {
                return false;
            }

            return pozice1.Equals(pozice2);
        }

        public static bool operator !=(Pozice pozice1, Pozice pozice2)
        {
            return !(pozice1 == pozice2);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "[" + Enum.GetName(typeof(Prikazy.Souradnice), Sloupec ) + ", " + (Radek + 1) +"]";
        }

        public static Pozice Stred(Pozice pole1, Pozice pole2)
        {
            return new Pozice((pole1.Radek + pole2.Radek) / 2, (pole1.Sloupec + pole2.Sloupec) / 2);
        }
    }
}
