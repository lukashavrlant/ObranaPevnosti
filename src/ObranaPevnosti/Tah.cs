using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    public class Tah : ICloneable
    {
        public List<Pozice> seznamTahu = new List<Pozice>();

        public Tah() { }

        public Tah(params Pozice[] VsechnyPozice)
        {
            seznamTahu = VsechnyPozice.ToList<Pozice>();
        }

        public Tah(List<Pozice> VsechnyPozice)
        {
            seznamTahu = VsechnyPozice;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < seznamTahu.Count; i++)
            {
                if(i < (seznamTahu.Count - 1))
                    sb.Append(String.Format("{0} -> ", seznamTahu[i].ToString()));
                else
                    sb.Append(String.Format("{0}", seznamTahu[i].ToString()));
            }

            return sb.ToString();
        }

        public int PocetTahu()
        {
            return seznamTahu.Count;
        }

        public bool stejne(Tah tah)
        {
            if(tah.seznamTahu.Count != seznamTahu.Count)
                return false;


            for(int i = 0; i < seznamTahu.Count; i++)
                if(!(seznamTahu[i].Equals(tah.seznamTahu[i])))
                    return false;

            return true;
        }

        public bool SkakaloSe()
        {
            if(seznamTahu.Count > 2)
                return true;
            else if((Math.Abs(seznamTahu[0].Radek - seznamTahu[1].Radek) == 2) ||
                    (Math.Abs(seznamTahu[0].Sloupec - seznamTahu[1].Sloupec) == 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Object Clone()
        {
            Tah NovyTah = new Tah();

            foreach(Pozice pozice in seznamTahu)
                NovyTah.seznamTahu.Add((Pozice) pozice.Clone());

            return NovyTah;
        }
    }
}
