using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class Kundregister : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        public List<Kund> VisaKund { get; set; }
        public List<FöretagsKund> VisaFöretagsKund { get; set; }

        public Kund Kund { get; set; }
        public FöretagsKund FöretagsKund { get; set; }
        public Kundregister(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
            FyllLista(VisaKund);
            FyllLista2(VisaFöretagsKund);
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;

        }
        /// <summary>
        /// Fyller gridviewn med kunder
        /// </summary>
        /// <param name="VisaKund"></param>
        private void FyllLista(List<Kund> VisaKund)
        {
            KundLista.DataSource = VisaKund;
        }
        /// <summary>
        /// Initierar hämtning av kunder från databas
        /// </summary>
        /// <returns></returns>
        private void FyllLista2(List<FöretagsKund> VisaFöretagsKund)
        {
            KundLista.DataSource = VisaFöretagsKund;
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

        }

        private void KundLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// Hämtar kunder baserat på val i combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visakunderknapp_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Ingen kund är vald i listan. Välj alternativ i rullmenyn och klicka på visa.");
            }
            else if (comboBox1.SelectedItem.ToString() == "Alla privatkunder")
            {
                var lista = new List<Kund>();

                foreach (Kund r in kk.HämtaKund())
                {
                    lista.Add(r);
                }
                if (lista == null)
                {
                    MessageBox.Show("Det finns inga registrerade kunder");
                }
                FyllLista(lista);

            }
            else if (comboBox1.SelectedItem.ToString() == "Alla företagskunder")
            {
                var lista = new List<FöretagsKund>();

                foreach (FöretagsKund r in kk.HämtaFöretagsKund())
                {
                    lista.Add(r);
                }
                if (lista == null)
                {
                    MessageBox.Show("Det finns inga registrerade företagskunder");
                }
                FyllLista2(lista);

            }
            else if (comboBox1.SelectedItem.ToString() == "Sök privatkund")
            {

                KundLista.DataSource = null;
                string sökPersNr = Sökruta.Text;
                if (Sökruta.Text == "")
                {
                    MessageBox.Show("Vänligen fyll i personnummer du vill söka fram kund på.");
                }
                else
                {
                    long persNrinput = long.Parse(sökPersNr);
                    VisaKund = new List<Kund>();
                    foreach (Kund k in kk.SökKunderPerPersonNr(persNrinput))
                    {
                        VisaKund.Add(k);
                    }


                    if (VisaKund != null)
                    {
                        KundLista.DataSource = VisaKund;
                    }
                    else
                    {
                        MessageBox.Show("Det finns ingen kund med angivet personnummer");
                    }
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "Sök företagskund")
            {

                KundLista.DataSource = null;
                string sökOrgId = Sökruta.Text;
                if (Sökruta.Text == "")
                {
                    MessageBox.Show("Vänligen fyll i personnummer du vill söka fram kund på.");
                }
                else
                {
                    long orgIdinput = long.Parse(sökOrgId);
                    VisaFöretagsKund = new List<FöretagsKund>();
                    foreach (FöretagsKund f in kk.SökFöretagsKunderPerOrganisationsNr(orgIdinput))
                    {
                        VisaFöretagsKund.Add(f);
                    }


                    if (VisaFöretagsKund != null)
                    {
                        KundLista.DataSource = VisaFöretagsKund;
                    }
                    else
                    {
                        MessageBox.Show("Det finns ingen kund med angivet personnummer");
                    }
                }

            }
            else
            {
                MessageBox.Show("Välj sökalternativ i rutan");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Fyller comboboxarna med alternativ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kundregister_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Alla privatkunder");
            comboBox1.Items.Add("Sök privatkund");
            comboBox1.Items.Add("Alla företagskunder");
            comboBox1.Items.Add("Sök företagskund");
        }
        private void Sökruta_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void buttonBokningar_Click(object sender, EventArgs e)
        {
            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2)
            {
                new LäggTillPrivatKund(pk, bk, kk, rk, uk, lk).Show();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 1)
            {
                new LäggTillFöretagsKund(pk, bk, kk, rk, uk, lk).Show();
            }
            else
            {
                MessageBox.Show("Obehörig användare: endast marknadschefen får lägga till företagskunder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        /// <summary>
        /// Tar bort kund så länge det inte finns en aktiv bokning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabortKnapp_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2)
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Ingen kund är vald i listan. Välj alternativ i rullmenyn och klicka på visa.");
                }
                else if (comboBox1.SelectedItem.ToString() == "Alla privatkunder" || comboBox1.SelectedItem.ToString() == "Sök privatkund")
                {

                    try
                    {
                        Kund = KundLista.SelectedRows[0].DataBoundItem as Kund;
                        FöretagsKund fk = new FöretagsKund();
                        Bokning bokning1 = bk.HittaKundBokning(Kund, fk);
                        if (bokning1 != null)
                        {
                            MessageBox.Show("Vald kund har en aktiv bokning och kan därför inte tas bort.");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort kund med kundnr " +
                               $"{Kund.KundNr}? Kunden kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    kk.TaBortKund(Kund);
                                    KundLista.DataSource = null;
                                    KundLista.Refresh();
                                    VisaKund = kk.HämtaKund();
                                    FyllLista(VisaKund);
                                    MessageBox.Show($"Kund har nu raderats från kundregistret.");
                                    break;
                                case DialogResult.No:
                                    break;
                            }

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen kund är vald i listan. Markera kund i listan.");
                    }
                }
                else
                {
                    try
                    {

                        FöretagsKund = KundLista.SelectedRows[0].DataBoundItem as FöretagsKund;
                        Bokning bokning1 = bk.HittaKundBokning(Kund, FöretagsKund);
                        if (bokning1 != null)
                        {
                            MessageBox.Show("Vald kund har en aktiv bokning och kan därför inte tas bort.");
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort kund med kundnr " +
                               $"{FöretagsKund.FKundNr}? Kunden kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    kk.TaBortFöretagsKund(FöretagsKund);
                                    KundLista.DataSource = null;
                                    KundLista.Refresh();
                                    VisaFöretagsKund = kk.HämtaFöretagsKund();
                                    FyllLista2(VisaFöretagsKund);
                                    MessageBox.Show($"Kund har nu raderats från kundregistret.");
                                    break;
                                case DialogResult.No:
                                    break;
                            }


                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen kund är vald i listan. Markera kund i listan.");
                    }

                }
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


        }
    }
}


