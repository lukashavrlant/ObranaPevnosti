using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ObranaPevnosti
{
    class ObranceMusiSkakat : Exception
    {
        public ObranceMusiSkakat() { }

        public ObranceMusiSkakat(string message)
            : base(message)
        { }
    }

    class NeplatnyTah : Exception
    {
        public NeplatnyTah() { }

        public NeplatnyTah(string message)
            : base(message)
        { }
    }

    [Serializable]
    public class Manazer
    {
        [NonSerialized]
        public static Action AktualizaceUI;

        [NonSerialized]
        public static Action AktualizaceHistorie;

        [NonSerialized]
        public static Action AkcePoKonciHry;

        // Výčet reprezentující možné stavy na desce
        public enum StavPole
        {
            mimo = -1,
            volne,
            utok,
            obrana
        }

        // Deska, na které se hra hraje
        public static HraciDeska Deska
        {
            get;
            set;
        }

        // Nastavili jsme již obránce?
        public bool NastaveniObranci
        {
            set;
            get;
        }

        // Počet půltahů hry
        public int PocetTahu
        {
            get;
            private set;
        }

        // Byla již hra ukončena?
        public bool KonecHry
        {
            get;
            private set;
        }

        // Kdo vyhrál?
        public MoznostiVyher Vitez
        {
            get;
            private set;
        }

        // zásobník uchovávající si zpětné tahy
        public Stack<Tah> TahyZpet
        {
            get;
            private set;
        }

        // zásobník uchovávající si tahy vpřed
        public Stack<Tah> TahyVpred
        {
            get;
            private set;
        }

        // Obtížnost počítače
        public Obtiznost ObtiznostHry
        {
            get;
            private set;
        }

        // Je na tahu počítačový hráč s umělou inteligencí?
        public bool JePocitacNaTahu
        {
            get { return VratHraceNaTahu().umelaInteligence; }
        }

        // Limit pro konec hry pro případ opakování tahů
        public const int LimitTahu = 300;

        // Pole obsahující dva hráče -- útočníka a obránce
        public Hrac[] Sokove;

        // Obtížnost počítače
        public enum Obtiznost
        {
            snadna = 2, normalni, tezka
        }

        // Stav hry aneb kdo vyhrál
        public enum MoznostiVyher
        {
            nikdo, utok, obrana, remiza
        }

        // Vrátí odkaz na obránce
        public Hrac Obrance
        {
            get
            { return Sokove[1]; }
            set
            { Sokove[1] = value; }
        }

        // Vrátí odkaz na útočníka
        public Hrac Utocnik
        {
            get
            { return Sokove[0]; }
            set
            { Sokove[0] = value; }
        }

        // hrají pouze počítače?
        public bool DvaPocitace
        {
            get
            { return Utocnik.umelaInteligence && Obrance.umelaInteligence; }
        }

        // Který hráč je zrovna na tahu?
        private Manazer.StavPole HracNaTahu = Manazer.StavPole.obrana;
        public Manazer.StavPole hracNaTahu
        {
            private set
            {
                if(value == Manazer.StavPole.obrana || value == Manazer.StavPole.utok)
                    HracNaTahu = value;
            }
            get
            {
                return HracNaTahu;
            }
        }

        public Manazer()
        {
            Vitez = MoznostiVyher.nikdo;
            KonecHry = true;
            ObtiznostHry = Obtiznost.normalni;
            Sokove = new Hrac[2];
        }


        /// <summary>
        /// Vrátí hru o jeden tah zpět.
        /// </summary>
        public Manazer TahZpet()
        {
            Tah zpetnyTah = TahyZpet.Peek();
            TahyVpred.Push(TahyZpet.Pop());

            if(zpetnyTah.PocetTahu() == 1)
            {
                Deska.VratZpatkyTah(zpetnyTah);
                NastaveniObranci = false;
                hracNaTahu = Manazer.StavPole.obrana;

                return this;
            }

            Deska.VratZpatkyTah(zpetnyTah);
            Deska.PridejPreskoceneKameny(zpetnyTah);

            PrehodHraceNaTahu();

            KonecHry = false;
            Vitez = MoznostiVyher.nikdo;

            return this;
        }

        /// <summary>
        /// Vrátí hru o jeden tah vpřed.
        /// </summary>
        public Manazer TahVpred()
        {
            TahyZpet.Push(TahyVpred.Pop());
            Tah tahVpred = TahyZpet.Peek();

            if(tahVpred.PocetTahu() == 1)
            {
                Deska.NastavObrance(tahVpred.seznamTahu[0]);

                if(Deska.PocetObrancu == 2)
                {
                    NastaveniObranci = true;
                    hracNaTahu = Manazer.StavPole.utok;
                }

                return this;
            }

            tahVpred.seznamTahu.Reverse();

            Deska.Tahni(tahVpred);
            Deska.OdstranPreskoceneKameny(tahVpred);

            PrehodHraceNaTahu();

            ZjistiViteze();

            return this;
        }

        /// <summary>
        /// Přehodí hráče na tahu.
        /// </summary>
        public void PrehodHraceNaTahu()
        {
            if(hracNaTahu == Manazer.StavPole.obrana)
                hracNaTahu = Manazer.StavPole.utok;
            else
                hracNaTahu = Manazer.StavPole.obrana;

            PocetTahu++;
        }


        /// <summary>
        /// Metoda přehazující hráče ("otočí hrací desku").
        /// </summary>
        /// <returns>Odkaz na manažera.</returns>
        public Manazer PrehodHrace()
        {
            Hrac pomoc;

            pomoc = Utocnik;
            Utocnik = Obrance;
            Obrance = pomoc;

            foreach(Hrac hrac in Sokove)
                if (hrac.umelaInteligence)
                    ((AI)hrac).ZmenBarvu();

            return this;
        }


        /// <summary>
        /// Vrátí konkrétní tah hráče, který je uložen v patřičném slotu. 
        /// Do tohoto slotu je tah uložen buď z konzole nebo ze souboru nebo
        /// se o vhodný tah postará minimax.
        /// </summary>
        /// <returns>Vrací patřičně vyplněnou instanci třídy Tah.</returns>
        public Tah VratTahHrace()
        {
            return VratHraceNaTahu().VratTah();
        }


        /// <summary>
        /// Metoda vrací přímý odkaz na hráče, který je zrovna na tahu.
        /// </summary>
        /// <returns>Objekt Hrac</returns>
        public Hrac VratHraceNaTahu()
        {
            return Sokove[((int) hracNaTahu) - 1];
        }

        /// <summary>
        /// Provede tah hráče.
        /// </summary>
        public void Tahni()
        {
            if(!KonecHry)
            {
                if(JePocitacNaTahu)
                {
                    Minimax.AkcePoNalezeniTahu = Tahni;
                    Minimax.NejlepsiTah(Deska, hracNaTahu, (int) ObtiznostHry);
                    return;
                }
                else
                {
                    Tahni(VratHraceNaTahu().VratTah());
                }
            }

            // Tohle je nové a psáno ve dvě ráno
            // A díky tomuhle se to samo nepřekresluje :-(
            if(JePocitacNaTahu && !KonecHry)
            {
                AktualizaceUI();
                Tahni();
            }
        }

        /// <summary>
        /// Funkce si od rozhodčího vyžádá všechny platné tahy z dané pozici 
        /// a pokud se jeden z nich shoduje s předaných tahem, provede tah na desku.
        /// </summary>
        /// <param name="hracuvTah">Objekt Tah, který obsahuje odkud a kam táhnout.</param>
        public void Tahni(Tah hracuvTah)
        {
            List<List<Tah>> platneTahy;

            platneTahy = Rozhodci.VratVsechnyPlatneTahy(hracNaTahu, Deska);

            foreach(List<Tah> tahy in platneTahy)
            {
                foreach(Tah tah in tahy)
                {
                    if(tah.stejne(hracuvTah))
                    {
                        Deska.Tahni(hracuvTah);
                        Deska.OdstranPreskoceneKameny(hracuvTah);

                        // Uložíme si aktuální tah do záznamu celé hry
                        TahyZpet.Push((Tah)hracuvTah.Clone());

                        // Vynulujeme zásobník s redo tahy
                        TahyVpred.Clear();

                        // Přehodíme hráče na tahu
                        PrehodHraceNaTahu();

                        // Zjistíme jestli někdo nevyhrál
                        ZjistiViteze();

                        try
                        {
                            AktualizaceUI();
                            AktualizaceHistorie();
                        }
                        catch(Exception)
                        { }

                        if (VitezNeboLimit())
                            AkcePoKonciHry();

                        return;
                    }
                }
            }

            if(platneTahy[0][0].SkakaloSe())
                throw new ObranceMusiSkakat("Obránce musí skákat");

            throw new NeplatnyTah("Neplatný tah!");
        }

        /// <summary>
        /// Funkce si od rozhodčího vyžádá všechny platné tahy z dané pozici 
        /// a pokud se jeden z nich shoduje s předaných tahem, provede tah na desku.
        /// </summary>
        /// <param name="odkud">Stringová souřadnice odkud se má táhnout (např. e4).</param>
        /// <param name="kam">Stringová souřadnice kam se má táhnout (např. e3).</param>
        public void Tahni(string odkud, string kam)
        {
            Pozice odkudPole, kamPole;

            odkudPole = Prikazy.StringNaSouradnice(odkud);
            kamPole = Prikazy.StringNaSouradnice(kam);

            Tahni(new Tah(odkudPole, kamPole));
        }

        /// <summary>
        /// Funce pro táhnutí počítače
        /// </summary>
        public void TahPocitace()
        {
            Minimax.AkcePoNalezeniTahu = FinalniTah;
            Minimax.NejlepsiTah(Deska, hracNaTahu, (int) ObtiznostHry);
        }

        private void FinalniTah(Tah tah)
        {
            TahPocitace(tah);
            Minimax.vyslednyTah = null;

            AktualizaceUI();
            AktualizaceHistorie();

            if (!KonecHry && PocetTahu < LimitTahu)
            {
                TahPocitace();
            }
            else
            {
                AkcePoKonciHry();
            }
        }

        private void TahPocitace(Tah tah)
        {
            Deska.Tahni(tah);
            Deska.OdstranPreskoceneKameny(tah);

            // Uložíme si aktuální tah do záznamu celé hry
            TahyZpet.Push((Tah) tah.Clone());

            // Vynulujeme zásobník s redo tahy
            TahyVpred.Clear();

            PrehodHraceNaTahu();

            if(PocetTahu >= LimitTahu)
            {
                KonecHry = true;
                Vitez = MoznostiVyher.remiza;
            }
            else
            {
                Vitez = Rozhodci.KdoVyhral(Deska);
                if(Vitez != MoznostiVyher.nikdo)
                {
                    KonecHry = true;
                }
            }
        }


        /// <summary>
        /// Nastaví obránce na hrací desku.
        /// </summary>
        /// <param name="s1">První obránce</param>
        /// <param name="s2">Druhý obránce</param>
        /*public void NastavObrance(string s1, string s2)
        {
            NastavObrance(Prikazy.StringNaSouradnice(s1));
            NastavObrance(Prikazy.StringNaSouradnice(s2));
        }*/

        /// <summary>
        /// Nastaví obránce po jednom.
        /// </summary>
        /// <param name="Souradnice">Stringové souřadnice</param>
        public void NastavObrance(Pozice Souradnice)
        {
            if(!NastaveniObranci)
            {
                Deska.NastavObrance(Souradnice);
                Tah tah = new Tah(Souradnice);
                TahyZpet.Push(tah);
            }

            if (Deska.PocetObrancu == 2)
            {
                NastaveniObranci = true;
                hracNaTahu = Manazer.StavPole.utok;
            }
        }

        /// <summary>
        /// Prostě restartuje hru.
        /// </summary>
        public void Restart()
        {
            Deska = new HraciDeska();
            NastaveniObranci = false;
            hracNaTahu = Manazer.StavPole.obrana;
            Deska.Restart();
            this.KonecHry = false;
            PocetTahu = 0;
            Vitez = MoznostiVyher.nikdo;

            TahyZpet = new Stack<Tah>();
            TahyVpred = new Stack<Tah>();

            if(Obrance.umelaInteligence)
            {
                NastavObrance();
            }
        }

        /// <summary>
        /// Automaticky nastaví obránce.
        /// </summary>
        public void NastavObrance()
        {
            String[] Souradnice = ((AI) Obrance).VratSouradniceObrancu();
            NastavObrance(Prikazy.StringNaSouradnice(Souradnice[0]));
            NastavObrance(Prikazy.StringNaSouradnice(Souradnice[1]));
            NastaveniObranci = true;
        }


        /// <summary>
        /// Nastaví obtížnost počítačovým hráčům
        /// </summary>
        public Manazer NastavObtiznost(Obtiznost Obtiznost)
        {
            foreach(Hrac hrac in Sokove)
                if (hrac is AI)
                    ((AI)hrac).obtiznost = (int)Obtiznost;

            ObtiznostHry = Obtiznost;

            return this;
        }

        /// <summary>
        /// Nastaví obtížnost počítačovým hráčům z již uložené obtížnosti
        /// </summary>
        public Manazer NastavObtiznost()
        {
            foreach(Hrac hrac in Sokove)
                if (hrac is AI)
                    ((AI)hrac).obtiznost = (int)ObtiznostHry;

            return this;
        }

        public bool MuzemeTahnoutVpred()
        {
            return TahyVpred.Count > 0 && !KonecHry;
        }

        public bool MuzemeTahnoutZpet()
        {
            return TahyZpet.Count > 0;
        }

        public bool VitezNeboLimit()
        {
            return (Vitez != MoznostiVyher.nikdo) || (PocetTahu == LimitTahu);
        }

        public void ZjistiViteze()
        {
            if (PocetTahu >= LimitTahu)
            {
                KonecHry = true;
                Vitez = MoznostiVyher.remiza;
            }
            else
            {
                Vitez = Rozhodci.KdoVyhral(Deska);
                if (Vitez != MoznostiVyher.nikdo)
                {
                    KonecHry = true;
                }
            }
        }
    }
}
