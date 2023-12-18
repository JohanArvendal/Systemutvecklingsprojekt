using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class Lektionsvy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        private DateTime fDatum = new DateTime();
        private DateTime tDatum = new DateTime();
        public List<Lektion> TillgängligLektion { get; set; }
        public List<Lektion> lAttBoka { get; set; }
        public List<Rum> rAttBoka { get; set; }
        public List<Utrustning> uAttBoka { get; set; }
        public List<UtrustningBokning> uBokning { get; set; }
        public List<RumBokning> rBokning { get; set; }
        public List<LektionBokning> lBokning { get; set; }
        public Kund HämtadKund { get; set; }
        public Bokning OavslutadBokning { get; set; }
        public Bokning BefintligBokning { get; set; }
        public FöretagsKund HämtadFöretagsKund { get; set; }
        public int Kostnad { get; set; }
        public Lektionsvy(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
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
            lAttBoka = new List<Lektion>();
            TillgängligLektion = new List<Lektion>();
            BefintligBokning = new Bokning();
            dateTimePickerStart.MinDate = DateTime.Now;
            dateTimePickerSlut.MinDate = DateTime.Now;

            LäggTillIBefintligBokning();
            PågåendeBokning();
        }
        private void LäggTillIBefintligBokning()
        {
            BefintligBokning = bk.HämtaBokning();
            if (BefintligBokning != null)
            {
                if (BefintligBokning.Kund != null)
                {
                    HämtadKund = BefintligBokning.Kund;
                    textBoxNamn.Text = BefintligBokning.Kund.FörNamn + " " + BefintligBokning.Kund.EfterNamn;
                    textBoxKredgräns.Text = BefintligBokning.Kund.KreditGräns.ToString();
                }
                else
                {
                    HämtadFöretagsKund = BefintligBokning.FKund;
                    textBoxNamn.Text = BefintligBokning.FKund.FöretagsNamn;
                    textBoxKredgräns.Text = BefintligBokning.FKund.KreditGräns.ToString();
                }
                textBoxResultat.Text = BefintligBokning.BokningsNr;
                textBoxPeriod.Text = BefintligBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + BefintligBokning.TillDatum.ToString("dd-MM-yyyy");
                radioButtonPK.Checked = true;
                dateTimePickerStart.MinDate = DateTime.Parse(BefintligBokning.FrånDatum.ToString("yyyy/MM/dd"));
                dateTimePickerSlut.Value = BefintligBokning.TillDatum;
                FyllBokningLista();
            }
        }
        private void PågåendeBokning()
        {
            OavslutadBokning = bk.HämtaPågåendeBokning();
            rAttBoka = bk.HämtaPågåendeRum();
            uAttBoka = uk.HämtaPågåendeUtrustning();
            uBokning = uk.HämtaPågåendeuBokning();
            lAttBoka = lk.HämtaPågåendeLektion();
            lBokning = lk.HämtaPågåendelBokning();
            rBokning = bk.HämtaPågåenderBokning();
            if (OavslutadBokning != null)
            {
                string bNr = "Bokningsnr inte skapad än";
                textBoxResultat.Text = bNr;
                if (OavslutadBokning.Kund != null)
                {
                    HämtadKund = OavslutadBokning.Kund;
                    textBoxNamn.Text = OavslutadBokning.Kund.FörNamn + " " + OavslutadBokning.Kund.EfterNamn;
                    textBoxKredgräns.Text = OavslutadBokning.Kund.KreditGräns.ToString();
                }
                else
                {
                    HämtadFöretagsKund = OavslutadBokning.FKund;
                    textBoxNamn.Text = OavslutadBokning.FKund.FöretagsNamn;
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

        private void FyllLista(List<Lektion> tillgängligLektion)
        {
            dataGridViewRum.DataSource = tillgängligLektion;

        }

        private void radioButtonpersnr_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonboknr_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSök_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Initierar hämtning av kund baserat på inmatad data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSök_Click(object sender, EventArgs e)
        {
            string input = textBoxSök.Text;
            if (radioButtonpersnr.Checked)
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
            else if (radioButtonboknr.Checked)
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
            textBoxNamn.Text = HämtadKund.FörNamn + " " + HämtadKund.EfterNamn;
            textBoxKredgräns.Text = HämtadKund.KreditGräns.ToString();
            textBoxResultat.Text = hämtadBokning.BokningsNr;
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
            if (radioButtonpersnr.Checked)
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
            else if (radioButtonboknr.Checked)
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
            textBoxNamn.Text = HämtadFöretagsKund.FöretagsNamn;
            textBoxKredgräns.Text = HämtadFöretagsKund.KreditGräns.ToString();
            textBoxResultat.Text = hämtadBokning.BokningsNr;
            textBoxPeriod.Text = hämtadBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + hämtadBokning.TillDatum.ToString("dd-MM-yyyy");
            radioButtonFK.Checked = true;
            FyllBokningsInnehåll(hämtadBokning);
        }

        private void textBoxResultat_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNamn_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxGata_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPnr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxOrt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTelenr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxmejl_TextChanged(object sender, EventArgs e)
        {

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
                MessageBox.Show("Obehörig användare", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void buttonUtrustning_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                new Utrustningsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                MessageBox.Show("Obehörig användare", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                MessageBox.Show("Ogiltig behörighet", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
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
                MessageBox.Show("Ogiltig behörighet", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void loggaUtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new LoggaIn(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void stängProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxUTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola")
            {

                comboBoxGrupp.Items.Clear();
                comboBoxGrupp.Items.Add("Alla");
                comboBoxGrupp.Items.Add("Grön");
                comboBoxGrupp.Items.Add("Blå");
                comboBoxGrupp.Items.Add("Röd");
                comboBoxGrupp.Items.Add("Svart");
            }
            else if (comboBoxLVisa.SelectedItem.ToString() == "Privatlektion")
            {
                comboBoxGrupp.Items.Clear();
                comboBoxGrupp.Items.Add("Privatlektion");
            }


        }

        private void Lektionsvy_Load(object sender, EventArgs e)
        {
            comboBoxGrupp.Items.Clear();
            comboBoxLVisa.Items.Add("Skidskola");
            comboBoxLVisa.Items.Add("Privatlektion");

        }

        private void buttonHämta_Click(object sender, EventArgs e)
        {
            fDatum = dateTimePickerStart.Value.Date;
            tDatum = dateTimePickerSlut.Value.Date;
            int start = (int)fDatum.DayOfWeek;
            int slut = (int)tDatum.DayOfWeek;
            if (start == 6 || start == 0 || slut == 6 || slut == 0)
            {
                MessageBox.Show("Lektioner kan inte bokas under helger.");
            }
            else
            {
                if (comboBoxLVisa.SelectedItem != null && comboBoxGrupp.SelectedItem != null)
                {

                    if (comboBoxLVisa.SelectedItem.ToString() == "Privatlektion")
                    {
                        string grupp = "Privatlektion";
                        UppdateraLista();

                        foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                        {
                            TillgängligLektion.Add(l);
                            int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                            l.Pris = dagspris;
                        }
                        FyllLista(TillgängligLektion);
                        GridDesign();
                    }
                    else if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola" && comboBoxGrupp.SelectedItem.ToString() == "Grön")
                    {

                        string grupp = "Grön";
                        if ((start == 1 || start == 4) && (slut == 3 || slut == 5))
                        {
                            UppdateraLista();


                            foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                            {
                                TillgängligLektion.Add(l);
                                int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                                l.Pris = dagspris;
                            }
                            FyllLista(TillgängligLektion);
                            GridDesign();

                        }
                        else
                        {
                            MessageBox.Show("Skidskola kan enbart bokas mån-ons eller tors-fre.");
                        }
                    }

                    else if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola" && comboBoxGrupp.SelectedItem.ToString() == "Alla")
                    {
                        string grupp = null;
                        if ((start == 1 || start == 4) && (slut == 3 || slut == 5))
                        {
                            UppdateraLista();


                            foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                            {
                                TillgängligLektion.Add(l);
                                int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                                l.Pris = dagspris;
                            }
                            FyllLista(TillgängligLektion);
                            GridDesign();

                        }
                        else
                        {
                            MessageBox.Show("Skidskola kan enbart bokas mån-ons eller tors-fre.");
                        }
                    }
                    else if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola" && comboBoxGrupp.SelectedItem.ToString() == "Röd")
                    {
                        string tillgänglig = null;
                        string grupp = "Röd";

                        if ((start == 1 || start == 4) && (slut == 3 || slut == 5))
                        {
                            UppdateraLista();


                            foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                            {
                                TillgängligLektion.Add(l);
                                int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                                l.Pris = dagspris;
                            }
                            FyllLista(TillgängligLektion);
                            GridDesign();

                        }
                        else
                        {
                            MessageBox.Show("Skidskola kan enbart bokas mån-ons eller tors-fre.");
                        }
                    }

                    else if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola" && comboBoxGrupp.SelectedItem.ToString() == "Blå")
                    {
                        string tillgänglig = null;
                        string grupp = "Blå";
                        if ((start == 1 || start == 4) && (slut == 3 || slut == 5))
                        {
                            UppdateraLista();


                            foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                            {
                                TillgängligLektion.Add(l);
                                int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                                l.Pris = dagspris;
                            }
                            FyllLista(TillgängligLektion);
                            GridDesign();

                        }
                        else
                        {
                            MessageBox.Show("Skidskola kan enbart bokas mån-ons eller tors-fre.");
                        }
                    }

                    else if (comboBoxLVisa.SelectedItem.ToString() == "Skidskola" && comboBoxGrupp.SelectedItem.ToString() == "Svart")
                    {
                        string tillgänglig = null;
                        string grupp = "Svart";
                        if ((start == 1 || start == 4) && (slut == 3 || slut == 5))
                        {
                            UppdateraLista();


                            foreach (Lektion l in lk.HämtaLedigLektion(grupp, fDatum, tDatum))
                            {
                                TillgängligLektion.Add(l);
                                int dagspris = lk.HämtaLektionPris(l.Typ, l.Grupp, fDatum, tDatum);
                                l.Pris = dagspris;
                            }
                            FyllLista(TillgängligLektion);
                            GridDesign();

                        }
                        else
                        {
                            MessageBox.Show("Skidskola kan enbart bokas mån-ons eller tors-fre.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vänligen fyll i alla fält i korrekt format för att söka efter lektion", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            
        }

        public void UppdateraLista()
        {
            dataGridViewRum.DataSource = null;
            TillgängligLektion.Clear();
        }

        private void buttonTillagda_Click(object sender, EventArgs e)
        {
            if (buttonTillagda.Text == "Tillagda")
            {
                dataGridViewRum.DataSource = lAttBoka;
                buttonTillagda.Text = "Tillgängliga";
                buttonTaBortLek.Visible = true;
                buttonLäggTill.Visible = false;
                GridDesign();
            }
            else if (buttonTillagda.Text == "Tillgängliga")
            {
                FyllLista(TillgängligLektion);
                GridDesign();
                buttonTillagda.Text = "Tillagda";
                buttonTaBortLek.Visible = false;
                buttonLäggTill.Visible = true;
            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            try
            {

                Lektion valdLektion = dataGridViewRum.SelectedRows[0].DataBoundItem as Lektion;
                string antal = textBoxAntalpers.Text;
                if (antal == "")
                {
                    MessageBox.Show("Fyll i antal personer som ska delta i lektionen");
                }
                else
                {
                    int antalpers = int.Parse(antal);

                    if (antalpers > valdLektion.AntalPlatser)
                    {
                        MessageBox.Show($"Lektion har bara plats för {valdLektion.AntalPlatser}. Välj en annan lektion eller tid.");
                    }
                    else
                    {
                        int a = valdLektion.AntalPlatser - antalpers;
                        valdLektion.AntalPlatser = a;
                        if (valdLektion.AntalPlatser == 0)
                        {
                            valdLektion.Tillgänglig = false;
                        }

                        if (lAttBoka == null)
                        {
                            lAttBoka = new List<Lektion>();
                        }
                        if (lBokning == null)
                        {
                            lBokning = new List<LektionBokning>();
                        }
                        int kostnad = valdLektion.Pris ?? default(int);
                        LektionBokning lb = new LektionBokning(valdLektion.Benämning, null, valdLektion, antalpers, fDatum, tDatum, kostnad);
                        lAttBoka.Add(valdLektion);
                        lBokning.Add(lb);
                        TillgängligLektion.Remove(valdLektion);
                        dataGridViewRum.DataSource = null;
                        dataGridViewRum.Refresh();
                        dataGridViewRum.DataSource = TillgängligLektion;
                        GridDesign();
                        Kostnad += kostnad * antalpers;

                        textBoxKostnad.Text = Kostnad.ToString();
                        double moms = Kostnad * 0.1071;
                        double m = Math.Ceiling(moms);
                        textBoxMoms.Text = m.ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ingen lektion vald.");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Lektion valdLektion = dataGridViewRum.SelectedRows[0].DataBoundItem as Lektion;

            TillgängligLektion.Add(valdLektion);
            lAttBoka.Remove(valdLektion);
            dataGridViewRum.DataSource = null;
            dataGridViewRum.Refresh();
            dataGridViewRum.DataSource = lAttBoka;
            GridDesign();
            LektionBokning lb = lBokning.FirstOrDefault(t => t.LektionId.Contains(valdLektion.Benämning));
            lBokning.Remove(lb);
            int kostnad = valdLektion.Pris ?? default(int);
            Kostnad -= kostnad;
            textBoxKostnad.Text = Kostnad.ToString();
            double moms = Kostnad * 0.1071;
            double m = Math.Ceiling(moms);
            textBoxMoms.Text = m.ToString();
        }

        private void buttonBUtrustning_Click(object sender, EventArgs e)
        {
            if ((HämtadKund == null && HämtadFöretagsKund == null) || lAttBoka == null)
            {
                MessageBox.Show("Lägg till lektion att boka eller sök fram kund innan du väljer att lägga till utrustning.");
            }
            else
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
                    bk.HämtaBokningsinnehåll(rAttBoka, lAttBoka, uAttBoka, BefintligBokning, fDatum, tDatum, HämtadKund, HämtadFöretagsKund, OavslutadBokning.Avbokningsskydd, uBokning, lBokning, rBokning, Kostnad);
                    new Utrustningsvy(pk, bk, kk, rk, uk, lk).Show();
                    this.Close();
                }
            }
        }

        private void buttonSlutför_Click(object sender, EventArgs e)
        {
            if ((HämtadKund == null && HämtadFöretagsKund == null) || lAttBoka == null)
            {
                MessageBox.Show("Ej möjligt att slutföra. Sök fram kund att lägga bokning på eller välj lektion att boka genom att markera lektion i listan och klicka på lägg till.");
            }
            else
            {

                int rabatt = 0;
                if (BefintligBokning == null)
                {
                    BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, fDatum, tDatum);
                }

                if (OavslutadBokning == null || BefintligBokning != null)
                {

                    if (BefintligBokning != null)
                    {

                        Bokning bokning = bk.LäggTillIBokning(lAttBoka, uAttBoka, rAttBoka, BefintligBokning, uBokning, lBokning, rBokning);
                        Faktura f = bk.HämtaFaktura(bokning);
                        if (bokning.Kund != null)
                        {
                            MessageBox.Show($"Bokning med bokningsnummer {bokning.BokningsNr} är nu kompletterad med lektion! \n\nBokningsdetaljer:" +
                            $"\nNamn: {HämtadKund.FörNamn} {HämtadKund.EfterNamn}\nKundnummer: {HämtadKund.KundNr}" +
                            $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal lektioner: {lAttBoka.Count}\n" +
                            $"Förfallodatum för debitering ett: { f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\n" +
                            $"Förfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}" +
                            $"\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}");
                        }
                        else
                        {
                            MessageBox.Show($"Bokning med bokningsnummer {bokning.BokningsNr} är nu kompletterad med lektion! \n\nBokningsdetaljer:" +
                            $"\nNamn: {HämtadFöretagsKund.FöretagsNamn}\nKundnummer: {HämtadFöretagsKund.FKundNr}" +
                            $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal lektioner: {lAttBoka.Count}\n" +
                            $"Förfallodatum för debitering ett: {f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\n" +
                            $"Förfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}");
                        }
                        new ValdBokningsvy(bk, bokning, kk, rk, uk, lk, pk).Show();
                    }
                    else
                    {
                        MessageBox.Show("Kund måste ha en befintlig logibokning för att kunna boka lektion");
                    }

                    foreach (Lektion l in lAttBoka)
                    {
                        foreach (LektionBokning lb in lBokning.Where(t => t.LektionId.Contains(l.Benämning)))
                        {
                            l.AntalPlatser += lb.AntalPersoner;
                        }

                    }
                    lBokning.Clear();
                    lAttBoka.Clear();

                    UppdateraLista();
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
                            $"\nPeriod: {OavslutadBokning.FrånDatum.ToString("dd-MM-yyyy")} - {OavslutadBokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\n" +
                            $"Förfallodatum för debitering ett: {OavslutadBokning.Faktura.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\n" +
                            $"Förfallodatum för debitering två: { OavslutadBokning.Faktura.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {avbok}");

                    }
                    else
                    {
                        MessageBox.Show($"Bokning är nu slutförd med bokningsnummer {OavslutadBokning.BokningsNr}! \n\nBokningsdetaljer:" +
                           $"\nNamn: {OavslutadBokning.FKund.FöretagsNamn}\nKundnummer: {OavslutadBokning.FKund.FKundNr}" +
                           $"\nPeriod: {OavslutadBokning.FrånDatum.ToString("dd-MM-yyyy")} - {OavslutadBokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\n" +
                           $"Förfallodatum för debitering ett: { OavslutadBokning.Faktura.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\nFörfallodatum för debitering två: { OavslutadBokning.Faktura.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\n" +
                           $"Efterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {avbok}");

                    }
                    new ValdBokningsvy(bk, OavslutadBokning, kk, rk, uk, lk, pk).Show();
                }



                UppdateraLista();
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
           
        }

        public void GridDesign()
        {
            dataGridViewRum.Columns["Tillgänglig"].Visible = false;
            dataGridViewRum.Columns["LektionBokning"].Visible = false;
            dataGridViewRum.Columns["Lärare"].Visible = false;
            dataGridViewRum.AllowUserToOrderColumns = true;
            TillgängligLektion.Sort(
            delegate (Lektion p1, Lektion p2)
            {
                int compare = p1.Typ.CompareTo(p2.Typ);
                if (compare == 0)
                {
                    return p2.Benämning.CompareTo(p1.Benämning);
                }
                return compare;
            });
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

        private void textBoxNamn_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerSlut_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonBRum_Click(object sender, EventArgs e)
        {

        }
    }
}
