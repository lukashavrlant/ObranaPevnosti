using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    [Serializable]
    abstract public class Hrac
    {
        public Hrac(string jmeno)
        {
            this.Jmeno = jmeno;
        }

        protected bool UmelaInteligence;
        public bool umelaInteligence
        {
            get { return UmelaInteligence; }
        }

        /// <summary>
        /// Jméno/přezdívka hráče.
        /// </summary>
        public string Jmeno
        {
            set;
            get;
        }

        /// <summary>
        /// Slot pro ukládání aktuálního tahu hráče.
        /// </summary>
        public Tah AktualniTah
        {
            set;
            get;
        }

        /// <summary>
        /// Obecná metoda hráče pro vrácení aktuálního tahu.
        /// </summary>
        /// <returns></returns>
        public virtual Tah VratTah()
        {
            return AktualniTah;
        }
    }
}
