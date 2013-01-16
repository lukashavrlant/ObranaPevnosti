using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/*
namespace ObranaPevnosti
{
    public class Konzole
    {
        Manazer manazer = new Manazer();
        public bool automatickaHraPocitace
        {
            get;
            set;
        }

        public Konzole()
        {
            // Má počítač hrát automaticky pokud je na tahu nebo má čekat na příkaz "tah" z vnějška?
            automatickaHraPocitace = true;

            spustKonzoli();
        }


        /// <summary>
        /// Spustí a obsluhuje konzolovou část hry.
        /// </summary>
        private void spustKonzoli()
        {
            Console.Title = "Obrana pevnosti";
            string prikazy = string.Empty;
            string[] rozdelenePrikazy;

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("--- Vítejte ve hře Obrana pevnosti ---");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("\nStručný seznam příkazů:");


            vytiskniHelp("start <utok> <obrana>",
                "Spustí hru, pro počítačového hráče zadejte jako jméno 'AI'.\n\tPokud argument není 'AI', bude brán jako jméno hráče.");
            vytiskniHelp("restart <utok> <obrana>",
                "Restartuje hru, bez argumentů ponechá současný stav.");
            vytiskniHelp("obr pozice<1> pozice<2>",
                "Umístí obránce na zadané pozice.\n\tPozice zadávejte ve tvaru: 'a3', 'd5', ...");
            vytiskniHelp("tah poz<1> poz<2> ... poz<n>",
                "Provede tah po naznačených souřadnicích. \n\tVíce souřadnic značí vícenásobný skok u obránce.");
            vytiskniHelp("tisk", "Vytiskne aktuální hrací desku.");
            vytiskniHelp("exit", "Ukončí program.");
            vytiskniHelp("auto", "Přepíná mezi automatickým tahem počítače a ručním tahem.\n\tPokud je vypnuto, počítač čeká na příkaz 'tah'.");


            while(prikazy != Prikazy.exit)
            {
                if(manazer.KonecHry)
                {
                    // Pokud už někdo vyhrál, oznámíme to 
                    if(manazer.Vitez != Prikazy.nikdo)
                    {
                        Console.WriteLine("Kdo vyhrál: {0}. Počet tahů: {1}.", manazer.Vitez, manazer.PocetTahu.ToString());
                    }
                }
                else
                {
                    if(manazer.NastavenObrance)
                    {
                        if(automatickaHraPocitace && manazer.VratHraceNaTahu().umelaInteligence)
                        {
                            Console.WriteLine("Na tahu je hráč {0}", manazer.VratHraceNaTahu().Jmeno);
                            manazer.Tahni();
                            manazer.Deska.Tisk();
                            continue;
                        }

                        Console.WriteLine("Na tahu je hráč {0}", manazer.VratHraceNaTahu().Jmeno);
                    }

                    // Jestliže nebyl nastavený obránce, nic nebudeme provádět, pouze upozorníme uživatele,
                    // že zapomněl nastavit obránce.
                    else
                    {
                        manazer.Deska.Tisk();
                        Console.WriteLine("Nastavte obránce.\n");
                    }
                }

                Console.Write("> ");
                prikazy = Console.ReadLine();
                rozdelenePrikazy = prikazy.Split(' ');

                try
                {
                    switch(rozdelenePrikazy[0])
                    {
                        // Tisk hrací desky
                        case Prikazy.tisk:
                            manazer.Deska.Tisk();
                            break;

                        // ULožení hry
                        case Prikazy.save:
                            Partie.UlozHru("save");
                            break;

                        // Nahrání hry
                        case Prikazy.load:
                            manazer = Partie.NahrajHru("save");
                            break;

                        // Vrátí hru o jeden tah zpět
                        case Prikazy.undo:
                            manazer.TahZpet();
                            break;

                        // Posune hru o tah dopředu, je-li to možné
                        case Prikazy.redo:
                            manazer.TahVpred();
                            break;

                        // Prohodí hráče
                        case Prikazy.vymen:
                            manazer.PrehodHrace();
                            break;

                        // Vrátí nápovědu nejlepšího možného tahu
                        case Prikazy.help:
                            Console.WriteLine("Nejlepší tah: {0}", manazer.NapovedaNejlepsihoTahu());
                            break;

                        // Táhnutí kamenem
                        case Prikazy.tah:
                            if(manazer.NastavenObrance)
                            {
                                if(!manazer.VratHraceNaTahu().umelaInteligence)
                                {
                                    try
                                    {
                                        manazer.VratHraceNaTahu().AktualniTah = new Tah(rozdelenePrikazy);
                                    }
                                    catch
                                    {
                                        throw new Exception("Zadané souřadnice jsou velice podivné");
                                    }
                                }

                                manazer.Tahni();
                                manazer.Deska.Tisk();
                            }
                            else
                            {
                                Console.WriteLine("Musíte nejprve nastavit obránce!");
                            }
                            break;


                        // (Re)start celé hry
                        case Prikazy.start:
                        case Prikazy.restart:
                            if(rozdelenePrikazy.Length == 3)
                            {
                                Manazer.Hraci barvaHrac;

                                for(int i = 1; i <= 2; i++)
                                {
                                    if(i == 1)
                                        barvaHrac = Manazer.Hraci.utok;
                                    else
                                        barvaHrac = Manazer.Hraci.obrana;

                                    if(rozdelenePrikazy[i] == "AI")
                                    {
                                        manazer.Hraci[i - 1] = new AI("AI — " + barvaHrac.ToString(),
                                                barvaHrac, manazer.Deska, 3);
                                    }
                                    else
                                    {
                                        manazer.Hraci[i - 1] = new LidskyHrac(rozdelenePrikazy[i]);
                                    }
                                }
                            }

                            manazer.Restart();
                            break;


                        // Nastavení obránce
                        case Prikazy.obrance:
                            if(!manazer.NastavenObrance)
                            {
                                try
                                {
                                    manazer.NastavObrance(rozdelenePrikazy[1], rozdelenePrikazy[2]);
                                }
                                catch
                                {
                                    throw new Exception("Zadané souřadnice jsou velice podivné");
                                }

                                manazer.Deska.Tisk();
                            }
                            break;


                        // Načtení tahů z txt souboru
                        case Prikazy.nacti:
                            nactiHruZeSouboru(rozdelenePrikazy[1]);
                            break;

                        // Zapnutí a vypnutí automatické hry počítače
                        case Prikazy.auto:
                            automatickaHraPocitace = !automatickaHraPocitace;
                            Console.WriteLine("Automatická hra počítače: {0}", automatickaHraPocitace);
                            break;
                        case Prikazy.exit:
                            break;
                        default:
                            Console.WriteLine("Neznámý příkaz.");
                            break;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Vytiskne nápovědů k příkazům.
        /// </summary>
        /// <param name="prikaz">Příkaz</param>
        /// <param name="help">Vysvětlení příkazu.</param>
        private void vytiskniHelp(string prikaz, string help)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n{0}", prikaz);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t{0}", help);
        }

        /// <summary>
        /// Načte hru z textového souboru a provede patřičné příkazy.
        /// (Metoda prozatím rozpoznává pouze příkazy „tah“ a „obr“.)
        /// </summary>
        /// <param name="Cesta">Relativní cesta k umístění souboru</param>
        private void nactiHruZeSouboru(string Cesta)
        {
            manazer.Restart();

            StreamReader soubor = new StreamReader(Cesta);

            string[] prikazy;

            while(soubor.Peek() >= 0 && !manazer.KonecHry)
            {
                prikazy = soubor.ReadLine().Split(' ');

                switch(prikazy[0])
                {
                    case Prikazy.tah:
                        if(manazer.NastavenObrance)
                        {
                            manazer.VratHraceNaTahu().AktualniTah = new Tah(prikazy);
                            manazer.Tahni();
                        }
                        break;
                    case Prikazy.obrance:
                        try
                        {
                            manazer.NastavObrance(prikazy[1], prikazy[2]);
                            manazer.NastavenObrance = true;
                        }
                        catch
                        { }

                        break;
                }

                Console.WriteLine("Provádím tah: {0}", new Tah(prikazy));
                manazer.Deska.Tisk();
                
                if(!automatickaHraPocitace)
                {
                    Console.WriteLine("Pro další tah stiskněte Enter.");
                    Console.ReadLine();
                }
            }
        }
    }
}
*/