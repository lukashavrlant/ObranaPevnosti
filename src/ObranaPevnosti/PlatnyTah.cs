using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    class PlatnyTah
    {
        public Pole Kam
        {
            get;
            set;
        }

        public Pole Preskoceno
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(Prikazy.Souradnice), Kam.sloupec) + (Kam.radek + 1);
        }
    }
}
