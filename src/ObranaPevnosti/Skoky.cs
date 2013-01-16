using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    class Skoky
    {
        public Skoky(Pozice odkud, List<Skoky> skoky)
        {
            this.odkud = odkud;
            this.skoky = skoky;
        }

        public Skoky() { }

        public Pozice odkud
        {
            get;
            set;
        }

        public List<Skoky> skoky
        {
            get;
            set;
        }

        public HraciDeska deska
        {
            get;
            set;
        }
    }
}
