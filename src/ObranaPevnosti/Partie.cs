using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ObranaPevnosti
{
    static class Partie
    {
        /// <summary>
        /// Uloží hru do binárního souboru.
        /// </summary>
        /// <param name="CilovySoubor">Soubor, do kterého bude hra ulžoena.</param>
        public static void UlozHru(string CilovySoubor, Manazer ManazerHry)
        {
            Schranka CelaHra = new Schranka(ManazerHry, Manazer.Deska);
            BinaryFormatter binFirmat = new BinaryFormatter();
            Stream fStrem = new FileStream(CilovySoubor, FileMode.Create,
                FileAccess.Write, FileShare.None);
            binFirmat.Serialize(fStrem, CelaHra);
            fStrem.Close();            
        }


        /// <summary>
        /// Načte z binárního souboru hru.
        /// </summary>
        /// <param name="ZdrojovySoubor">Cesta k uložené hře</param>
        /// <returns>Nový manažer hry</returns>
        public static Schranka NahrajHru(string ZdrojovySoubor)
        {
            BinaryFormatter binFirmat = new BinaryFormatter();
            Stream fStrem = File.OpenRead(ZdrojovySoubor);
            Schranka WTF = (Schranka)binFirmat.Deserialize(fStrem);
            fStrem.Close();
            return WTF;
        }
    }
}
