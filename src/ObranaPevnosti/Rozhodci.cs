using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObranaPevnosti
{
    public static class Rozhodci
    {
        /// <summary>
        /// Vrací všechny platné tahy pro zadanou pozici a pro zadaného hráče.
        /// </summary>
        public static List<Tah> VratPlatneTahy(int radekOdkud, int sloupecOdkud, 
            Manazer.StavPole hrac, HraciDeska hraciDeska)
        {
            List<Tah> platneTahy = new List<Tah>();

            if(hraciDeska.Hrac(radekOdkud, sloupecOdkud) == hrac)
            {
                for(int i = 0; i < hraciDeska.rozmer; i++)
                {
                    for(int j = 0; j < hraciDeska.rozmer; j++)
                    {
                        if(ValidniTah(i, j, radekOdkud, sloupecOdkud, hrac, hraciDeska) &&
                            hraciDeska.MuzemeTahnout(i, j))
                        {
                            Tah tah = new Tah(new Pozice(radekOdkud, sloupecOdkud), new Pozice(i, j));

                            platneTahy.Add(tah);
                        }
                    }
                }
            }

            return platneTahy;    
        }

        /// <summary>
        /// Vrací všechny platné tahy pro zadanou pozici a pro zadaného hráče.
        /// </summary>
        public static List<Tah> VratPlatneTahy(Pozice pole, Manazer.StavPole hrac, HraciDeska hraciDeska)
        {
            if(hraciDeska.Hrac(pole) == hrac)
                return VratPlatneTahy(pole.Radek, pole.Sloupec, hrac, hraciDeska);

            return new List<Tah>();
        }

        /// <summary>
        /// Vrací všechny platné tahy daného hráče v tomto kole
        /// </summary>
        public static List<List<Tah>> VratVsechnyPlatneTahy(Manazer.StavPole hrac, HraciDeska hraciDeska)
        {
            List<List<Tah>> vsechnyPlatneTahy = new List<List<Tah>>();

            for(int i = 0; i < hraciDeska.rozmer; i++)
                for(int j = 0; j < hraciDeska.rozmer; j++)
                {
                    // Zjistíme všechny platné tahy z dané pozice...
                    List<Tah> tahy = VratPlatneTahy(i, j, hrac, hraciDeska);

                    // ...a pokud z této pozice můžeme alespoň někam táhnout, uložíme si tah
                    if(tahy.Count > 0)
                        vsechnyPlatneTahy.Add(tahy);
                }

            // Pokud obránce může skákat, zahodíme obyčejné tahy a zjistíme, 
            // jestli nemůže obránce skákat ještě dál (vícenásobný skok).
            if(MuzeObranceSkakat(vsechnyPlatneTahy))
            {
                List<Pozice> poziceObrancu = hraciDeska.VratPoziceObrancu();

                vsechnyPlatneTahy = OdstranObycejneTahy(vsechnyPlatneTahy);

                // Skoky prvního obránce
                Skoky skoky = VsechnySkoky(poziceObrancu[0], hraciDeska);

                // Skoky druhého obránce
                Skoky skoky2 = VsechnySkoky(poziceObrancu[1], hraciDeska);

                vsechnyPlatneTahy.Clear();

                // serializujeme skoky do listů
                List<Tah> platneTahy1 = Serializace(skoky);
                List<Tah> platneTahy2 = Serializace(skoky2);

                // Pokud může obránce alespoň jednou skočit, uložíme si skok
                if(platneTahy1.Count != 0)
                    vsechnyPlatneTahy.Add(platneTahy1);

                if(platneTahy2.Count != 0)
                    vsechnyPlatneTahy.Add(platneTahy2);
            }

            return vsechnyPlatneTahy;
        }


        /// <summary>
        /// Serializuje (linearizuje) strom skoků do seznamu (listu).
        /// </summary>
        private static List<Tah> Serializace(Skoky skok)
        {
            List<Tah> serializovaneSkoky = new List<Tah>();

            while(true)
            {
                try
                {
                    Tah t = SerializaceSkoku(skok);
                    serializovaneSkoky.Add(t);
                }
                catch
                {
                    break;
                }
            }

            return serializovaneSkoky;
        }

        /// <summary>
        /// Serializuje (linearizuje) jednu větev stromu do seznamu (listu).
        /// </summary>
        private static Tah SerializaceSkoku(Skoky skok)
        {
            Tah tah = new Tah();
            Skoky pomoc = null;
            List<Skoky> uloziste = new List<Skoky>();

            if(skok.skoky == null || skok.skoky.Count == 0)
                throw new Exception("Konec cyklu");
            
            while(true)
            {
                pomoc = skok;
                //tah.kam = skok.odkud;
                tah.seznamTahu.Add(skok.odkud);
                uloziste.Add(skok);

                if(skok.skoky != null && skok.skoky.Count > 0)
                    skok = skok.skoky[0];
                else
                    break;
            }

            if(uloziste.Count > 1)
            {
                int index = uloziste.Count - 2;

                if(uloziste[index].skoky.Count == 0)
                    uloziste[index].skoky = null;
                else 
                    uloziste[index].skoky.Remove(uloziste[index].skoky[0]);
            }

            /*if(pomoc.skoky != null)
                pomoc.skoky.Remove(pomoc.skoky[0]);
            else if(pomoc.skoky.Count == 0)
                pomoc.skoky = null;*/

            return tah;
        }

        /// <summary>
        /// Vrací všechny (i násobné) skoky jednoho obránce.
        /// </summary>
        private static Skoky VsechnySkoky(Pozice souradniceObrance, HraciDeska hraciDeska)
        {
            List<Tah> obrancovySkoky;

            Skoky skok = new Skoky();
            skok.odkud = souradniceObrance;
            skok.skoky = null;


            obrancovySkoky = KamMuzeObranceSkocit(souradniceObrance, hraciDeska);

            if(obrancovySkoky.Count != 0)
            {
                skok.skoky = new List<Skoky>();

                foreach(Tah tah in obrancovySkoky)
                {
                    HraciDeska novaHraciDeska;
                    novaHraciDeska = hraciDeska.Copy();
                    // novaHraciDeska = (HraciDeska) hraciDeska.Clone();
                    novaHraciDeska.Tahni(tah);
                    novaHraciDeska.OdstranPreskoceneKameny(tah);

                    Skoky novySkok = new Skoky(tah.seznamTahu[1], null);
                    novySkok.deska = novaHraciDeska;
                    skok.skoky.Add(novySkok);
                }

                HelpSkoky(skok);
            }

            return skok;
        }


        /// <summary>
        /// Pomocná funkce pro výpočet všech skoků jednoho obránce.
        /// </summary>
        private static Skoky HelpSkoky(Skoky skok)
        {
            List<Tah> obrancovySkoky;

            if(skok.skoky != null)
            {
                foreach(Skoky s in skok.skoky)
                {
                    obrancovySkoky = KamMuzeObranceSkocit(s.odkud, s.deska);

                    if(obrancovySkoky.Count != 0)
                    {
                        s.skoky = new List<Skoky>();

                        foreach(Tah tah in obrancovySkoky)
                        {
                            HraciDeska novaHraciDeska;
                            novaHraciDeska = s.deska.Copy();
                            // novaHraciDeska = (HraciDeska) s.deska.Clone();
                            novaHraciDeska.Tahni(tah);
                            novaHraciDeska.OdstranPreskoceneKameny(tah);

                            Skoky novySkok = new Skoky(tah.seznamTahu[1], null);
                            novySkok.deska = novaHraciDeska;
                            s.skoky.Add(novySkok);
                        }
                    }

                    HelpSkoky(s);
                }
            }

            return skok;
        }


        /// <summary>
        /// Vrací všechny skoky jedné úrovně jednoho obránce.
        /// </summary>
        private static List<Tah> KamMuzeObranceSkocit(Pozice souradniceObrance, HraciDeska hraciDeska)
        {
            List<Tah> tahy;

            tahy = VratPlatneTahy(souradniceObrance, Manazer.StavPole.obrana, hraciDeska);
            tahy = OdstranObycejneTahy(tahy);

            return tahy;
        }


        /// <summary>
        /// Zjišťuje, jestli se v platných tazích vyskytují obráncovy skoky.
        /// </summary>
        private static bool MuzeObranceSkakat(List<List<Tah>> platneTahy)
        {
            foreach(List<Tah> tahy in platneTahy)
            {
                foreach(Tah tah in tahy)
                {
                    for(int i = 1; i < tah.PocetTahu(); i++)
                    {
                        if((Math.Abs(tah.seznamTahu[i - 1].Radek - tah.seznamTahu[i].Radek) == 2) ||
                            (Math.Abs(tah.seznamTahu[i - 1].Sloupec - tah.seznamTahu[i].Sloupec) == 2))
                        {

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Odstraní obyčejné tahy, tedy ty, ve kterých se neskáče.
        /// </summary>
        private static List<List<Tah>> OdstranObycejneTahy(List<List<Tah>> platneTahy)
        {
            List<List<Tah>> noveTahyKolekce = new List<List<Tah>>();
            List<Tah> noveTahy;
            

            foreach(List<Tah> tahy in platneTahy)
            {
                noveTahy = new List<Tah>();

                tahy.ForEach(tah => { if (tah.SkakaloSe()) noveTahy.Add(tah); });

                if(noveTahy.Count != 0)
                    noveTahyKolekce.Add(noveTahy);
            }

            return noveTahyKolekce;
        }

        /// <summary>
        /// Odstraní obyčejné tahy, tedy ty, ve kterých se neskáče.
        /// </summary>
        private static List<Tah> OdstranObycejneTahy(List<Tah> platneTahy)
        {
            List<Tah> noveTahy = new List<Tah>();

            platneTahy.ForEach(tah =>  {if (tah.SkakaloSe()) noveTahy.Add(tah); });

            return noveTahy;
        }


        /// <summary>
        /// Zjistíme, jestli je daný tah validní.
        /// </summary>
        public static bool ValidniTah(int radekKam, int sloupecKam, int radekOdkud, int sloupecOdkud,
            Manazer.StavPole hrac, HraciDeska hraciDeska)
        {
            return DoBoku(radekKam, sloupecKam, radekOdkud, sloupecOdkud) ||
                Dopredu(radekKam, sloupecKam, radekOdkud, sloupecOdkud, hrac) ||
                Diagonalne(radekKam, sloupecKam, radekOdkud, sloupecOdkud, hrac) ||
                Skok(radekKam, sloupecKam, radekOdkud, sloupecOdkud, hrac, hraciDeska);
        }


        /// <summary>
        /// Jde kámen do boku?
        /// </summary>
        private static bool DoBoku(int radekKam, int sloupecKam, int radekOdkud, int sloupecOdkud)
        {
            return (radekKam == radekOdkud) && (Math.Abs(sloupecKam - sloupecOdkud) == 1);
        }

        /// <summary>
        /// Jde kámen dopředu/dozadu?
        /// </summary>
        private static bool Dopredu(int radekKam, int sloupecKam, int radekOdkud, int sloupecOdkud, Manazer.StavPole hrac)
        {
            int rozdil = radekKam - radekOdkud;

            if (hrac == Manazer.StavPole.obrana)
                rozdil = Math.Abs(rozdil);
            else
                rozdil *= -1;

            return (sloupecKam == sloupecOdkud) && (rozdil == 1);
        }

        /// <summary>
        /// Jde kámen diagonálně?
        /// </summary>
        private static bool Diagonalne(int radekKam, int sloupecKam, int radekOdkud, int sloupecOdkud, Manazer.StavPole hrac)
        {
            if(hrac == Manazer.StavPole.obrana)
            {
                if((ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 1, 2, 2, 1)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 2, 1, 1, 2)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 1, 4, 2, 5)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 2, 5, 1, 4)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 5, 4, 4, 5)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 4, 5, 5, 4)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 5, 2, 4, 1)) ||
                    (ZakazanyPohyb(radekKam, sloupecKam, radekOdkud, sloupecOdkud, 4, 1, 5, 2)))
                    return false;

                int rozdilRadku = Math.Abs(radekKam - radekOdkud);
                int rozdilSloupcu = Math.Abs(sloupecKam - sloupecOdkud);

                return rozdilRadku == rozdilSloupcu && rozdilSloupcu == 1;
            }

            return false;
        }


        /// <summary>
        /// Zjišťuje, jestli se obránce nepokouší provést nějaký specifický zakázaný tah.
        /// Například přechod přes roh hrací desky.
        /// </summary>
        private static bool ZakazanyPohyb(int radekKam, int sloupecKam, int radekOdkud,
            int sloupecOdkud, int radekOdkudZakaz, int sloupecOdkudZakaz, 
            int radekKamZakaz, int sloupecKamZakaz)
        {
            return (((radekOdkud == radekOdkudZakaz && sloupecOdkud == sloupecOdkudZakaz) &&
                (radekKam == radekKamZakaz && sloupecKam == sloupecKamZakaz)) ||
                ((radekOdkud == radekOdkudZakaz && sloupecOdkud == sloupecOdkudZakaz) &&
                (radekKam == radekKamZakaz && sloupecKam == sloupecOdkudZakaz)));

        }

        /// <summary>
        /// Skáče obránce horizontálně nebo vertikálně nebo diagonálně?
        /// </summary>
        private static bool Skok(int radekKam, int sloupecKam, int radekOdkud, int sloupecOdkud,
            Manazer.StavPole hrac, HraciDeska hraciDeska)
        {
            if(hrac == Manazer.StavPole.obrana)
            {
                Manazer.StavPole hracNaPozici;
                int rozdilRadku = Math.Abs(radekKam - radekOdkud);
                int rozdilSloupcu = Math.Abs(sloupecKam - sloupecOdkud);
                
                // Obecný skok
                if((sloupecKam == sloupecOdkud && rozdilRadku == 2) || 
                    (radekKam == radekOdkud && rozdilSloupcu == 2) ||
                    (rozdilRadku == rozdilSloupcu && rozdilSloupcu == 2))
                {
                    int preskokRadek = (radekOdkud + radekKam) / 2;
                    int preskokSloupec = (sloupecKam + sloupecOdkud) / 2;

                    // Skáče obránce přes roh hrací desky?
                    if((rozdilRadku == rozdilSloupcu && rozdilSloupcu == 2) &&
                        (((preskokRadek == 2) && (preskokSloupec == 1 || preskokSloupec == 5)) ||
                        ((preskokRadek == 4) && (preskokSloupec == 1 || preskokSloupec == 5)) ||
                        ((preskokRadek == 1) && (preskokSloupec == 2 || preskokSloupec == 4)) ||
                        ((preskokRadek == 5) && (preskokSloupec == 2 || preskokSloupec == 4))))
                        return false;

                    hracNaPozici = hraciDeska.Hrac(preskokRadek, preskokSloupec);

                    if(hracNaPozici == Manazer.StavPole.utok)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Zjišťuje, zda již někdo nevyhrál. Vrací buď hráče, který vyhrál nebo vrací Hraci.mimo.
        /// </summary>
        public static Manazer.MoznostiVyher KdoVyhral(HraciDeska hraciDeska)
        {
            if (hraciDeska.PocetUtocniku() < 9)
            {
                return Manazer.MoznostiVyher.obrana;
            }

            if (hraciDeska.ObsazenaPevnost())
            {
                return Manazer.MoznostiVyher.utok;
            }

            if(ZablokovanyHrac(Manazer.StavPole.obrana, hraciDeska))
            {
                return Manazer.MoznostiVyher.utok;
            }

            if(ZablokovanyHrac(Manazer.StavPole.utok, hraciDeska))
            {
                return Manazer.MoznostiVyher.obrana;
            }

            return Manazer.MoznostiVyher.nikdo;
        }


        /// <summary>
        /// Zablokovali jsme obránce?
        /// </summary>
        private static bool ZablokovanyObrance(HraciDeska hraciDeska)
        {
            List<Pozice> obranci;

            // obranci = hraciDeska.VratPoziceObrancu();
            obranci = hraciDeska.VratPoziceHrace(Manazer.StavPole.obrana);

            foreach(Pozice pole in obranci)
                if((VratPlatneTahy(pole, Manazer.StavPole.obrana, hraciDeska)).Count > 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Zablokovali jsme na hrací desce daného hráče?
        /// </summary>
        private static bool ZablokovanyHrac(Manazer.StavPole barvaHrace, HraciDeska hraciDeska)
        {
            List<Pozice> hraci;

            hraci = hraciDeska.VratPoziceHrace(barvaHrace);

            foreach(Pozice pole in hraci)
                if((VratPlatneTahy(pole, barvaHrace, hraciDeska)).Count > 0)
                    return false;

            return true;
        }
    }
}
