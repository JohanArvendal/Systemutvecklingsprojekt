using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class Bokningsvy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        public List<Bokning> BefintligaBokningar { get; set; }
        private DateTime fDatum = new DateTime();
        private DateTime tDatum = new DateTime();
        public Bokning ValdBokning { get; set; }
        
        public Bokningsvy(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;

            radioButtonAktiva.Checked = true;
            bool aktiv = true;
            BefintligaBokningar = BefintligaBokning(aktiv);
            FyllLista();
            GridDesign();
            menuStrip1.BackColor = Color.FromArgb(0, 102, 204);
            menuStrip1.ForeColor = Color.White;
        }
        /// <summary>
        /// Fyller gridview med alla befintliga bokningar
        /// </summary>
        /// <param name="BefintligaBokningar"></param>
        private void FyllLista()
        {
            dataGridViewBokningar.DataSource = BefintligaBokningar;
        }
        /// <summary>
        /// Metod för att initiera hämtning av bokningar
        /// </summary>
        /// <returns></returns>
        private List<Bokning> BefintligaBokning(bool aktiv)
        {

            var lista = new List<Bokning>();

            foreach (Bokning r in bk.HämtaAllaBokningar(aktiv))
            {
                lista.Add(r);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns inga befintliga bokningar");
            }

            return lista;
        }
        /// <summary>
        /// Tar bort vissa kolumner som inte ska visas i gridviewen
        /// </summary>
        public void GridDesign()
        {
            dataGridViewBokningar.Columns["Personal"].Visible = false;
            dataGridViewBokningar.Columns["RumBokning"].Visible = false;
            dataGridViewBokningar.Columns["Kund"].Visible = false;
            dataGridViewBokningar.Columns["Godkänd"].Visible = false;
            dataGridViewBokningar.Columns["Aktiv"].Visible = false;
            dataGridViewBokningar.Columns["LektionBokning"].Visible = false;
            dataGridViewBokningar.Columns["UtrustningBokning"].Visible = false;
            dataGridViewBokningar.Columns["RumBokning"].Visible = false;
            dataGridViewBokningar.Columns["RumBokning"].Visible = false;
            dataGridViewBokningar.Columns["Avbokningsskydd"].Visible = false;
            dataGridViewBokningar.Columns["Faktura"].Visible = false;
            dataGridViewBokningar.Columns["Kund"].Visible = false;
            dataGridViewBokningar.Columns["FKund"].Visible = false;
        }
        private void buttonRum_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2) // se databas för info om vilken behörighet = vilken siffra
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

        private void buttonLektion_Click(object sender, EventArgs e)
        {

            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                new Lektionsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Fel", MessageBoxButtons.OK);
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

        private void dataGridViewBokningar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimeStart_ValueChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Gör så att bokningar visas per vecka
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeSlut_ValueChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Tömmer gridviewn med bokningar så att den endast visar de bokningar som har hämtats för vald tidsperiod
        /// </summary>
        public void UppdateraLista()
        {
            bool aktiv = true;
            radioButtonAktiva.Checked = true;
            dataGridViewBokningar.DataSource = null;
            BefintligaBokningar.Clear();
            BefintligaBokningar = BefintligaBokning(aktiv);
            FyllLista();
            GridDesign();

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
        /// <summary>
        /// gör att användaren kan söka fram en bokning på bokningsnr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSök_Click(object sender, EventArgs e)
        {

            bool aktiv = false;
            if (radioButtonAktiva.Checked == true)
            {
                aktiv = true;
            }
            else if (radioButtonArkiverade.Checked == true)
            {
                aktiv = false;
            }
            BefintligaBokningar.Clear();
            string input = textBoxSök.Text;
            if (input == "")
            {
                dataGridViewBokningar.ClearSelection();
                dataGridViewBokningar.DataSource = BefintligaBokning(aktiv);
                dataGridViewBokningar.Refresh();
                //FyllLista();
                GridDesign();
            }
            else
            {
                dataGridViewBokningar.DataSource = null;
                BefintligaBokningar = bk.HämtaEnBokning(input, aktiv);
                if (!BefintligaBokningar.Any())
                {
                    MessageBox.Show("Inga bokningar matchade din sökning. Kontrollera att du skrivit rätt");
                }
                else
                {
                    dataGridViewBokningar.DataSource = BefintligaBokningar;
                    dataGridViewBokningar.Refresh();
                    FyllLista();
                    GridDesign();
                }

            }


        }

        private void buttonDetaljer_Click(object sender, EventArgs e)
        {
            try
            {
                ValdBokning = dataGridViewBokningar.SelectedRows[0].DataBoundItem as Bokning;
                new ValdBokningsvy(bk, ValdBokning, kk, rk, uk, lk, pk).Show();
            }
            catch
            {
                MessageBox.Show("Ingen bokning vald. Markera en bokning i listan.");
            }


        }

        /// <summary>
        /// Klickar man på välj så visas mer info om bokningen i fälten bredvid listan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVälj_Click(object sender, EventArgs e)
        {
            try
            {
                ValdBokning = dataGridViewBokningar.SelectedRows[0].DataBoundItem as Bokning;
                FyllBokningsInfo();
            }
            catch
            {
                MessageBox.Show("Fel med bokningsvalet, försök igen", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FyllBokningsInfo()
        {
            Faktura f = bk.HämtaFaktura(ValdBokning);
            textBoxattbetala.Text = (f.Summa - f.DelBelopp).ToString();
            textBoxDatum.Text = ValdBokning.FrånDatum.ToString("dd-MM-yyyy") + " - " + ValdBokning.TillDatum.ToString("dd-MM-yyyy");
            textBoxKostnad.Text = f.Summa.ToString();
            textBoxMoms.Text = f.Moms.ToString();
            textBoxFDatumF.Text = f.FörfalloDatumFöreBokning.ToString("dd-MM-yyyy");
            textBoxFDatumE.Text = f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy");
            textBoxattbetala.Text = f.DelBelopp.ToString();
            textBoxMoms.Text = f.Moms.ToString();
            labelANr.Visible = true;
            labelANr.Text = ValdBokning.AnställningsNr;
            labelfnr.Visible = true;
            labelfnr.Text = f.FakturaNr.ToString();
            labelbnr.Visible = true;
            labelbnr.Text = ValdBokning.BokningsNr;
            if (ValdBokning.Kund != null)
            {
                textBoxKundnr.Text = ValdBokning.KundNr.ToString();
                textBoxNamn.Text = ValdBokning.Kund.FörNamn + " " + ValdBokning.Kund.EfterNamn;
                checkBox1.Checked = true;
                checkBox2.Checked = false;
            }
            else
            {
                textBoxKundnr.Text = ValdBokning.FKundNr.ToString();
                textBoxNamn.Text = ValdBokning.FKund.FöretagsNamn;
                checkBox2.Checked = true;
                checkBox1.Checked = false;
            }

            if (ValdBokning.Avbokningsskydd == true)
            {
                radioButtonJa.Checked = true;
            }
            else
            {
                radioButtonNej.Checked = true;
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonAvbryt_Click(object sender, EventArgs e)
        {
            try
            {
                ValdBokning = dataGridViewBokningar.SelectedRows[0].DataBoundItem as Bokning;
                if (ValdBokning.Avbokningsskydd == false)
                {
                    MessageBox.Show("Denna bokning lades utan avbokningsskydd och går därför inte att avbryta");
                }
                else if (ValdBokning.FrånDatum <= DateTime.Now)
                {
                    MessageBox.Show("Bokning har redan påbörjats. Den kan inte längre avbrytas");
                }
                else
                {
                    DialogResult dr = MessageBox.Show($"Är du säker på att du vill avbryta bokning med bokningsnr " +
                        $"{ValdBokning.BokningsNr}? Bokningen kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                    switch (dr)
                    {
                        case DialogResult.Yes:
                            bk.AvbrytBokning(ValdBokning);
                            MessageBox.Show($"Bokning med bokningsnr {ValdBokning.BokningsNr} har nu avbrutits");
                            BefintligaBokningar.Remove(ValdBokning);
                            UppdateraLista();
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

        private void Bokningsvy_Load(object sender, EventArgs e)
        {

        }
    }
}
