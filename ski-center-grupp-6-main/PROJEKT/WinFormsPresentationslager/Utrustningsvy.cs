using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class Utrustningsvy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        


        private DateTime fDatum = new DateTime();
        private DateTime tDatum = new DateTime();
        public List<Utrustning> LedigUtrustning { get; set; }
        public Kund HämtadKund { get; set; }
        public List<Utrustning> uAttBoka { get; set; }
        public List<UtrustningBokning> uBokning { get; set; }
        public List<RumBokning> rBokning { get; set; }
        public List<LektionBokning> lBokning { get; set; }
        public List<Rum> rAttBoka { get; set; }
        public List<Lektion> lAttBoka { get; set; }
        public Bokning OavslutadBokning { get; set; }
        public Bokning BefintligBokning { get; set; }
        public FöretagsKund HämtadFöretagsKund { get; set; }
        public int Kostnad { get; set; }
        public int PaketPris { get; set; }

        public Utrustningsvy(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;
            uAttBoka = new List<Utrustning>();
            LedigUtrustning = new List<Utrustning>();
            BefintligBokning = new Bokning();
            dateTimePickerStart.MinDate = DateTime.Now;
            LäggTillIBefintligBokning();
            PågåendeBokning();

        }
        /// <summary>
        /// Hämtar befintlig bokning vald i valdbokningsvy för att lägga till rum
        /// </summary>
        private void LäggTillIBefintligBokning()
        {
            BefintligBokning = bk.HämtaBokning();
            if (BefintligBokning != null)
            {

                if (BefintligBokning.Kund != null)
                {
                    HämtadKund = BefintligBokning.Kund;
                    textBoxKNamn.Text = BefintligBokning.Kund.FörNamn + " " + BefintligBokning.Kund.EfterNamn;
                    textBoxKredgräns.Text = BefintligBokning.Kund.KreditGräns.ToString();
                }
                else
                {
                    HämtadFöretagsKund = BefintligBokning.FKund;
                    textBoxKNamn.Text = BefintligBokning.FKund.FöretagsNamn;
                    textBoxKredgräns.Text = BefintligBokning.FKund.KreditGräns.ToString();
                }
                textBoxBokNr.Text = BefintligBokning.BokningsNr;
                textBoxPeriod.Text = BefintligBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + BefintligBokning.TillDatum.ToString("dd-MM-yyyy");
                radioButtonPK.Checked = true;

                dateTimePickerStart.MinDate = DateTime.Parse(BefintligBokning.FrånDatum.ToString("yyyy/MM/dd"));
                dateTimePickerSlut.Value = BefintligBokning.TillDatum;
                FyllBokningLista();
            }
        }

        private void PågåendeBokning()
        {
            if (OavslutadBokning == null)
            {
                OavslutadBokning = bk.HämtaPågåendeBokning();
                rAttBoka = bk.HämtaPågåendeRum();
                uAttBoka = uk.HämtaPågåendeUtrustning();
                uBokning = uk.HämtaPågåendeuBokning();
                lAttBoka = lk.HämtaPågåendeLektion();
                lBokning = lk.HämtaPågåendelBokning();
                rBokning = bk.HämtaPågåenderBokning();
            }

            if (OavslutadBokning != null)
            {
                string bNr = "Bokningsnr inte skapad än";
                if (OavslutadBokning.BokningsNr != null)
                {
                    textBoxBokNr.Text = OavslutadBokning.BokningsNr;
                }
                else
                {
                    textBoxBokNr.Text = bNr;
                }
                if (OavslutadBokning.Kund != null)
                {
                    HämtadKund = OavslutadBokning.Kund;
                    textBoxKNamn.Text = OavslutadBokning.Kund.FörNamn + " " + OavslutadBokning.Kund.EfterNamn;
                    textBoxKredgräns.Text = OavslutadBokning.Kund.KreditGräns.ToString();
                }
                else
                {
                    HämtadFöretagsKund = OavslutadBokning.FKund;
                    textBoxKNamn.Text = OavslutadBokning.FKund.FöretagsNamn;
                    textBoxKredgräns.Text = OavslutadBokning.FKund.KreditGräns.ToString();
                }
                textBoxPeriod.Text = OavslutadBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + OavslutadBokning.TillDatum.ToString("dd-MM-yyyy");
                radioButtonPK.Checked = true;
                dateTimePickerStart.MinDate = OavslutadBokning.FrånDatum;
                dateTimePickerSlut.Value = OavslutadBokning.TillDatum;
                FyllBokningLista();

            }


        }
        private void FyllBokningLista()
        {
            listBoxBokningar.Items.Add("Rum:");
            if (rAttBoka != null)
            {
                foreach (Rum r in rAttBoka)
                {
                    listBoxBokningar.Items.Add(r.Typ + "   " + r.RumsStorlek);
                }
            }
            listBoxBokningar.Items.Add("Utrustning:");
            if (uAttBoka != null)
            {
                foreach (Utrustning u in uAttBoka)
                {
                    listBoxBokningar.Items.Add(u.Typ + "   " + "   " + u.UtrustningsArtikel);
                }
            }
            listBoxBokningar.Items.Add("Lektioner:");
            if (lAttBoka != null)
            {
                foreach (Lektion l in lAttBoka)
                {
                    listBoxBokningar.Items.Add(l.Typ + "   " + "   " + l.LektionStart);


                }
            }
        }
        /// <summary>
        /// Fyller datagriden med tillgänglig utrustning
        /// </summary>
        /// <param name="ledigaRum"></param>
        private void FyllLista(List<Utrustning> ledigUtrustnung)
        {
            dataGridViewUtrustning.DataSource = LedigUtrustning;

        }


        private void buttonBokningar_Click(object sender, EventArgs e)
        {
            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonRum_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2)
            {
                new Rumvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buttonLektion_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                new Lektionsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buttonKundRegister_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() != 5 || pk.HämtaBehörighet() != 0)
            {
                new Kundregister(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void stängProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loggaUtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new LoggaIn(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void loggaUtToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gåTillAdministrationssidanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 5)
            {
                new SystemadminPersonal(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ogiltig behörighet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void seÄrendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 1)
            {
                new Ärenden(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ogiltig behörighet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buttonHutrustning_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2)
            {
                bk.HämtaBokningsinnehåll(rAttBoka, lAttBoka, uAttBoka, BefintligBokning, fDatum, tDatum, HämtadKund, HämtadFöretagsKund, BefintligBokning.Avbokningsskydd, uBokning, lBokning, rBokning, Kostnad);
                new Rumvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        private void listBoxBokningar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonTaBort_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSök_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Söker fram kund baserat på person/orgnummer eller bokningssnummer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSöka_Click(object sender, EventArgs e)
        {
            listBoxBokningar.Items.Clear();
            string input = textBoxSök.Text;
            if (radioButtonPersorgNr.Checked)
            {
                long inputInt = long.Parse(input);
                HämtadKund = kk.SökKund(inputInt);
                if (HämtadKund == null)
                {
                    SökFöretagsKund(input);
                }
                else
                {
                    FyllKundData();
                }
            }
            else if (radioButtonBokningsNr.Checked)
            {
                HämtadKund = kk.SökKundPerBokningsNr(input);
                if (HämtadKund == null)
                {
                    SökFöretagsKund(input);
                }
                else
                {
                    FyllKundData();
                }
            }
            else
            {
                MessageBox.Show("Bocka i person/orgnr eller bokningsnr innan du söker.");
            }
        }
        /// <summary>
        /// Fyller datagrid med kunddata från hämtning av data från databasen
        /// </summary>
        private void FyllKundData()
        {
            Bokning hämtadBokning = bk.SökAktuellKundBokning(HämtadKund);
            textBoxKNamn.Text = HämtadKund.FörNamn + " " + HämtadKund.EfterNamn;
            textBoxKredgräns.Text = HämtadKund.KreditGräns.ToString();
            textBoxBokNr.Text = hämtadBokning.BokningsNr;
            textBoxPeriod.Text = hämtadBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + hämtadBokning.TillDatum.ToString("dd-MM-yyyy");
            radioButtonPK.Checked = true;
            FyllBokningsInnehåll(hämtadBokning);
        }
        /// <summary>
        /// Söker fram företagskund om systemet inte hittar privatkund med inmatad data i sökfältet
        /// </summary>
        /// <param name="input"></param>
        private void SökFöretagsKund(string input)
        {
            if (radioButtonPersorgNr.Checked)
            {
                long inputInt = long.Parse(input);
                HämtadFöretagsKund = kk.SökFöretagsKundPerOrgNr(inputInt);
                if (HämtadFöretagsKund == null)
                {
                    MessageBox.Show("Inget resultat. Kontrollera att du skrev in rätt nummer, alternativt lägg till ny kund om kund inte tidigare har bokat.");
                }
                else
                {
                    FyllFöretagsKundData();
                }
            }
            else if (radioButtonBokningsNr.Checked)
            {

                HämtadFöretagsKund = kk.SökFöretagsKundPerBokningsNr(input);
                if (HämtadFöretagsKund == null)
                {
                    MessageBox.Show("Inget resultat. Kontrollera att du skrev in rätt nummer, alternativt lägg till ny kund om kund inte tidigare har bokat.");
                }
                else
                {
                    FyllFöretagsKundData();
                }
            }
        }
        /// <summary>
        /// Fyller datagrid med företagskundsdata utifrån hämtad data från databas
        /// </summary>
        private void FyllFöretagsKundData()
        {
            Bokning hämtadBokning = bk.SökAktuellFKundBokning(HämtadFöretagsKund);
            textBoxKNamn.Text = HämtadFöretagsKund.FöretagsNamn;
            textBoxKredgräns.Text = HämtadFöretagsKund.KreditGräns.ToString();
            textBoxBokNr.Text = hämtadBokning.BokningsNr;
            textBoxPeriod.Text = hämtadBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + hämtadBokning.TillDatum.ToString("dd-MM-yyyy");
            radioButtonFK.Checked = true;
            FyllBokningsInnehåll(hämtadBokning);
        }

        private void textBoxBokNr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxKNamn_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPeriod_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonPK_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonFK_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxUTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUTyp.SelectedItem.ToString() == "Alpint" || comboBoxUTyp.SelectedItem.ToString() == "Längd")
            {
                comboBoxArtikel.Items.Clear();
                comboBoxArtikel.Items.Add("Paket");
                comboBoxArtikel.Items.Add("Pjäxor");
                comboBoxArtikel.Items.Add("Skidor");
                comboBoxArtikel.Items.Add("Stavar");

            }
            else if (comboBoxUTyp.SelectedItem.ToString() == "Snowboard")
            {
                comboBoxArtikel.Items.Clear();
                comboBoxArtikel.Items.Add("Paket");
                comboBoxArtikel.Items.Add("Snowboard");
                comboBoxArtikel.Items.Add("Snowboardsskor");
            }
            else if (comboBoxUTyp.SelectedItem.ToString() == "Skoter")
            {
                comboBoxArtikel.Items.Clear();
                comboBoxArtikel.Items.Add("Lynx 50");
                comboBoxArtikel.Items.Add("Yamaha Viking");
            }
            else if (comboBoxUTyp.SelectedItem.ToString() == "Pulka" || comboBoxUTyp.SelectedItem.ToString() == "Hjälm")
            {
                comboBoxArtikel.Items.Clear();

            }
        }


        private void comboBoxArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxStorlek_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerSlut_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonHämta_Click(object sender, EventArgs e)
        {
            
                comboBoxStorlek.Items.Clear();
                fDatum = dateTimePickerStart.Value.Date;
                tDatum = dateTimePickerSlut.Value.Date;
                if ((fDatum - tDatum).TotalDays < -5)
                {
                    MessageBox.Show("Utrustning kan enbart bokas i max fem dagar.");
                }
                else
                {
                

                
                    if (LedigUtrustning != null)
                    {
                        UppdateraLista();
                    }
                if (comboBoxUTyp.SelectedItem != null && comboBoxArtikel.SelectedItem != null) //valde nästlade if-satser då det ej funkade med try-catch
                {
                    if (comboBoxUTyp.SelectedItem.ToString() == "Alpint" && comboBoxArtikel.SelectedItem.ToString() == "Paket")
                    {



                        string typ = "Alpint";
                        string artikel = null;

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }




                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Alpint" && comboBoxArtikel.SelectedItem.ToString() == "Pjäxor")
                    {

                        string typ = "Alpint";
                        string artikel = "Pjäxor";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }

                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Alpint" && comboBoxArtikel.SelectedItem.ToString() == "Skidor")
                    {

                        string typ = "Alpint";
                        string artikel = "Skidor";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Alpint" && comboBoxArtikel.SelectedItem.ToString() == "Stavar")
                    {

                        string typ = "Alpint";
                        string artikel = "Stavar";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        GridDesign();
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Längd" && comboBoxArtikel.SelectedItem.ToString() == "Paket")
                    {
                        string typ = "Längd";
                        string artikel = null;

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }

                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Längd" && comboBoxArtikel.SelectedItem.ToString() == "Pjäxor")
                    {

                        string typ = "Längd";
                        string artikel = "Pjäxor";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Längd" && comboBoxArtikel.SelectedItem.ToString() == "Skidor")
                    {

                        string typ = "Längd";
                        string artikel = "Skidor";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Längd" && comboBoxArtikel.SelectedItem.ToString() == "Stavar")
                    {

                        string typ = "Längd";
                        string artikel = "Stavar";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Snowboard" && comboBoxArtikel.SelectedItem.ToString() == "Paket")
                    {
                        string typ = "Snowboard";
                        string artikel = null;

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Snowboard" && comboBoxArtikel.SelectedItem.ToString() == "Snowboard")
                    {
                        string typ = "Snowboard";
                        string artikel = "Snowboard ";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Snowboard" && comboBoxArtikel.SelectedItem.ToString() == "Snowboardsskor")
                    {
                        string typ = "Snowboard";
                        string artikel = "Snowboardskor";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Skoter" && comboBoxArtikel.SelectedItem.ToString() == "Lynx 50")
                    {
                        if ((fDatum.Date - tDatum.Date).Days == -2 || (fDatum.Date - tDatum.Date).Days == -4)
                        {
                            MessageBox.Show("Skoter går endast att boka i 1, 3 eller 5 dagar.");
                        }
                        else
                        {
                            string typ = "Skoter";
                            string artikel = "Lynx 50";

                            foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                            {
                                LedigUtrustning.Add(u);
                                comboBoxStorlek.Items.Add(u.Storlek);
                                int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                                u.Pris = dagspris;

                            }
                            FyllLista(LedigUtrustning);
                            if (LedigUtrustning != null)
                            {
                                GridDesign();
                            }
                        }

                    }
                    else if (comboBoxUTyp.SelectedItem.ToString() == "Skoter" && comboBoxArtikel.SelectedItem.ToString() == "Yamaha Viking")
                    {
                        if ((fDatum.Date - tDatum.Date).Days == -2 || (fDatum.Date - tDatum.Date).Days == -4)
                        {
                            MessageBox.Show("Skoter går endast att boka i 1, 3 eller 5 dagar.");
                        }
                        else
                        {
                            string typ = "Skoter";
                            string artikel = "Yamaha Viking";

                            foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                            {
                                LedigUtrustning.Add(u);
                                comboBoxStorlek.Items.Add(u.Storlek);
                                int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                                u.Pris = dagspris;
                            }
                            FyllLista(LedigUtrustning);
                            if (LedigUtrustning != null)
                            {
                                GridDesign();
                            }
                        }
                    }


                }
                else
                {
                    Console.WriteLine("error utrustning"); //console istället för messagebox eftersom det redan dyker upp en messagebox
                }
                if (comboBoxUTyp.SelectedItem != null && comboBoxArtikel.SelectedItem == null) //för alternativ utan specifika artikel
                {

                
                    if (comboBoxUTyp.SelectedItem.ToString() == "Pulka" && comboBoxArtikel.SelectedItem == null)
                {
                    if ((fDatum.Date - tDatum.Date).Days == -2 || (fDatum.Date - tDatum.Date).Days == -4)
                    {
                        MessageBox.Show("Skoter går endast att boka i 1, 3 eller 5 dagar.");
                    }
                    else
                    {
                        string typ = "Pulka";
                        string artikel = "Nilapulka";

                        foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                        {
                            LedigUtrustning.Add(u);
                            comboBoxStorlek.Items.Add(u.Storlek);
                            int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                            u.Pris = dagspris;
                        }
                        FyllLista(LedigUtrustning);
                        if (LedigUtrustning != null)
                        {
                            GridDesign();
                        }
                    }


                }
                else if (comboBoxUTyp.SelectedItem.ToString() == "Hjälm" && comboBoxArtikel.SelectedItem == null)
                {
                    string typ = "Hjälm";
                    string artikel = "Hjälm";

                    foreach (Utrustning u in uk.HittaLedigUtrustning(fDatum, tDatum, typ, artikel))
                    {
                        LedigUtrustning.Add(u);
                        comboBoxStorlek.Items.Add(u.Storlek);
                        int dagspris = uk.HämtaUtrustningPris(u.Typ, u.UtrustningsArtikel, fDatum, tDatum);
                        u.Pris = dagspris;
                    }
                    FyllLista(LedigUtrustning);
                    if (LedigUtrustning != null)
                    {
                        GridDesign();
                    }
                }
                else
                {
                    MessageBox.Show("Ogiltiga alternativ");
                }
                }
                else
                {
                    Console.WriteLine("error");
                    //MessageBox.Show("Fel i utrustningsvalet. Vänligen försök igen."); fel dyker upp trots rätt val
                }
               
            }

            
           

        }


        private void radioButtonavbokskydd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRabatt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMoms_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxKostnad_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonBLektion_Click(object sender, EventArgs e)
        {
            BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, tDatum, fDatum);
            if (BefintligBokning == null && OavslutadBokning == null)
            {
                MessageBox.Show("Kund måste ha en aktiv logibokning för att kunna boka konferensrum. Boka logi och försök igen");
            }
            else
            {

                if (OavslutadBokning != null)
                {
                    BefintligBokning = OavslutadBokning;
                }
                bk.HämtaBokningsinnehåll(rAttBoka, lAttBoka, uAttBoka, BefintligBokning, fDatum, tDatum, HämtadKund, HämtadFöretagsKund, BefintligBokning.Avbokningsskydd, uBokning, lBokning, rBokning, Kostnad);
                new Lektionsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
        }


        private void buttonSlutför_Click(object sender, EventArgs e)
        {


            int rabatt = 0;
            if (HämtadKund == null && HämtadFöretagsKund == null)
            {
                Console.WriteLine("Ingen bokning vald att slutföra.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
           else if (BefintligBokning == null)
            {
                BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, fDatum, tDatum);
            }


            if (OavslutadBokning == null || BefintligBokning != null)
            {
                try
                {


                    if (BefintligBokning != null)
                    {

                        Bokning bokning = bk.LäggTillIBokning(lAttBoka, uAttBoka, rAttBoka, BefintligBokning, uBokning, lBokning, rBokning);
                        Faktura f = bk.HämtaFaktura(bokning);
                        if (bokning.Kund != null)
                        {
                            MessageBox.Show($"Bokning med bokningsnummer {bokning.BokningsNr} är nu kompletterad med utrustning! \n\nBokningsdetaljer:" +
                            $"\nNamn: {HämtadKund.FörNamn} {HämtadKund.EfterNamn}\nKundnummer: {HämtadKund.KundNr}" +
                            $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal utrustning: {uAttBoka.Count}\n" +
                            $"Förfallodatum för debitering ett: {f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\nFörfallodatum för debitering två: {f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}" +
                            $"\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}");
                        }
                        else
                        {
                            MessageBox.Show($"Bokning med bokningsnummer {bokning.BokningsNr} är nu kompletterad med utrustning! \n\nBokningsdetaljer:" +
                            $"\nNamn: {HämtadFöretagsKund.FöretagsNamn}\nKundnummer: {HämtadFöretagsKund.FKundNr}" +
                            $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\n" +
                            $"Antal utrustning: {uAttBoka.Count}\nFörfallodatum för debitering ett: { f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\n" +
                            $"Förskottsbelopp: {f.DelBelopp}\nFörfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}");
                        }
                        new ValdBokningsvy(bk, BefintligBokning, kk, rk, uk, lk, pk).Show();
                    }
                    else
                    {
                        MessageBox.Show("Kund måste ha en befintlig logibokning för att kunna boka utrustning");
                    }
                    UppdateraLista();
                    if (uAttBoka != null)
                    {
                        uAttBoka.Clear();
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
                catch
                {
                    MessageBox.Show("Fel upptäckt vid slutförandet av bokningen. Vänligen försök igen", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (OavslutadBokning != null && BefintligBokning == null)
            {
                string avbok = "Nej";
                if (OavslutadBokning.Avbokningsskydd == true)
                {
                    avbok = "Ja";
                }
                OavslutadBokning = bk.SkapaBokning(rAttBoka, OavslutadBokning.Kund, OavslutadBokning.FKund, OavslutadBokning.FrånDatum, OavslutadBokning.TillDatum, lAttBoka, uAttBoka, rabatt, OavslutadBokning.Avbokningsskydd, OavslutadBokning.BokningsNr, uBokning, lBokning, rBokning);
                Faktura f = bk.HämtaFaktura(OavslutadBokning);
                if (OavslutadBokning.Kund != null)
                {
                    MessageBox.Show($"Bokning är nu slutförd med bokningsnummer {OavslutadBokning.BokningsNr}! \n\nBokningsdetaljer:" +
                        $"\nNamn: {OavslutadBokning.Kund.FörNamn} {OavslutadBokning.Kund.EfterNamn}\nKundnummer: {OavslutadBokning.Kund.KundNr}" +
                        $"\nPeriod: {OavslutadBokning.FrånDatum.ToString("dd-MM-yyyy")} - {OavslutadBokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\nFörfallodatum för debitering ett: {f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\n" +
                        $"Förskottsbelopp: {f.DelBelopp}\nFörfallodatum för debitering två: {f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {avbok}");

                }
                else
                {
                    MessageBox.Show($"Bokning är nu slutförd med bokningsnummer {OavslutadBokning.BokningsNr}! \n\nBokningsdetaljer:" +
                       $"\nNamn: {OavslutadBokning.FKund.FöretagsNamn}\nKundnummer: {OavslutadBokning.FKund.FKundNr}" +
                       $"\nPeriod: {OavslutadBokning.FrånDatum.ToString("dd-MM-yyyy")} - {OavslutadBokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\nFörfallodatum för debitering ett: { f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\n" +
                        $"Förskottsbelopp: {f.DelBelopp}\nFörfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {avbok}");

                }

                UppdateraLista();
                new ValdBokningsvy(bk, OavslutadBokning, kk, rk, uk, lk, pk).Show();

            }
            if (rAttBoka != null)
            {
                rAttBoka.Clear();
            }
            if (uAttBoka != null)
            {
                uAttBoka.Clear();
            }
            if (lAttBoka != null)
            {
                lAttBoka.Clear();
            }
            if (rBokning != null)
            {
                rBokning.Clear();
            }
            if (uBokning != null)
            {
                uBokning.Clear();
            }
            if (lBokning != null)
            {
                lBokning.Clear();
            }
            if (rBokning != null)
            {
                rBokning.Clear();
            }
            OavslutadBokning = null;
            BefintligBokning = null;
        }




        private void Utrustningsvy_Load(object sender, EventArgs e)
        {
            comboBoxUTyp.Items.Add("Alpint");
            comboBoxUTyp.Items.Add("Längd");
            comboBoxUTyp.Items.Add("Snowboard");
            comboBoxUTyp.Items.Add("Skoter");
            comboBoxUTyp.Items.Add("Pulka");
            comboBoxUTyp.Items.Add("Hjälm");

        }
        /// <summary>
        /// Uppdaterar datagrid så att den tar bort utrustning som visas innan användaren initierar att hämta utrustning via att klicka på Hämta
        /// </summary>
        public void UppdateraLista()
        {
            fDatum = dateTimePickerStart.Value.Date;
            tDatum = dateTimePickerSlut.Value.Date;
            dataGridViewUtrustning.DataSource = null;
            LedigUtrustning.Clear();


        }
        /// <summary>
        /// Tar bort vissa fält i gridview som inte behöver/ska visas
        /// </summary>
        public void GridDesign()
        {
            dataGridViewUtrustning.Columns["Tillgänglig"].Visible = false;
            dataGridViewUtrustning.Columns["UtrustningBokning"].Visible = false;
            dataGridViewUtrustning.AllowUserToOrderColumns = true;
            LedigUtrustning.Sort(
            delegate (Utrustning p1, Utrustning p2)
            {
                int compare = p1.UtrustningsArtikel.CompareTo(p2.UtrustningsArtikel);
                if (compare == 0)
                {
                    return p2.Benämning.CompareTo(p1.Benämning);
                }
                return compare;
            });
        }
        /// <summary>
        /// Metod för att sortea listan efter storlek
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUppdatera_Click(object sender, EventArgs e)
        {
            dataGridViewUtrustning.DataSource = null;
            List<Utrustning> sUtrustning = new List<Utrustning>();
            if (comboBoxStorlek.SelectedItem == null)
            {
                MessageBox.Show("Ingen storlek vald.");
            }
            else
            {
                string valdStorlek = comboBoxStorlek.SelectedItem.ToString();
                int vStorlek = int.Parse(valdStorlek);
                foreach (Utrustning ut in LedigUtrustning.Where(b => b.Storlek.Equals(vStorlek)))
                {
                    int storlek = ut.Storlek; 
                    if (!sUtrustning.Any(t=>t.Storlek == storlek))
                    {
                        sUtrustning.Add(ut);
                    }
                    
                }
                dataGridViewUtrustning.DataSource = sUtrustning;
                GridDesign();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Tillagda")
            {
                dataGridViewUtrustning.DataSource = uAttBoka;
                button2.Text = "Tillgängliga";
                buttonTaBortUt.Visible = true;
                buttonLäggTill.Visible = false;
                GridDesign();
            }
            else if (button2.Text == "Tillgängliga")
            {
                FyllLista(LedigUtrustning);
                GridDesign();
                button2.Text = "Tillagda";
                buttonTaBortUt.Visible = false;
                buttonLäggTill.Visible = true;
            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            try
            {
                Utrustning valdUtrustning = dataGridViewUtrustning.SelectedRows[0].DataBoundItem as Utrustning;
                List<Utrustning> alpintpaket = new List<Utrustning>();
                List<Utrustning> längdpaket = new List<Utrustning>();
                if (uAttBoka == null)
                {
                    uAttBoka = new List<Utrustning>();
                }
                if (uBokning == null)
                {
                    uBokning = new List<UtrustningBokning>();
                }
                int kostnad = valdUtrustning.Pris ?? default(int);
                UtrustningBokning ub = new UtrustningBokning(valdUtrustning.Benämning, null, valdUtrustning, fDatum, tDatum, kostnad);

                Kostnad += kostnad;

                textBoxKostnad.Text = Kostnad.ToString();
                double moms = Kostnad * 0.1071;
                double m = Math.Ceiling(moms);
                textBoxMoms.Text = m.ToString();
                uAttBoka.Add(valdUtrustning);
                uBokning.Add(ub);
                LedigUtrustning.Remove(valdUtrustning);
                dataGridViewUtrustning.DataSource = null;
                dataGridViewUtrustning.DataSource = LedigUtrustning;
                dataGridViewUtrustning.Refresh();
                GridDesign();
            }
            catch
            {
                MessageBox.Show("Ingen utrustning vald. Vänligen välj utrustning att lägga till i bokningen och försök igen.", "Fel",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTaBortUt_Click(object sender, EventArgs e)
        {
            Utrustning valdUtrustning = dataGridViewUtrustning.SelectedRows[0].DataBoundItem as Utrustning;
            LedigUtrustning.Add(valdUtrustning);
            uAttBoka.Remove(valdUtrustning);
            dataGridViewUtrustning.DataSource = null;
            dataGridViewUtrustning.Refresh();
            dataGridViewUtrustning.DataSource = uAttBoka;
            GridDesign();
            UtrustningBokning ub = uBokning.FirstOrDefault(t => t.UtrustningId.Contains(valdUtrustning.Benämning));
            uBokning.Remove(ub);
            int kostnad = valdUtrustning.Pris ?? default(int);
            Kostnad -= kostnad;
            textBoxKostnad.Text = Kostnad.ToString();
            double moms = Kostnad * 0.1071;
            double m = Math.Ceiling(moms);
            textBoxMoms.Text = m.ToString();
        }

        private void radioButtonBokningsNr_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonPersorgNr_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void FyllBokningsInnehåll(Bokning hämtadBokning)
        {
            listBoxBokningar.Items.Clear();
            listBoxBokningar.Items.Add("Rum:");
            foreach (Rum r in rk.HämtaBokadeRum(hämtadBokning))
            {
                listBoxBokningar.Items.Add(r.Typ + "   " + r.RumsStorlek);
            }
            listBoxBokningar.Items.Add("\nUtrustning:");
            foreach (Utrustning u in uk.HämtaBokadUtrustning(hämtadBokning))
            {
                listBoxBokningar.Items.Add(u.Typ + "   " + "   " + u.UtrustningsArtikel);
            }
            listBoxBokningar.Items.Add("\nLektioner:");
            foreach (Lektion l in lk.HämtaBokadLektion(hämtadBokning))
            {
                listBoxBokningar.Items.Add(l.Typ + "   " + "   " + l.LektionStart);
            }
        }

        private void dataGridViewUtrustning_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
