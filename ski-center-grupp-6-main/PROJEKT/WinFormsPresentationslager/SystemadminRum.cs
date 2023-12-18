using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class SystemadminRum : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
       

        public List<Rum> Rum { get; set; }
        public Rum ValtRum { get; set; }
        public SystemadminRum(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
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
            Rum = HämtaRum();
            FyllLista();
            GridDesign();

        }
        private void FyllLista()
        {
            dataGridViewRum.DataSource = Rum;
            GridDesign();
        }

        private List<Rum> HämtaRum()
        {
            var lista = new List<Rum>();

            foreach (Rum r in rk.HittaRum())
            {
                lista.Add(r);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns ingen inlagd Utrustning", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return lista;
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

        private void gåTillAdministrationssidanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonBokningar_Click(object sender, EventArgs e)
        {
            new SystemadminPersonal(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonUtrustningReg_Click(object sender, EventArgs e)
        {
            new SystemadminUtrustning(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonLektionReg_Click(object sender, EventArgs e)
        {
            new SystemadminLektion(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridViewRum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonSök_Click(object sender, EventArgs e)
        {
            Rum.Clear();
            string input = textBoxSök.Text;
            Rum = rk.HittaEttRum(input);
            dataGridViewRum.DataSource = null;
            dataGridViewRum.Refresh();
            dataGridViewRum.DataSource = Rum;
            FyllLista();

        }
        public void UppdateraLista()
        {
            Rum.Clear();
            dataGridViewRum.DataSource = null;
            dataGridViewRum.Refresh();
            dataGridViewRum.DataSource = Rum;
            Rum = HämtaRum();
            FyllLista();
            GridDesign();

        }

        private void buttonVälj_Click(object sender, EventArgs e)
        {
            try
            {
                ValtRum = dataGridViewRum.SelectedRows[0].DataBoundItem as Rum;
                FyllRumInfo();
            }
            catch
            {
                MessageBox.Show("Inget rum vald.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FyllRumInfo()
        {
            comboBoxTyp.Text = ValtRum.Typ;
            textBoxStorlek.Text = ValtRum.RumsStorlek;
            textBoxBeskrivning.Text = ValtRum.Beskrivning;
            textBoxPrestanda.Text = ValtRum.Prestanda;
            RumBokning b = rk.HämtaRumBokning(ValtRum);
            if (b != null)
            {
                textBoxBoknr.Text = b.BokningsId;
            }

            labelsysadminanr.Text = ValtRum.RegistreradAv;
            labelsysadminanr.Visible = true;
            labelsysadminanr.Text = ValtRum.RegistreradAv;
            labelrnr.Text = ValtRum.RumsNr.ToString();
            labelbnm.Visible = true;
            labelbnm.Text = ValtRum.Benämning.ToString();
            labelrnr.Visible = true;
            if (ValtRum.Tillgänglig == true)
            {
                radioButtonJa.Checked = true;
            }
            else
            {
                radioButtonNej.Checked = true;
            }

        }

        private void buttonSpara_Click(object sender, EventArgs e)
        {
            if (ValtRum == null)
            {
                MessageBox.Show("Inget rum har valts. Markera rum i listan och klicka på välj innan du ändrar uppgifter.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string typ = comboBoxTyp.Text;
                string storlek = textBoxStorlek.Text;
                string boknr = textBoxBoknr.Text;
                string beskrivning = textBoxBeskrivning.Text;
                string prestanda = textBoxPrestanda.Text;
                bool tillgänglig = false;
                if (radioButtonJa.Checked == true)
                {
                    tillgänglig = true;
                }
                else if (radioButtonNej.Checked == false)
                {
                    tillgänglig = false;
                }

                if (typ == "" || storlek == "" || beskrivning == "" || prestanda == "" || (!radioButtonNej.Checked && !radioButtonJa.Checked))
                {
                    MessageBox.Show("Rumsinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    rk.UppdateraRum(ValtRum, typ, boknr, beskrivning, prestanda, tillgänglig, storlek);
                    MessageBox.Show($"Ändrade uppgifter för rum med rumsnr {ValtRum.RumsNr} har nu sparats.");
                }
                UppdateraLista();
            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            string typ = comboBoxTyp.Text;
            string storlek = textBoxStorlek.Text;
            string boknr = textBoxBoknr.Text;
            string beskrivning = textBoxBeskrivning.Text;
            string prestanda = textBoxPrestanda.Text;
            string antal = textBoxAntal.Text;

            bool tillgänglig = false;
            if (radioButtonJa.Checked == true)
            {
                tillgänglig = true;
            }
            else if (radioButtonNej.Checked == false)
            {
                tillgänglig = false;
            }

            if (typ == "" || storlek == "" || beskrivning == "" || antal == "" || (!radioButtonNej.Checked && !radioButtonJa.Checked))
            {
                MessageBox.Show("Rumsinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int intantal = int.Parse(antal);
                List<Rum> tillagdaRum = new List<Rum>();

                for (int i = 1; i <= intantal; i++)
                {
                    Rum r = rk.LäggTillRum(typ, boknr, beskrivning, prestanda, tillgänglig, storlek);
                    tillagdaRum.Add(r);
                }
                foreach (Rum r in tillagdaRum)
                {
                    MessageBox.Show($"Nytt rum har registrerats och tilldelats rumsnr {r.RumsNr}");
                }
                UppdateraLista();
            }

        }


        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            try
            {
                ValtRum = dataGridViewRum.SelectedRows[0].DataBoundItem as Rum;


                DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort rum med rumsnr " +
                    $"{ValtRum.RumsNr}? Rummet kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                switch (dr)
                {
                    case DialogResult.Yes:
                        rk.TaBortRum(ValtRum);
                        MessageBox.Show($"Rum med rumsnr {ValtRum.RumsNr} har nu raderats");
                        Rum.Remove(ValtRum);
                        UppdateraLista();
                        break;
                    case DialogResult.No:
                        break;
                }

            }
            catch
            {
                MessageBox.Show("Inget rum valt. Markera rum i listan som du vill ta bort.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonRensa_Click(object sender, EventArgs e)
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };
            radioButtonJa.Checked = false;
            radioButtonNej.Checked = false;
            labelrnr.Visible = false;
            labelsysadminanr.Visible = false;
            labelbnm.Visible = false;


            func(Controls);
        }
        public void GridDesign()
        {
            dataGridViewRum.Columns["Tillgänglig"].Visible = false;
            dataGridViewRum.Columns["Beskrivning"].Visible = false;
            dataGridViewRum.Columns["RumBokning"].Visible = false;
            dataGridViewRum.Columns["Pris"].Visible = false;
            dataGridViewRum.AllowUserToOrderColumns = true;
            Rum.Sort(
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

        private void SystemadminRum_Load(object sender, EventArgs e)
        {
            comboBoxTyp.Items.Add("Logi");
            comboBoxTyp.Items.Add("Konferens");
            comboBoxTyp.Items.Add("Camp");

        }

        private void comboBoxTyp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
