using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class Rumvy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        private DateTime fDatum = new DateTime();
        private DateTime tDatum = new DateTime();
        public List<Rum> LedigaRum { get; set; }
        public List<Rum> rAttBoka { get; set; }
        public List<Lektion> lAttBoka { get; set; }
        public List<Utrustning> uAttBoka { get; set; }
        public List<UtrustningBokning> uBokning { get; set; }
        public List<RumBokning> rBokning { get; set; }
        public List<LektionBokning> lBokning { get; set; }
        public Bokning OavslutadBokning { get; set; }
        public Kund HämtadKund { get; set; }
        public Bokning BefintligBokning { get; set; }
        public FöretagsKund HämtadFöretagsKund { get; set; }
        public int Kostnad { get; set; }
        public Rumvy(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();

            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;

            FyllLista(LedigaRum);
            rAttBoka = new List<Rum>();
            BefintligBokning = new Bokning();
            LedigaRum = new List<Rum>();
            dateTimePickerStart.MinDate = DateTime.Now;
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;
            LäggTillIBefintligBokning();

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
                    textBoxNamn.Text = BefintligBokning.Kund.FörNamn;
                    textBoxEfternamn.Text = BefintligBokning.Kund.EfterNamn;
                    textBoxKredgräns.Text = BefintligBokning.Kund.KreditGräns.ToString();
                    radioButtonPrivat.Checked = true;
                    textBoxGata.Text = BefintligBokning.Kund.Adress;
                    textBoxmejl.Text = BefintligBokning.Kund.Mail;
                    textBoxOrt.Text = BefintligBokning.Kund.Ort;
                    textBoxPnr.Text = BefintligBokning.Kund.PostNr.ToString();
                    textBoxResultat.Text = BefintligBokning.Kund.PersonNr.ToString();
                    textBoxTelenr.Text = BefintligBokning.Kund.TelefonNr.ToString();
                }
                else
                {
                    HämtadFöretagsKund = BefintligBokning.FKund;
                    textBoxNamn.Text = BefintligBokning.FKund.FöretagsNamn;
                    textBoxKredgräns.Text = BefintligBokning.FKund.KreditGräns.ToString();
                    radioButtonFöretag.Checked = true;
                    textBoxGata.Text = BefintligBokning.FKund.Adress;
                    textBoxmejl.Text = BefintligBokning.FKund.Mail;
                    textBoxOrt.Text = BefintligBokning.FKund.Ort;
                    textBoxPnr.Text = BefintligBokning.FKund.PostNr.ToString();
                    textBoxResultat.Text = BefintligBokning.FKund.OrganisationsId.ToString();
                    textBoxTelenr.Text = BefintligBokning.FKund.TelefonNr.ToString();
                }
                dateTimePickerStart.MinDate = BefintligBokning.FrånDatum;
                dateTimePickerSlut.Value = BefintligBokning.TillDatum;

            }
        }

        /// <summary>
        /// Fyller datagriden med tillgängliga rum
        /// </summary>
        /// <param name="ledigaRum"></param>
        private void FyllLista(List<Rum> ledigaRum)
        {
            dataGridViewRum.DataSource = LedigaRum;

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerSlut_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonKonferens_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonLogi_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewRum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonRum_Click(object sender, EventArgs e)
        {

        }

        private void buttonBokningar_Click(object sender, EventArgs e)
        {
            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
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

        /// <summary>
        /// fyller combobox med alternativ och tidsformat på datetimepicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rumvy_Load(object sender, EventArgs e)
        {
            comboBoxTyp.Items.Add("Alla");
            comboBoxTyp.Items.Add("Logi");
            comboBoxTyp.Items.Add("Konferens");
            comboBoxTyp.Items.Add("Camp");

            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = "MM/dd/yyyy hh:mm";

            dateTimePickerSlut.Format = DateTimePickerFormat.Custom;
            dateTimePickerSlut.CustomFormat = "MM/dd/yyyy hh:mm";

        }

        /// <summary>
        /// När användare klickar på Hämta initieras hämtning av lediga rum baserat på vald data i comboboxarna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKonferens_Click(object sender, EventArgs e)
        {

            fDatum = dateTimePickerStart.Value;
            tDatum = dateTimePickerSlut.Value;

            if ((fDatum - tDatum).Days < -7)
            {
                MessageBox.Show("Rum kan enbart bokas i max en vecka.");
                UppdateraLista();

            }
            else
            {
                int vecka = RumKontroller.HämtaVecka(fDatum);

                if (LedigaRum != null)
                {
                    UppdateraLista();
                }

                if (comboBoxTyp.SelectedItem != null && comboBoxStorlek.SelectedItem != null)
                {



                    if (comboBoxTyp.SelectedItem.ToString() == "Logi" && comboBoxStorlek.SelectedItem.ToString() == "LGH1")
                    {
                        string storlek = "LGH1";

                        foreach (Rum r in rk.HittaLedigLogi(fDatum, tDatum, storlek))
                        {
                            LedigaRum.Add(r);
                        }
                        if ((fDatum - tDatum).Days == -7)
                        {

                            int veckopris = rk.HämtaLogiPris(vecka, 7, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = veckopris;
                            }
                        }
                        else if ((fDatum - tDatum).Days < 7)
                        {
                            if (vecka == 7 || vecka == 8)
                            {
                                MessageBox.Show("Logi kan inte bokas för dygn under vecka 7 och 8. Välj att boka vecka eller välj andra datum.");
                            }
                            else
                            {
                                int dygnpris = rk.HämtaLogiPris(vecka, 1, storlek, fDatum, tDatum);
                                foreach (Rum l in LedigaRum)
                                {
                                    l.Pris = dygnpris;
                                }
                            }

                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }
                    }
                    if (comboBoxTyp.SelectedItem.ToString() == "Camp" && comboBoxStorlek.SelectedItem.ToString() == "Camp")
                    {
                        string storlek = "Camp";

                        foreach (Rum r in rk.HittaLedigLogi(fDatum, tDatum, storlek))
                        {
                            LedigaRum.Add(r);
                        }
                        if ((fDatum - tDatum).Days == -7)
                        {

                            int veckopris = rk.HämtaKonferensPris(vecka, 7, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = veckopris;
                            }
                        }
                        else if ((fDatum - tDatum).Days < 7)
                        {
                            int dygnpris = rk.HämtaKonferensPris(vecka, 1, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = dygnpris;
                            }
                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }
                    }
                    if (comboBoxTyp.SelectedItem.ToString() == "Alla" && comboBoxStorlek.SelectedItem.ToString() == "Alla")
                    {

                        foreach (Rum r in rk.HittaLedigaRum(fDatum, tDatum))
                        {
                            LedigaRum.Add(r);
                            if ((fDatum - tDatum).Days == -7)
                            {
                                if (r.RumsStorlek.Contains("LGH1") || r.RumsStorlek.Contains("LGH2"))
                                {
                                    int veckopris = rk.HämtaLogiPris(vecka, 7, r.RumsStorlek, fDatum, tDatum);
                                    r.Pris = veckopris;
                                }
                                else
                                {
                                    int veckopris = rk.HämtaKonferensPris(vecka, 7, r.RumsStorlek, fDatum, tDatum);
                                    r.Pris = veckopris;
                                }


                            }
                            else if ((fDatum - tDatum).Days < 7)
                            {
                                if (r.RumsStorlek.Contains("LGH1") || r.RumsStorlek.Contains("LGH2"))
                                {
                                    if (vecka == 7 || vecka == 8)
                                    {
                                        MessageBox.Show("Logi kan inte bokas för dygn under vecka 7 och 8. Välj att boka vecka eller välj andra datum.");
                                    }
                                    else
                                    {
                                        int dygnpris = rk.HämtaLogiPris(vecka, 1, r.RumsStorlek, fDatum, tDatum);
                                        r.Pris = dygnpris;
                                    }
                                }
                                else
                                {
                                    int dygnpris = rk.HämtaKonferensPris(vecka, 1, r.RumsStorlek, fDatum, tDatum);
                                    r.Pris = dygnpris;
                                }


                            }
                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }
                    }
                    else if (comboBoxTyp.SelectedItem.ToString() == "Logi" && comboBoxStorlek.SelectedItem.ToString() == "LGH2")
                    {
                        string storlek = "LGH2";

                        foreach (Rum r in rk.HittaLedigLogi(fDatum, tDatum, storlek))
                        {
                            LedigaRum.Add(r);
                        }
                        if ((fDatum - tDatum).Days == -7)
                        {

                            int veckopris = rk.HämtaLogiPris(vecka, 7, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = veckopris;
                            }
                        }
                        else if ((fDatum - tDatum).Days < 7)
                        {

                            if (vecka == 7 || vecka == 8)
                            {
                                MessageBox.Show("Logi kan inte bokas för dygn under vecka 7 och 8. Välj att boka vecka eller välj andra datum.");
                            }
                            else
                            {
                                int dygnpris = rk.HämtaLogiPris(vecka, 1, storlek, fDatum, tDatum);
                                foreach (Rum l in LedigaRum)
                                {
                                    l.Pris = dygnpris;
                                }
                            }
                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }

                    }
                    else if (comboBoxTyp.SelectedItem.ToString() == "Konferens" && comboBoxStorlek.SelectedItem.ToString() == "20 platser")
                    {
                        string storlek = "20 platser";
                        foreach (Rum r in rk.HittaLedigKonferens(fDatum, tDatum, storlek))
                        {
                            LedigaRum.Add(r);
                        }
                        if ((fDatum - tDatum).Days == -7)
                        {

                            int veckopris = rk.HämtaKonferensPris(vecka, 7, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = veckopris;
                            }
                        }
                        else if ((fDatum - tDatum).Days < 7)
                        {

                            int dygnpris = rk.HämtaKonferensPris(vecka, 1, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = dygnpris;
                            }
                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }

                    }
                    else if (comboBoxTyp.SelectedItem.ToString() == "Konferens" && comboBoxStorlek.SelectedItem.ToString() == "50 platser")
                    {
                        string storlek = "50 platser";
                        foreach (Rum r in rk.HittaLedigKonferens(fDatum, tDatum, storlek))
                        {
                            LedigaRum.Add(r);
                        }
                        if ((fDatum - tDatum).Days == -7)
                        {

                            int veckopris = rk.HämtaKonferensPris(vecka, 7, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = veckopris;
                            }
                        }
                        else if ((fDatum - tDatum).Days < 7)
                        {

                            int dygnpris = rk.HämtaKonferensPris(vecka, 1, storlek, fDatum, tDatum);
                            foreach (Rum l in LedigaRum)
                            {
                                l.Pris = dygnpris;
                            }
                        }
                        FyllLista(LedigaRum);
                        if (LedigaRum != null)
                        {
                            GridDesign();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Vänligen fyll i alla fält i korrekt format för att söka efter rum", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }



            }



        }
        /// <summary>
        /// Uppdaterar datagrid så att den tar bort rummen som visas innan användaren initierar att hämta logi/konferensrum via att klicka på hämta
        /// </summary>
        public void UppdateraLista()
        {
            fDatum = dateTimePickerStart.Value;
            tDatum = dateTimePickerSlut.Value;
            dataGridViewRum.DataSource = null;
            LedigaRum.Clear();

        }
        /// <summary>
        /// Tar bort vissa fält i gridview som inte behöver/ska visas
        /// </summary>
        public void GridDesign()
        {
            dataGridViewRum.Columns["Tillgänglig"].Visible = false;
            dataGridViewRum.Columns["Beskrivning"].Visible = false;
            dataGridViewRum.Columns["RumBokning"].Visible = false;
            dataGridViewRum.Columns["Prestanda"].Visible = false;
            dataGridViewRum.AllowUserToOrderColumns = true;
            LedigaRum.Sort(
            delegate (Rum p1, Rum p2)
            {
                int compare = p1.RumsNr.CompareTo(p2.RumsNr);
                if (compare == 0)
                {
                    return p2.Benämning.CompareTo(p1.Benämning);
                }
                return compare;
            });
        }
        /// <summary>
        /// Skickar valt rum i listan vidare till ett nytt fönster för att visa fler detaljer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDetalj_Click(object sender, EventArgs e)
        {
            try
            {
                Rum valtRum = dataGridViewRum.SelectedRows[0].DataBoundItem as Rum;
                new ValdRumVy(pk, bk, kk, rk, uk, lk, valtRum, fDatum).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vänligen välj ett rum", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }

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
        /// Söker fram kund baserat på person/orgnummer eller bokningssnummer
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
            bool aktiv = true;
            textBoxGata.Text = HämtadKund.Adress;
            textBoxmejl.Text = HämtadKund.Mail;
            textBoxNamn.Text = HämtadKund.FörNamn;
            textBoxEfternamn.Text = HämtadKund.EfterNamn;
            textBoxOrt.Text = HämtadKund.Ort;
            textBoxPnr.Text = HämtadKund.PostNr.ToString();
            textBoxTelenr.Text = HämtadKund.TelefonNr.ToString();
            textBoxKredgräns.Text = HämtadKund.KreditGräns.ToString();
            textBoxResultat.Text = HämtadKund.PersonNr.ToString();
            radioButtonPrivat.Checked = true;
            Bokning b = bk.SökAktuellKundBokning(HämtadKund);
            if (b != null)
            {
                dateTimePickerStart.MinDate = b.FrånDatum;
                dateTimePickerSlut.Value = b.TillDatum;
            }



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
            textBoxGata.Text = HämtadFöretagsKund.Adress;
            textBoxmejl.Text = HämtadFöretagsKund.Mail;
            textBoxNamn.Text = HämtadFöretagsKund.FöretagsNamn;
            textBoxOrt.Text = HämtadFöretagsKund.Ort;
            textBoxPnr.Text = HämtadFöretagsKund.PostNr.ToString();
            textBoxTelenr.Text = HämtadFöretagsKund.TelefonNr.ToString();
            textBoxKredgräns.Text = HämtadFöretagsKund.KreditGräns.ToString();
            textBoxResultat.Text = HämtadFöretagsKund.OrganisationsId.ToString();
            radioButtonFöretag.Checked = true;
            Bokning b = bk.SökAktuellFKundBokning(HämtadFöretagsKund);
            if (b != null)
            {
                dateTimePickerStart.Value = b.FrånDatum;
                dateTimePickerSlut.Value = b.TillDatum;
            }
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

        private void radioButtonavbokskydd_CheckedChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Slutför bokning och skickar vidare för att lägga in i databas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSlutför_Click(object sender, EventArgs e)
        {
            List<Lektion> lektionAttBoka = new List<Lektion>();
            List<Utrustning> utrustningAttBoka = new List<Utrustning>();

            var kontrolleraRum = rAttBoka.Any(b => b.Typ.Contains("Logi") || b.Typ.Contains("Camp"));
            if (BefintligBokning == null)
            {
                BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, tDatum, fDatum);
            }


            if (kontrolleraRum == true)
            {
                if (BefintligBokning != null)
                {
                    KompletteraBokning(BefintligBokning, lektionAttBoka, utrustningAttBoka);
                }
                else
                {
                    int rabatt = 0;
                    bool avbokSkydd = false;
                    string boknr = null;
                    string printAvbok = "Nej";

                    if (radioButtonavbokskydd.Checked)
                    {
                        avbokSkydd = true;
                        printAvbok = "Ja";
                    }
                    Bokning bokning = bk.SkapaBokning(rAttBoka, HämtadKund, HämtadFöretagsKund, fDatum, tDatum, lektionAttBoka, utrustningAttBoka, rabatt, avbokSkydd, boknr, uBokning, lBokning, rBokning);
                    Faktura f = bk.HämtaFaktura(bokning);
                    if (HämtadKund != null)
                    {
                        MessageBox.Show($"Bokning är nu slutförd med bokningsnummer {bokning.BokningsNr}! \n\nBokningsdetaljer:" +
                   $"\nNamn: {HämtadKund.FörNamn} {HämtadKund.EfterNamn}\nKundnummer: {HämtadKund.KundNr}" +
                   $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\nFörfallodatum för debitering ett: { f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\n" +
                                    $"Förfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {printAvbok}");
                    }
                    else
                    {
                        MessageBox.Show($"Bokning är nu slutförd med bokningsnummer {bokning.BokningsNr}! \n\nBokningsdetaljer:" +
                   $"\nNamn: {HämtadFöretagsKund.FöretagsNamn}\nKundnummer: {HämtadFöretagsKund.FKundNr}" +
                   $"\nPeriod: {bokning.FrånDatum.ToString("dd-MM-yyyy")} - {bokning.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count}\nFörfallodatum för debitering ett: { f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy")}\nFörskottsbelopp: {f.DelBelopp}\n" +
                                    $"Förfallodatum för debitering två: { f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}\nEfterskottsbelopp: {f.Summa - f.DelBelopp}\nTotalsumma: {f.Summa}\nAvbokningsskydd: {printAvbok}");
                    }
                    new ValdBokningsvy(bk, bokning, kk, rk, uk, lk, pk).Show();
                }
            }
            else if (kontrolleraRum == false)
            {
                if (BefintligBokning == null)
                {
                    MessageBox.Show("Kund måste ha en aktiv logibokning under dessa datum för att kunna boka konferensrum. Boka logi och försök igen");
                }
                else
                {
                    KompletteraBokning(BefintligBokning, lektionAttBoka, utrustningAttBoka);
                }
            }
            else
            {
                MessageBox.Show("Inget rum valt. Välj rum");
            }
            rAttBoka.Clear();
            rBokning.Clear();
            UppdateraLista();
            FyllLista(LedigaRum);

        }
        /// <summary>
        /// Om aktiv bokning finns för kund lägger denna metod till valda rum i aktiv bokning
        /// </summary>
        /// <param name="check"></param>
        /// <param name="lektionAttBoka"></param>
        /// <param name="utrustningAttBoka"></param>
        public void KompletteraBokning(Bokning check, List<Lektion> lektionAttBoka, List<Utrustning> utrustningAttBoka)
        {
            Faktura f = bk.HämtaFaktura(check);


            bk.LäggTillIBokning(lektionAttBoka, utrustningAttBoka, rAttBoka, check, uBokning, lBokning, rBokning);

            if (HämtadFöretagsKund == null)
            {
                MessageBox.Show($"Rum är nu tillagt i bokning {check.BokningsNr}! \n\nBokningsdetaljer:" +
                  $"\nNamn: {HämtadKund.FörNamn} {HämtadKund.EfterNamn}\nKundnummer: {HämtadKund.KundNr}" +
                  $"\nPeriod: {check.FrånDatum.ToString("dd-MM-yyyy")} - {check.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count + check.RumBokning.Count}\nTillagd kostnad: {Kostnad}\nTotal kostnad: {f.Summa}");
            }
            else if (HämtadKund == null)
            {
                MessageBox.Show($"Rum är nu tillagt i bokning {check.BokningsNr}! \n\nBokningsdetaljer:" + $"\nNamn: {HämtadFöretagsKund.FöretagsNamn} \nKundnummer: {HämtadFöretagsKund.FKundNr}" +
                  $"\nPeriod: {check.FrånDatum.ToString("dd-MM-yyyy")} - {check.TillDatum.ToString("dd-MM-yyyy")}\nAntal rum: {rAttBoka.Count + check.RumBokning.Count}\nTillagd kostnad: {Kostnad}\nTotal kostnad: {f.Summa}");
            }
            new ValdBokningsvy(bk, BefintligBokning, kk, rk, uk, lk, pk).Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxRabatt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMoms_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxKostnad_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonPrivat_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonFöretag_CheckedChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Lägga till lektion i pågående bokning. Skickar pågående rum vidare till property i kontroller för att man ska kunna 
        /// hämta det och fortsätta med bokning i Lektionsgränssnittet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBLektion_Click(object sender, EventArgs e)
        {
            var kontrolleraRum = rAttBoka.Any(b => b.Typ.Contains("Logi"));
            if (HämtadKund != null && HämtadFöretagsKund != null)
            {
                BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, tDatum, fDatum);
            }
            else
            {
                MessageBox.Show("Ingen pågående bokning har valts. Välj en bokning, eller boka lektion via lektionssidan.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (kontrolleraRum == false)
            {
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
            else
            {
                bool avbok = false;
                if (BefintligBokning == null)
                {
                    if (radioButtonavbokskydd.Checked == true)
                    {
                        avbok = true;
                    }
                }
                bk.HämtaBokningsinnehåll(rAttBoka, lAttBoka, uAttBoka, BefintligBokning, fDatum, tDatum, HämtadKund, HämtadFöretagsKund, avbok, uBokning, lBokning, rBokning, Kostnad);
                new Lektionsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
        }
        /// <summary>
        /// Lägga till utrustning i pågående bokning. Skickar pågående rum vidare till property i kontroller för att man ska kunna 
        /// hämta det och fortsätta med bokning i utrustningsgränssnittet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHutrustning_Click(object sender, EventArgs e)
        {



            var kontrolleraRum = rAttBoka.Any(b => b.Typ.Contains("Logi"));

            if (HämtadKund == null && HämtadFöretagsKund == null)
            {
                MessageBox.Show("Ingen kund eller bokning vald. Om du vill boka utrustning för en ny/befintlig kund, vänligen välj en bokning eller gå till utrustningssidan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                BefintligBokning = rk.KontrolleraRumsbokning(HämtadKund, HämtadFöretagsKund, tDatum, fDatum);

            }


            if (kontrolleraRum == false)
            {
                if (BefintligBokning == null)
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
                    new Utrustningsvy(pk, bk, kk, rk, uk, lk).Show();
                    this.Close();
                }

            }
            else
            {
                bool avbok = false;
                if (BefintligBokning == null)
                {
                    if (radioButtonavbokskydd.Checked == true)
                    {
                        avbok = true;
                    }
                }
                bk.HämtaBokningsinnehåll(rAttBoka, lAttBoka, uAttBoka, BefintligBokning, fDatum, tDatum, HämtadKund, HämtadFöretagsKund, avbok, uBokning, lBokning, rBokning, Kostnad);
                new Utrustningsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
        }
    

            
        

        private void ärendenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loggaUtToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void loggaUtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new LoggaIn(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void stängProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// Fyller comboboxarna med alternativ baserat på vald rumstyp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTyp.SelectedItem.ToString() == "Konferens")
            {
                comboBoxStorlek.Items.Clear();
                comboBoxStorlek.Items.Add("20 platser");
                comboBoxStorlek.Items.Add("50 platser");
            }
            else if (comboBoxTyp.SelectedItem.ToString() == "Logi")
            {
                comboBoxStorlek.Items.Clear();
                comboBoxStorlek.Items.Add("LGH1");
                comboBoxStorlek.Items.Add("LGH2");
            }
            else if (comboBoxTyp.SelectedItem.ToString() == "Camp")
            {
                comboBoxStorlek.Items.Clear();
                comboBoxStorlek.Items.Add("Camp");
            }
            else if (comboBoxTyp.SelectedItem.ToString() == "Alla")
            {
                comboBoxStorlek.Items.Clear();
                comboBoxStorlek.Items.Add("Alla");

            }
        }

        private void comboBoxStorlek_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Lägger markerat rum i listan i tillagda rum för bokning och tar bort rummet från tillgängliga rum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Rum valtRum = dataGridViewRum.SelectedRows[0].DataBoundItem as Rum;
                if (rAttBoka == null)
                {
                    rAttBoka = new List<Rum>();
                }
                if (rBokning == null)
                {
                    rBokning = new List<RumBokning>();
                }
                int kostnad = valtRum.Pris ?? default(int);
                RumBokning rb = new RumBokning(valtRum.RumsNr, null, valtRum, fDatum, tDatum, kostnad);
                rAttBoka.Add(valtRum);
                rBokning.Add(rb);
                LedigaRum.Remove(valtRum);
                dataGridViewRum.DataSource = null;
                dataGridViewRum.Refresh();
                dataGridViewRum.DataSource = LedigaRum;
                GridDesign();
                Kostnad += kostnad;
                textBoxKostnad.Text = Kostnad.ToString();
                double moms = Kostnad * 0.1071;
                double m = Math.Ceiling(moms);
                textBoxMoms.Text = m.ToString();
            }
            catch
            {
                MessageBox.Show("Vänligen välj ett rum", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// gör att användaren kan byta mellan lista rum att boka och valda rum (så att de kan boka flera samtidigt)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Tillagda")
            {
                dataGridViewRum.DataSource = rAttBoka;
                button3.Text = "Tillgängliga";
                buttonTaBort.Visible = true;
                buttonLäggTill.Visible = false;
                GridDesign();
            }
            else if (button3.Text == "Tillgängliga")
            {
                FyllLista(LedigaRum);
                GridDesign();
                button3.Text = "Tillagda";
                buttonTaBort.Visible = false;
                buttonLäggTill.Visible = true;
            }
        }
        /// <summary>
        /// Tar bort rum från listan tillagda rum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            Rum valtRum = dataGridViewRum.SelectedRows[0].DataBoundItem as Rum;
            LedigaRum.Add(valtRum);
            rAttBoka.Remove(valtRum);
            dataGridViewRum.DataSource = null;
            dataGridViewRum.Refresh();
            dataGridViewRum.DataSource = rAttBoka;
            GridDesign();
            RumBokning rb = rBokning.FirstOrDefault(t => t.RumId.Equals(valtRum.RumsNr));
            rBokning.Remove(rb);
            int kostnad = valtRum.Pris ?? default(int);
            Kostnad -= kostnad;
            textBoxKostnad.Text = Kostnad.ToString();
            double moms = Kostnad * 0.1071;
            double m = Math.Ceiling(moms);
            textBoxMoms.Text = m.ToString();
        }
        /// <summary>
        /// Lägger till ny kund innan man bokar rum ifall kunden inte finns i kundregistret
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLäggTillKund_Click(object sender, EventArgs e)
        {
            try
            {


                string gata = textBoxGata.Text;
                string mejl = textBoxmejl.Text;
                string fnamn = textBoxNamn.Text;
                string enamn = textBoxEfternamn.Text;
                string ort = textBoxOrt.Text;
                string posttext = textBoxPnr.Text;

                string teletext = textBoxTelenr.Text;

                string perstext = textBoxResultat.Text;


                long telenr = long.Parse(teletext);
                long persnr = long.Parse(perstext);
                int postnr = int.Parse(posttext);




                if (gata == null || mejl == null || fnamn == null || ort == null || posttext == null || teletext == null || perstext == null)
                {
                    MessageBox.Show("Vänligen fyll i alla fält i Sök kund.");
                }
                else
                {
                    if (radioButtonPrivat.Checked == true)
                    {
                        int kredgräns = 12000;

                        if (enamn == null)
                        {
                            MessageBox.Show("Vänligen fyll Efternamn.");
                        }
                        Kund kund = kk.LäggTillKund(gata, mejl, fnamn, enamn, ort, postnr, telenr, kredgräns, persnr);

                        MessageBox.Show($"Kund med kundnummer {kund.KundNr} har lagts till i kundregister. Du kan nu boka logi");
                        HämtadKund = kund;
                        textBoxKredgräns.Text = HämtadKund.KreditGräns.ToString();
                    }
                    else if (radioButtonFöretag.Checked == true)
                    {
                        int kredgräns = 0;
                        FöretagsKund fKund = kk.LäggTillFKund(gata, mejl, fnamn, ort, postnr, telenr, kredgräns, persnr);

                        MessageBox.Show($"Registrering av Kund med kundnummer {fKund.FKundNr} har nu skickats vidare på bevakning. Det går däremot att lägga logibokning på bevakning redan nu");
                        HämtadFöretagsKund = fKund;
                    }
                    else
                    {
                        MessageBox.Show("Vänligen bocka i privatkund eller företagskund");
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Vänligen fyll i alla fält i korrekt format för att söka efter en kund", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);

            }



        }

        private void textBoxEfternamn_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerTid_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
