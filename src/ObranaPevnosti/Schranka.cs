using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    class Schranka
    {
        public Manazer ManazerHry
        {
            get;
            set;
        }

        public HraciDeska AktualniHraciDeska
        {
            get;
            set;
        }

        public Schranka(Manazer ManazerHry, HraciDeska AktualniHraciDeska)
        {
            this.AktualniHraciDeska = AktualniHraciDeska;
            this.ManazerHry = ManazerHry;
        }
    }
}
