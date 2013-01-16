using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Timers;


// TODO: Vytvořit lepší správu výjimek.

namespace ObranaPevnosti
{
    public partial class Form1 : Form
    {
        delegate void SetVoidCallback();
        delegate void SetStatusCallback(string Text);


        private Manazer ManazerHry;
        private ToolStripMenuItem[] SeznamPolozekObtiznosti;
        private ToolStripMenuItem[] SeznamPolozekMoznosti;

        const int PolomerPolicka = 25;
        const int XMezeraMeziPolicky = 25;
        const int YMezeraMeziPolicky = 25;
        const int XPosunDesky = 19;
        const int YPosunDesky = 23;

        Image Utocnik;
        Image Obrance;

        /// <summary>
        /// Statusbar -- zobrazuje některé informace o průběhu hry
        /// </summary>
        private StatusBar Status;

        /// <summary>
        /// Indikuje, zda jsme již vybrali hrací kámen a provádíme s ním tah
        /// </summary>
        private bool PresunKamene = false;

        /// <summary>
        /// Obsahuje seznam načtených pozic, z kterého je pak vytvořen tah hráče.
        /// </summary>
        private List<Pozice> NactenePozice = new List<Pozice>();

        /// <summary>
        /// Název hry
        /// </summary>
        const string NazevHry = "Obrana Pevnosti";

        /// <summary>
        /// Uchovává si pozici políčka, přes které přejíždí myš
        /// </summary>
        private Pozice MouseHoverPozice = null;

        /// <summary>
        /// Uchovává si pozici kamene, se kterým chceme táhnout.
        /// </summary>
        private Pozice AktivniPozice = null;

        /// <summary>
        /// Provádíme vícenásobný skok?
        /// </summary>
        private bool VicenasobnySkok = false;

        /// <summary>
        /// Timer, který slouží k pozdějšímu přehrání celé hry
        /// </summary>
        private Timer PrubehHry;

        //private Timer HokusPokustimer;

        /// <summary>
        /// Výčet typů různého zobrazení políčka.
        /// </summary>
        private enum Policka
        {
            normalni, pevnost, hover, aktivni, skok
        }

        public Form1()
        {
            Status = new StatusBar();
            Status.Parent = this;
            Status.Text = "Obrana pevnosti";

            InitializeComponent();
            ManazerHry = new Manazer();
            SeznamPolozekObtiznosti = new ToolStripMenuItem[] { 
                this.snadnáToolStripMenuItem, 
                this.normálníToolStripMenuItem, 
                this.těžkáToolStripMenuItem };

            SeznamPolozekMoznosti = new ToolStripMenuItem[] {
                this.zpětToolStripMenuItem,
                this.vpředToolStripMenuItem,
                this.prohoďHráčeToolStripMenuItem,
                this.nejlepšíTahToolStripMenuItem,
                this.přehrátToolStripMenuItem
            };

            Utocnik = Image.FromFile("savle_maly.png");
            Obrance = Image.FromFile("stit_maly.png");

            Manazer.AktualizaceUI = AktualizujGUI;
            Manazer.AkcePoKonciHry = OznameniKonceHry;
            Manazer.AktualizaceHistorie = AktualizujPanelHistorie;
        }


        /// <summary>
        /// Hlavní metoda, která zajišťuje dění poté, co se klikne na hrací desku.
        /// </summary>
        private void Platno_MouseClick(object sender, MouseEventArgs e)
        {
            // Jestliže není hra spuštěna, nebudeme na kliknutí vůbec reagovat
            if (ManazerHry.KonecHry || ManazerHry.DvaPocitace)
                return;

            // Jestli jen potvrzujeme vícenásobný skok, skočíme
            if(e.Button == MouseButtons.Left && VicenasobnySkok)
            {
                VicenasobnySkok = false;
                ProvedTah();
            }

            Pozice Pozice = VratPoziciNaHraciDesce(e.X, e.Y);

            // Jestliže jsme klikli mimo desku, jen resetujeme všechny uložené věci 
            // Anebo ne
            if(Pozice == null || HraciDeska.Rohova(Pozice.Radek, Pozice.Sloupec))
            {
                /*AktivniPozice = null;
                PresunKamene = false;*/
                AktualizujGUI();
                return;
            }

            // Pokud už máme nastavené obránce, můžeme vytvářet normální tahy
            if (ManazerHry.NastaveniObranci)
            {
                // Už jsme klikli na kámen, se kterým budeme táhnout?
                if (PresunKamene)
                {
                    // Pokud jsme vybrali kámen a pak klikli na další kámen,
                    // dáme jako výchozí kámen ten druhý.
                    if(Manazer.Deska.Hrac(Pozice) == ManazerHry.hracNaTahu)
                    {
                        PresunKamene = true;
                        NactenePozice.Clear();
                        NactenePozice.Add((Pozice) Pozice.Clone());
                        AktivniPozice = Pozice;
                        AktualizujGUI();

                        return;
                    }

                    // Provádíme jednoduchý skok či tah?
                    // Nebo je na tahu útočník, který stejně skákat nemůže?
                    if(e.Button == MouseButtons.Left || ManazerHry.hracNaTahu == Manazer.StavPole.utok)
                    {
                        if(!VicenasobnySkok)
                            NactenePozice.Add((Pozice) Pozice.Clone());

                        ProvedTah();
                    }
                    // Provádíme vícenásobný skok?
                    else if(e.Button == MouseButtons.Right)
                    {
                        VicenasobnySkok = true;
                        NactenePozice.Add((Pozice) Pozice.Clone());
                    }
                }
                // Jestliže nemáme vybraný žádný kámen, pokusíme se nějaký vybrat
                else
                {
                    if(Manazer.Deska.Hrac(Pozice) == ManazerHry.hracNaTahu)
                    {
                        PresunKamene = true;
                        NactenePozice.Clear();
                        NactenePozice.Add((Pozice) Pozice.Clone());
                        AktivniPozice = Pozice;
                    }
                }
            }
            // Pokud nejsou nastavení obránci, zkusíme je nastavit
            else
            {
                try
                {
                    ManazerHry.NastavObrance(Pozice);
                    ZobrazHraceNaRade();
                    if (ManazerHry.NastaveniObranci)
                        if (ManazerHry.JePocitacNaTahu)
                            ManazerHry.Tahni();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            AktualizujGUI();
        }



        /// <summary>
        /// Provede tah
        /// </summary>
        private void ProvedTah()
        {
            Tah AktualniTah = new Tah(NactenePozice);
            ManazerHry.VratHraceNaTahu().AktualniTah = AktualniTah;

            try
            {
                ManazerHry.Tahni();
                Status.Text = NazevHry; 
            }
            catch(NeplatnyTah NT)
            {
                Status.Text = NT.Message;
                NactenePozice.Clear();
            }
            catch(ObranceMusiSkakat OMS)
            {
                Status.Text = OMS.Message;
                MessageBox.Show(OMS.Message);
            }

            ZobrazHraceNaRade();
            NactenePozice.Clear();

            PresunKamene = false;
            AktivniPozice = null;
        }

        /// <summary>
        /// Zvýrazní políčko, přes které zrovna přejedeme myší.
        /// </summary>
        private void Platno_MouseMove(object sender, MouseEventArgs e)
        {
            if(ManazerHry.KonecHry)
                return;

            Pozice HoverPozice = VratPoziciNaHraciDesce(e.X, e.Y);

            if(HoverPozice != null)
            {
                if(HraciDeska.Rohova(HoverPozice.Radek, HoverPozice.Sloupec))
                    return;

                // Status.Text = HoverPozice.ToString();
            }
                

            if(HoverPozice != MouseHoverPozice)
            {
                MouseHoverPozice = HoverPozice;
                Platno.Invalidate();
            }
        }


        /// <summary>
        /// Přepočítá souřadnice sejmuté po kliknutí myši na obrazovku na 
        /// ekvivalentní souřadnice na hrací desce.
        /// </summary>
        private Pozice VratPoziciNaHraciDesce(int X, int Y)
        {
            for (int i = 0; i < Manazer.Deska.deska.GetLength(0); i++)
            {
                for (int j = 0; j < Manazer.Deska.deska.GetLength(1); j++)
                {
                    if (KliknutoNaPolickoDesky(i, j, X, Y))
                        return new Pozice(j, i);
                }
            }

            return null;
        }


        /// <summary>
        /// Odpovídají souřadnice myši X a Y políčku na hrací desce
        /// o souřadnicích i a j?
        /// </summary>
        private bool KliknutoNaPolickoDesky(int i, int j, int X, int Y)
        {
            int radek = X - ((i * (2 * PolomerPolicka) + PolomerPolicka + i * XMezeraMeziPolicky) + XPosunDesky);
            int sloupec = Y - ((j * (2 * PolomerPolicka) + PolomerPolicka + j * YMezeraMeziPolicky) + YPosunDesky);

            return ((Math.Pow(radek, 2) + Math.Pow(sloupec, 2)) <= Math.Pow(PolomerPolicka, 2));
        }

        private void Platno_Paint(object sender, PaintEventArgs e)
        {
            VykresliHraciDesku(e.Graphics);

            if(!ManazerHry.KonecHry)
            {
                if(MouseHoverPozice != null)
                    VykresliHoverPolicko(e.Graphics);

                if(AktivniPozice != null)
                    VykresliAktivniPolicko(e.Graphics);

                if(NactenePozice.Count > 1)
                {
                    for(int i = 1; i < NactenePozice.Count; i++)
                        VykresliPolickoSkoku(e.Graphics, NactenePozice[i]);
                }
            }

            if (Manazer.Deska != null)
                VykresliHraciKameny(e.Graphics);           
        }

        /// <summary>
        /// Zvýrazní pole, na které jsme najeli myší.
        /// </summary>
        private void VykresliHoverPolicko(Graphics Platno)
        {
            VykresliPolicko(Platno, MouseHoverPozice.Radek, MouseHoverPozice.Sloupec, Policka.hover);
        }

        /// <summary>
        /// Zvýrazní kámen, kterým chceme táhnout
        /// </summary>
        /// <param name="Platno"></param>
        private void VykresliAktivniPolicko(Graphics Platno)
        {
            VykresliPolicko(Platno, AktivniPozice.Radek, AktivniPozice.Sloupec, Policka.aktivni);
        }

        /// <summary>
        /// Zvýrazní políčko, přes které se bude skákat
        /// </summary>
        private void VykresliPolickoSkoku(Graphics Platno, Pozice Pozice)
        {
            VykresliPolicko(Platno, Pozice.Radek, Pozice.Sloupec, Policka.skok);
        }

        /// <summary>
        /// Vykreslí hrací kameny, které jsou na hrací desce.
        /// </summary>
        private void VykresliHraciKameny(Graphics Platno)
        {
            for (int i = 0; i < Manazer.Deska.deska.GetLength(0); i++)
            {
                for (int j = 0; j < Manazer.Deska.deska.GetLength(1); j++)
                {
                    if (Manazer.Deska.deska[i, j] == Manazer.StavPole.utok)
                    {
                        NastavKamen(Brushes.Orange, Platno, i, j);
                    }
                    else if (Manazer.Deska.deska[i, j] == Manazer.StavPole.obrana)
                    {
                        NastavKamen(Brushes.Green, Platno, i, j);
                    }
                }
            }
        }


        /// <summary>
        /// Vykreslí samotnou hrací desku (podklad).
        /// </summary>
        private void VykresliHraciDesku(Graphics Platno)
        {
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    if(!HraciDeska.Rohova(i, j))
                        if(HraciDeska.Pevnost(i, j))
                            VykresliPolicko(Platno, i, j, Policka.pevnost);
                        else
                            VykresliPolicko(Platno, i, j, Policka.normalni);
                }
            }
        }

        /// <summary>
        /// Vykreslí jeden hrací kámen
        /// </summary>
        private void NastavKamen(Brush BarvaHrace, Graphics Platno, int i, int j)
        {
            // Platno.FillEllipse(BarvaHrace, new Rectangle(new Point(j * 35 + j * 40 + 30, i * 35 + i * 40 + 33), new Size(29, 29)));

            // Platno.FillRectangle(BarvaHrace, new Rectangle(new Point(j * 35 + j * 40 + 32, i * 35 + i * 40 + 35), new Size(25, 25)));

            if(BarvaHrace == Brushes.Green)
                Platno.DrawImage(Obrance, new Point(j * 35 + j * 40 + 29, i * 35 + i * 40 + 30));
            else
                Platno.DrawImage(Utocnik, new Point(j * 35 + j * 40 + 22, i * 35 + i * 40 + 25));

            /*Platno.FillPolygon(BarvaHrace, new Point[] {
                new Point(j * 35 + j * 40 + 40, i * 35 + i * 40 + 33),
                new Point(j * 35 + j * 40 + 50, i * 35 + i * 40 + 33),
                new Point(j * 35 + j * 40 + 55, i * 35 + i * 40 + 43),
                new Point(j * 35 + j * 40 + 50, i * 35 + i * 40 + 53),
                new Point(j * 35 + j * 40 + 40, i * 35 + i * 40 + 53),
                new Point(j * 35 + j * 40 + 35, i * 35 + i * 40 + 43)
            });*/
        }

        /// <summary>
        /// Vykreslí jedno hrací políčko.
        /// </summary>
        private void VykresliPolicko(Graphics Platno, int i, int j, Policka TypPole)
        {
            Brush BarvaPolicka;
            Pen OkrajPolicka;

            switch(TypPole)
            {
                case Policka.pevnost:
                    BarvaPolicka = Brushes.Yellow;
                    OkrajPolicka = Pens.Yellow;
                    break;

                case Policka.hover:
                    if(HraciDeska.Pevnost(i, j))
                    {
                        BarvaPolicka = Brushes.LightYellow;
                        OkrajPolicka = Pens.LightYellow;
                    }
                    else
                    {
                        BarvaPolicka = new SolidBrush(Color.FromArgb(75, 75, 255));
                        OkrajPolicka = new Pen(new SolidBrush(Color.FromArgb(75, 75, 255)));
                    }
                    break;

                case Policka.aktivni:
                    BarvaPolicka = new SolidBrush(Color.FromArgb(60, 60, 60));
                    OkrajPolicka = new Pen(new SolidBrush(Color.FromArgb(60, 60, 60)));
                    break;

                case Policka.normalni:
                    BarvaPolicka = Brushes.Blue;
                    OkrajPolicka = Pens.Blue;
                    break;

                case Policka.skok:
                    BarvaPolicka = Brushes.Red;
                    OkrajPolicka = Pens.Red;
                    break;

                default:
                    throw new Exception("Neplatný typ pole!");
            }

            if(TypPole == Policka.skok)
                Platno.DrawEllipse(OkrajPolicka, new Rectangle(new Point(j * 35 + j * 40 + 21,
                i * 35 + i * 40 + 24), new Size(48, 48)));
            else
                Platno.FillEllipse(BarvaPolicka, new Rectangle(new Point(j * 35 + j * 40 + 22, 
                    i * 35 + i * 40 + 25), new Size(46, 46)));

            Platno.DrawEllipse(OkrajPolicka, new Rectangle(new Point(j * 35 + j * 40 + 20, 
                i * 35 + i * 40 + 23), new Size(50, 50)));
        }

        private void nováHraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VstupniInformace VS = new VstupniInformace();
            VS.ShowDialog();

            if (VS.DialogResult == DialogResult.OK)
            {
                NastavPocatecniInformace(VS.SVI);

                // Když budou hrát dva počítače proti sobě, budeme to řešit trochu jinak
                if(ManazerHry.DvaPocitace)
                {
                    ManazerHry.Restart();
                    AktualizujGUI();

                    TahPocitacu();
                    return;
                }

                ManazerHry.Restart();

                ZobrazHraceNaRade();
                AktualizujGUI();
            }
        }

        private void TahPocitacu()
        {
            AktualizujGUI();
            ManazerHry.TahPocitace();
        }

        private void OznameniKonceHry()
        {
            switch (ManazerHry.Vitez)
            {
                case Manazer.MoznostiVyher.remiza:
                    MessageBox.Show("Výsledný stav: remíza.", "Konec hry");
                    break;
                case Manazer.MoznostiVyher.nikdo:
                    MessageBox.Show("Překročen maximální počet tahů.", "Konec hry");
                    break;
                case Manazer.MoznostiVyher.obrana:
                    MessageBox.Show(string.Format("Výsledný stav: Obránce {0} vyhrál", 
                        ManazerHry.Obrance.Jmeno), "Konec hry");
                    break;
                case Manazer.MoznostiVyher.utok:
                    MessageBox.Show(string.Format("Výsledný stav: Útočník {0} vyhrál",
                        ManazerHry.Utocnik.Jmeno), "Konec hry");
                    break;
            }
        }

        private void NastavPocatecniInformace()
        {
            ManazerHry.NastavObtiznost();
            PocitaciHraj.Enabled = false;

            MouseHoverPozice = null;
            AktivniPozice = null;

            VicenasobnySkok = false;

            Status.Text = NazevHry;

            restartHryToolStripMenuItem.Enabled = true;
            uložitHruToolStripMenuItem.Enabled = true;
            prohoďHráčeToolStripMenuItem.Enabled = true;
            uložitHruToolStripMenuItem.Enabled = true;
            restartHryToolStripMenuItem.Enabled = true;
            přehrátToolStripMenuItem.Enabled = true;
            možnostiToolStripMenuItem.Enabled = true;

            if(Minimax.vlakno != null)
                Minimax.vlakno.Abort();
        }

        private void NastavPocatecniInformace(SeznamVstupnichInformaci SVI)
        {
            if (SVI.JeUtocnikPocitacovyHrac)
            {
                ManazerHry.Utocnik = new AI(SVI.JmenoUtocnika, Manazer.StavPole.utok, 3);
            }
            else
            {
                ManazerHry.Utocnik = new LidskyHrac(SVI.JmenoUtocnika);
            }

            if (SVI.JeObrancePocitacovyHrac)
            {
                ManazerHry.Obrance = new AI(SVI.JmenoObrance, Manazer.StavPole.obrana, 3);
            }
            else
            {
                ManazerHry.Obrance = new LidskyHrac(SVI.JmenoObrance);
            }

            NastavPocatecniInformace();
        }

        private void konecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Minimax.vlakno != null && Minimax.vlakno.IsAlive)
                Minimax.vlakno.Abort();

            Close();
        }

        private void snadnáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetujObtížnosti();

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.snadna);
            if (sender is ToolStripMenuItem)
                ((ToolStripMenuItem)sender).Checked = true;

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.snadna);
        }

        private void normálníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetujObtížnosti();

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.normalni);
            if (sender is ToolStripMenuItem)
                ((ToolStripMenuItem)sender).Checked = true;

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.normalni);
        }

        private void těžkáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetujObtížnosti();

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.tezka);
            if (sender is ToolStripMenuItem)
                ((ToolStripMenuItem)sender).Checked = true;

            ManazerHry.NastavObtiznost(Manazer.Obtiznost.tezka);
        }

        private void ResetujObtížnosti()
        {
            foreach (ToolStripMenuItem Polozka in SeznamPolozekObtiznosti)
                Polozka.Checked = false;
        }

        private void restartHryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NastavPocatecniInformace();

            if(ManazerHry.DvaPocitace)
            {
                ManazerHry.Restart();
                AktualizujGUI();

                TahPocitacu();
                return;
            }

            ManazerHry.Restart();
            AktualizujGUI();
        }

        private void uložitHruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog Uloz = new SaveFileDialog();
            Uloz.ShowDialog();

            try
            {
                if (Uloz.FileName != String.Empty)
                    Partie.UlozHru(Uloz.FileName, ManazerHry);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Hru se nepodařilo uložit. Typ chyby: {0}", ex.Message), 
                    "Chyba při ukládání");
            }
        }

        private void nahrátHruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Nacti = new OpenFileDialog();
            Nacti.ShowDialog();

            try
            {
                if (Nacti.FileName != String.Empty)
                {
                    NastavPocatecniInformace();
                    Schranka CelaHra = Partie.NahrajHru(Nacti.FileName);
                    ManazerHry = CelaHra.ManazerHry;
                    Manazer.Deska = CelaHra.AktualniHraciDeska;
                    AktualizujGUI();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Neplatný vstupní soubor.", "Chyba při nahrávání");
            }
        }

        private void zpětToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TahZpet();
        }

        private void TahZpet()
        {
            try
            {
                PripravaNaZpetVpred();

                ManazerHry.TahZpet();
                if(ManazerHry.JePocitacNaTahu)
                    PocitaciHraj.Enabled = true;
                else
                    PocitaciHraj.Enabled = false;

                ObnovaPoZpetVpredTazich();
            }
            catch(InvalidOperationException) { }
        }

        private void vpředToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TahVpred();
        }

        private void TahVpred()
        {
            try
            {
                PripravaNaZpetVpred();

                ManazerHry.TahVpred();
                if(ManazerHry.JePocitacNaTahu)
                    PocitaciHraj.Enabled = true;
                else
                    PocitaciHraj.Enabled = false;

                ObnovaPoZpetVpredTazich();
            }
            catch { }
        }

        private void PripravaNaZpetVpred()
        {
            if(Minimax.vlakno != null && Minimax.vlakno.IsAlive)
                Minimax.vlakno.Abort();
        }

        private void ObnovaPoZpetVpredTazich()
        {
            NactenePozice.Clear();
            AktivniPozice = null;
            AktualizujGUI();
            AktualizujPanelHistorie();
            PresunKamene = false; 
            VicenasobnySkok = false;
        }

        private void nejlepšíTahToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Minimax.AkcePoNalezeniTahu = zobrazNejTah;
            Minimax.NejlepsiTah(Manazer.Deska, ManazerHry.hracNaTahu, 2);
        }

        private void zobrazNejTah(Tah NejTah)
        {
            MessageBox.Show(NejTah.ToString());
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AktualizujGUI();
        }

        private void ZobrazHraceNaRade()
        {
            if(NaRadeLabel.InvokeRequired)
            {
                SetVoidCallback d = new SetVoidCallback(ZobrazHraceNaRade);
                this.Invoke(d, new object[] { });

            }
            else
            {
                if(ManazerHry.KonecHry)
                    NaRadeLabel.Text = "Nikdo";
                else
                    NaRadeLabel.Text = ManazerHry.VratHraceNaTahu().Jmeno +
                                " (" + ManazerHry.hracNaTahu + ")";
            }
        }

        private void PocitaciHraj_Click(object sender, EventArgs e)
        {
            if(ManazerHry.NastaveniObranci)
                ManazerHry.Tahni();
            else
                ManazerHry.NastavObrance();

            AktualizujGUI();
            PocitaciHraj.Enabled = false;
        }


        /// <summary>
        /// Aktualizuje ovládací prvky v GUI a průběh samotné hry
        /// </summary>
        private void AktualizujGUI()
        {
            ZobrazHraceNaRade();

            if(ManazerHry.KonecHry)
                NastavStatus("Konec hry");

            // TODO: Tohle se kurví, opravit
            // AktualizujPanelHistorie();
            AktualizujMenu();

            Platno.Invalidate();
        }

        private void AktualizujMenu()
        {
            if(menuStrip1.InvokeRequired)
            {
                SetVoidCallback d = new SetVoidCallback(AktualizujMenu);
                this.Invoke(d, new object[] { });
            }
            else
            {
                vpředToolStripMenuItem.Enabled = ManazerHry.MuzemeTahnoutVpred();
                zpětToolStripMenuItem.Enabled = ManazerHry.MuzemeTahnoutZpet();
                nejlepšíTahToolStripMenuItem.Enabled = ManazerHry.NastaveniObranci && !ManazerHry.KonecHry;
            }
        }

        private void NastavStatus(string Text)
        {
            if(Status.InvokeRequired)
            {
                SetStatusCallback d = new SetStatusCallback(NastavStatus);
                this.Invoke(d, new object[] { Text });
            }
            else
            {
                Status.Text = Text;
            }
        }


        private void AktualizujPanelHistorie()
        {
            if(Historie.InvokeRequired)
            {
                SetVoidCallback d = new SetVoidCallback(AktualizujPanelHistorie);
                this.Invoke(d, new object[] { });
            }
            else
            {
                Historie.Items.Clear();

                foreach(Tah tah in ManazerHry.TahyVpred.Reverse<Tah>())
                {
                    Tah NovyTah = (Tah) tah.Clone();
                    NovyTah.seznamTahu.Reverse();
                    Historie.Items.Add(NovyTah);
                }

                foreach(Tah tah in ManazerHry.TahyZpet)
                    Historie.Items.Add(tah.Clone());

                if(ManazerHry.TahyZpet.Count > 0)
                    Historie.SelectedIndex = ManazerHry.TahyVpred.Count;
            }
        }

        private void prohoďHráčeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManazerHry.PrehodHrace();

            if (ManazerHry.JePocitacNaTahu)
                PocitaciHraj.Enabled = true;

            AktualizujGUI();
        }

        private void Historie_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rozdil = Historie.SelectedIndex - ManazerHry.TahyVpred.Count;

            if(rozdil > 0)
            {
                for(int i = 0; i < rozdil; i++)
                    TahZpet();
            }

            if(rozdil < 0)
            {
                for(int i = 0; i < -rozdil; i++)
                    TahVpred();
            }

            Platno.Invalidate();
        }

        /// <summary>
        /// Začne přehrávat od začátku celou hru
        /// </summary>
        private void přehrátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NastavAPrehrajHru();
        }

        private void NastavAPrehrajHru()
        {
            int PocetTahu = ManazerHry.TahyZpet.Count;

            for(int i = 0; i < PocetTahu; i++)
                ManazerHry.TahZpet();

            Pausa.Enabled = true;

            PrubehHry = new Timer();
            PrubehHry.Interval = 750;
            PrubehHry.Tick += new EventHandler(PrubehHry_Tick);
            PrubehHry.Start();

            AktualizujGUI();
        }

        void PrubehHry_Tick(object sender, EventArgs e)
        {
            if(ManazerHry.TahyVpred.Count == 0)
            {
                PrubehHry.Stop();
                Pausa.Enabled = false;
            }
            else
            {
                TahVpred();
            }
        }

        private void Pausa_Click(object sender, EventArgs e)
        {
            if(PrubehHry.Enabled)
            {
                PrubehHry.Stop();
                Pausa.Text = "Spusť přehrávání";
            }
            else
            {
                PrubehHry.Start();
                Pausa.Text = "Zastav Přehrávání";
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if(Minimax.vlakno != null && Minimax.vlakno.IsAlive)
                Minimax.vlakno.Abort();
        }
    }
}
