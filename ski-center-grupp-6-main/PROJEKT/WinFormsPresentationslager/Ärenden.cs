using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{

    public partial class Ärenden : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        public List<Bokning> Bokningar { get; set; }
        public Bokning ValdBokning { get; set; }
        public FöretagsKund ValdKund { get; set; }
        public List<FöretagsKund> Fkunder { get; set; }
        public Ärenden(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
            Bokningar = HämtaBokningsÄrenden();
            FyllÄrenden();
            ValdBokning = new Bokning();
            ValdKund = new FöretagsKund();
            List<FöretagsKund> Fkunder = new List<FöretagsKund>();
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;
        }
        /// <summary>
        /// Fyller datagrid med ärenden
        /// </summary>
        private void FyllÄrenden()
        {
            dataGridView1.DataSource = Bokningar;
        }
        /// <summary>
        /// Hämtar alla bokningar som inte godkänts av marknadschef
        /// </summary>
        /// <returns></returns>
        private List<Bokning> HämtaBokningsÄrenden()
        {
            var lista = new List<Bokning>();

            foreach (Bokning b in bk.HittaBokningsBevakning())
            {
                lista.Add(b);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns inga befintliga bokningar på bevakning");
            }

            return lista;
        }
        /// <summary>
        /// Hämtar alla fkunder som inte godkänts av marknadschef än
        /// </summary>
        /// <returns></returns>
        private List<FöretagsKund> HämtaKunderÄrenden()
        {
            var lista = new List<FöretagsKund>();

            foreach (FöretagsKund f in kk.HittaKundBevakning())
            {
                lista.Add(f);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns inga befintliga företagskunder på bevakning");
            }

            return lista;
        }


        private void seÄrendenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
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


        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// klickar man på väljer fylls textrutorna med info (antingen med bokningar eller med kundinfo)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVälj_Click(object sender, EventArgs e)
        {

            if (panel7.Visible == false)
            {
                try
                {
                    ValdBokning = dataGridView1.SelectedRows[0].DataBoundItem as Bokning;
                    VisaBokningsInfo(ValdBokning);
                }
                catch
                {
                    MessageBox.Show("Ingen bokning har valts. Markera bokning i listan.");
                }

            }
            else
            {
                try
                {
                    ValdKund = dataGridView1.SelectedRows[0].DataBoundItem as FöretagsKund;
                    VisaKundInfo(ValdKund);
                }
                catch
                {
                    MessageBox.Show("Ingen kund har valts. Markera kund i listan.");
                }
            }

        }
        private void VisaKundInfo(FöretagsKund fKund)
        {
            textBoxfkmejl.Text = fKund.Mail;
            textBoxfktelenr.Text = fKund.TelefonNr.ToString();
            textBoxfkgata.Text = fKund.Adress;
            textBoxfkort.Text = fKund.Ort;
            textBoxfkpostnr.Text = fKund.PostNr.ToString();
            textBoxfkorgnr.Text = fKund.OrganisationsId.ToString();
            textBoxfknamn.Text = fKund.FöretagsNamn;
            textBoxAnställd.Text = fKund.AnställningsNr;
            textBoxKredgräns.Text = fKund.KreditGräns.ToString();

        }

        private void VisaBokningsInfo(Bokning valdBokning)
        {
            Faktura f = bk.HämtaFaktura(valdBokning);
            listBoxBokningar.Items.Clear();
            textBoxDatum.Text = valdBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + valdBokning.TillDatum.ToString("dd-MM-yyyy");
            textBoxAnställd.Text = valdBokning.AnställningsNr;
            textTele.Text = valdBokning.FKund.TelefonNr.ToString();
            textBoxKostnadmRM.Text = f.Summa.ToString();
            textBoxKostnadURM.Text = (f.Summa - f.Moms + f.Rabatt).ToString();
            textBoxRabatt.Text = f.Rabatt.ToString();
            textBoxKredgräns.Text = valdBokning.FKund.KreditGräns.ToString();
            textBoxMejl.Text = valdBokning.FKund.Mail;
            textBoxMoms.Text = f.Moms.ToString();
            textBoxNamn.Text = valdBokning.FKund.FöretagsNamn;
            textBoxOrgnr.Text = valdBokning.FKund.OrganisationsId.ToString();
            Kundnr.Visible = true;
            Kundnr.Text = valdBokning.FKund.FKundNr.ToString();
            Boknr.Visible = true;
            Boknr.Text = valdBokning.BokningsNr;
            if (valdBokning.Avbokningsskydd == true)
            {
                radioButtonAvbok.Checked = true;
            }
            listBoxBokningar.Items.Add("Rum:");
            foreach (Rum r in rk.HämtaBokadeRum(valdBokning))
            {
                listBoxBokningar.Items.Add(r.Typ + "   " + r.RumsStorlek);
            }
            listBoxBokningar.Items.Add("\nUtrustning:");
            foreach (Utrustning u in uk.HämtaBokadUtrustning(valdBokning))
            {
                listBoxBokningar.Items.Add(u.Typ + "   " + "   " + u.UtrustningsArtikel);
            }
            listBoxBokningar.Items.Add("\nLektioner:");
            foreach (Lektion l in lk.HämtaBokadLektion(valdBokning))
            {
                listBoxBokningar.Items.Add(l.Typ + "   " + "   " + l.LektionStart);
            }
        }
        /// <summary>
        /// Tar bort vissa kolumner i datagrid som inte behöver visas
        /// </summary>
        public void GridDesignKund()
        {
            dataGridView1.Columns["Aktiv"].Visible = false;
            dataGridView1.Columns["AnställningsNr"].Visible = false;
            dataGridView1.Columns["Personal"].Visible = false;
            dataGridView1.Columns["KreditGräns"].Visible = false;
        }
        /// <summary>
        /// Tar bort vissa kolumner i datagrid som inte behöver visas
        /// </summary>
        public void GridDesign()
        {
            dataGridView1.Columns["Aktiv"].Visible = false;
            dataGridView1.Columns["Godkänd"].Visible = false;
            dataGridView1.Columns["RumBokning"].Visible = false;
            dataGridView1.Columns["LektionBokning"].Visible = false;
            dataGridView1.Columns["UtrustningBokning"].Visible = false;
            dataGridView1.Columns["Personal"].Visible = false;
            dataGridView1.Columns["Avbokningsskydd"].Visible = false;
            dataGridView1.Columns["FKund"].Visible = false;
            dataGridView1.Columns["Kund"].Visible = false;
            dataGridView1.Columns["KundNr"].Visible = false;
            dataGridView1.AllowUserToOrderColumns = true;
            Bokningar.Sort(
            delegate (Bokning p1, Bokning p2)
            {
                int compare = p1.FrånDatum.CompareTo(p2.FrånDatum);
                if (compare == 0)
                {
                    return p2.BokningsNr.CompareTo(p1.BokningsNr);
                }
                return compare;
            });
        }

        private void Ärenden_Load(object sender, EventArgs e)
        {
            GridDesign();
        }
        /// <summary>
        /// Gör att marknadschef kan godkänna fkund eller fkundbokning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGodkänn_Click(object sender, EventArgs e)
        {

            string ändradKG = textBoxKredgräns.Text;
            if (dataGridView1.DataSource == Bokningar)
            {
                try
                {
                    ValdBokning = dataGridView1.SelectedRows[0].DataBoundItem as Bokning;
                    string ändradRabatt = textBoxRabatt.Text;
                    if (ändradRabatt == null)
                    {
                        ändradRabatt = "0";
                    }
                    bk.GodkännBokning(ValdBokning, ändradKG, ändradRabatt);
                    Bokningar.Remove(ValdBokning);
                    dataGridView1.DataSource = null;
                    dataGridView1.Refresh();
                    dataGridView1.DataSource = Bokningar;
                    GridDesign();

                    MessageBox.Show($"Bokning med bokningsnummer {ValdBokning.BokningsNr} har nu godkänts.");
                }
                catch
                {
                    MessageBox.Show("Ingen bokning har valts. Markera bokning i listan.");
                }

            }
            else
            {
                try
                {
                    ValdKund = dataGridView1.SelectedRows[0].DataBoundItem as FöretagsKund;
                    if (textBoxKredgräns.Text == "" || textBoxKredgräns.Text == "0")
                    {
                        MessageBox.Show("Fyll i kreditgräns och klicka på ändra innan du godkänner kund.");
                    }
                    else
                    {
                        kk.GodkännKund(ValdKund, ändradKG);
                        Fkunder.Remove(ValdKund);
                        dataGridView1.DataSource = null;
                        dataGridView1.Refresh();
                        dataGridView1.DataSource = Fkunder;
                        GridDesignKund();

                        MessageBox.Show($"Kund med kundnummer {ValdKund.FKundNr} har nu godkänts.");
                    }
                }
                catch
                {
                    MessageBox.Show("Ingen kund har valts. Markera kund i listan och klicka på välj innan du godkänner.");
                }


            }

        }

        /// <summary>
        /// Möjliggör för marknadschef att ta bort kund/bokning om den inte ska godkännas (ex marknads ringer kund som vill avbryta)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == Bokningar)
            {
                try
                {
                    ValdBokning = dataGridView1.SelectedRows[0].DataBoundItem as Bokning;
                    if (ValdBokning.FrånDatum < DateTime.Now)
                    {
                        MessageBox.Show("Bokningen har startat och går därför inte att avbryta.");
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort bokning med bokningsnr " +
                        $"{ValdBokning.BokningsNr}? Bokningen kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                        switch (dr)
                        {
                            case DialogResult.Yes:
                                bk.AvbrytBokning(ValdBokning);
                                MessageBox.Show($"Bokning med bokningsnr {ValdBokning.BokningsNr} har nu avbrutits");
                                Bokningar.Remove(ValdBokning);
                                dataGridView1.DataSource = null;
                                dataGridView1.Refresh();
                                dataGridView1.DataSource = Bokningar;
                                GridDesign();
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("Ingen bokning har valts. Markera bokning i listan.");
                }

            }
            else
            {
                try
                {
                    ValdKund = dataGridView1.SelectedRows[0].DataBoundItem as FöretagsKund;
                    if (Bokningar.Any(t => t.FKundNr.Equals(ValdKund.FKundNr)))
                    {
                        MessageBox.Show("Denna kund har en bokning kopplad till sig. För att ta bort kunden behöver bokningen avbrytas först.");
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort kund med kundnr " +
                       $"{ValdKund.FKundNr}? Kunder kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                        switch (dr)
                        {
                            case DialogResult.Yes:
                                kk.TaBortFöretagsKund(ValdKund);
                                MessageBox.Show($"Kund med kundnr {ValdKund.FKundNr} har nu raderats");
                                Fkunder.Remove(ValdKund);
                                dataGridView1.DataSource = null;
                                dataGridView1.Refresh();
                                dataGridView1.DataSource = Fkunder;
                                GridDesign();
                                break;
                            case DialogResult.No:
                                break;
                        }

                    }

                }
                catch
                {
                    MessageBox.Show("Ingen kund har valts. Markera kund i listan.");
                }
            }
        }
        /// <summary>
        /// ändrar vyn från fkundbokning till fkunder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonKunder_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            Fkunder = HämtaKunderÄrenden();
            dataGridView1.DataSource = Fkunder;
            buttonKunder.BackColor = SystemColors.ControlLightLight;
            buttonBok.BackColor = SystemColors.Control;
            GridDesignKund();
            panel7.Visible = true;
        }
        /// <summary>
        /// Ändrar vyn från bokningar till fkunder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBok_Click(object sender, EventArgs e)
        {
            HämtaBokningsÄrenden();
            buttonBok.BackColor = SystemColors.ControlLightLight;
            buttonKunder.BackColor = SystemColors.Control;
            FyllÄrenden();
            GridDesign();
            panel7.Visible = false;
        }

        private void textBoxfkmejl_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
