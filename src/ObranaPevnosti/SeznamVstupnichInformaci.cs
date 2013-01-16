using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    public struct SeznamVstupnichInformaci
    {
        public string JmenoUtocnika
        { get; set; }

        public string JmenoObrance
        { get; set; }

        public bool JeUtocnikPocitacovyHrac
        { get; set; }
        public bool JeObrancePocitacovyHrac
        { get; set; }
    }
}
