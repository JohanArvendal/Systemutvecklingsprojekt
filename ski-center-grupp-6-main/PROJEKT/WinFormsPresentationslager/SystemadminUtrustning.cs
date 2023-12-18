using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class SystemadminUtrustning : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        public List<Utrustning> Utrustning { get; set; }
        public Utrustning ValdUtrustning { get; set; }
        public SystemadminUtrustning(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
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
            Utrustning = HämtaUtrustning();
            FyllLista();
            GridDesign();
        }
        private void FyllLista()
        {
            dataGridViewUtrustning.DataSource = Utrustning;
        }

        private List<Utrustning> HämtaUtrustning()
        {
            var lista = new List<Utrustning>();

            foreach (Utrustning p in uk.HittaUtrustning())
            {
                lista.Add(p);
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

        private void buttonRumReg_Click(object sender, EventArgs e)
        {
            new SystemadminRum(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonLektionReg_Click(object sender, EventArgs e)
        {
            new SystemadminLektion(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void SystemadminUtrustning_Load(object sender, EventArgs e)
        {

        }

        private void textBoxTyp_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSök_Click(object sender, EventArgs e)
        {
            Utrustning.Clear();
            string input = textBoxSök.Text;
            Utrustning = uk.HittaEnUtrustning(input);
            dataGridViewUtrustning.DataSource = null;
            dataGridViewUtrustning.Refresh();
            dataGridViewUtrustning.DataSource = Utrustning;
            FyllLista();
            GridDesign();
        }
        public void UppdateraLista()
        {
            Utrustning.Clear();
            dataGridViewUtrustning.DataSource = null;
            dataGridViewUtrustning.Refresh();
            dataGridViewUtrustning.DataSource = Utrustning;
            Utrustning = HämtaUtrustning();
            FyllLista();
            GridDesign();

        }

        private void buttonVälj_Click(object sender, EventArgs e)
        {
            try
            {
                ValdUtrustning = dataGridViewUtrustning.SelectedRows[0].DataBoundItem as Utrustning;
                FyllUtrustningsInfo();
            }
            catch 
            { 
                MessageBox.Show("Ingen utrustning vald.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
        private void FyllUtrustningsInfo()
        {
            textBoxTyp.Text = ValdUtrustning.Typ;
            textBoxArtikel.Text = ValdUtrustning.UtrustningsArtikel;
            textBoxStorlek.Text = ValdUtrustning.Storlek.ToString();
            UtrustningBokning b = bk.HämtaUtrustningBokning(ValdUtrustning);
            if (b != null)
            {
                textBoxBoknr.Text = b.BokningsId;
            }
            labelsysadminanr.Text = ValdUtrustning.RegistreradAv;
            labelsysadminanr.Visible = true;
            labelbnm.Text = ValdUtrustning.Benämning;
            labelbnm.Visible = true;
            if (ValdUtrustning.Tillgänglig == true)
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
            if (ValdUtrustning == null)
            {
                MessageBox.Show("Ingen utrustning har valts. Markera utrustning i listan och klicka på välj innan du ändrar uppgifter.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string typ = textBoxTyp.Text;
                string artikel = textBoxArtikel.Text;
                string storlek = textBoxStorlek.Text;
                string boknr = textBoxBoknr.Text;
                bool tillgänglig = false;
                if (radioButtonJa.Checked == true)
                {
                    tillgänglig = true;
                }
                else if (radioButtonNej.Checked == false)
                {
                    tillgänglig = false;
                }

                if (typ == "" || artikel == "" || storlek == "" || (!radioButtonNej.Checked && !radioButtonJa.Checked))
                {
                    MessageBox.Show("Utrustningsinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    uk.UppdateraUtrustning(ValdUtrustning, typ, artikel, boknr, tillgänglig, storlek);
                    MessageBox.Show($"Ändrade uppgifter för utrustning med benämning {ValdUtrustning.Benämning.Trim()} har nu sparats.");
                }
                UppdateraLista();
            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            try
            {


                string typ = textBoxTyp.Text;
                string artikel = textBoxArtikel.Text;
                string storlek = textBoxStorlek.Text;
                string boknr = textBoxBoknr.Text;
                bool tillgänglig = false;
                string antal = textBoxAntal.Text;
                int intantal = int.Parse(antal);
                if (radioButtonJa.Checked == true)
                {
                    tillgänglig = true;
                }
                else if (radioButtonNej.Checked == false)
                {
                    tillgänglig = false;
                }

                if (typ == "" || artikel == "" || storlek == "" || (!radioButtonNej.Checked && !radioButtonJa.Checked))
                {
                    MessageBox.Show("Utrustningsinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    List<Utrustning> tillagdUtrustning = new List<Utrustning>();

                    for (int i = 1; i <= intantal; i++)
                    {
                        Utrustning u = uk.LäggTillUtrustning(typ, artikel, boknr, tillgänglig, storlek);
                        tillagdUtrustning.Add(u);
                    }
                    foreach (Utrustning u in tillagdUtrustning)
                    {
                        MessageBox.Show($"Ny utrustning har registrerats och tilldelats benämning {u.Benämning.Trim()}");
                    }

                }
                UppdateraLista();
            }
            catch
            {
                MessageBox.Show("Vänligen se över alla ifyllda fält och försök igen", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            try
            {
                ValdUtrustning = dataGridViewUtrustning.SelectedRows[0].DataBoundItem as Utrustning;

                DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort utrustning med benämning " +
                    $"{ValdUtrustning.Benämning.Trim()}? Utrustningen kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                switch (dr)
                {
                    case DialogResult.Yes:
                        uk.TaBortUtrustning(ValdUtrustning);
                        MessageBox.Show($"Utrustning med benämning {ValdUtrustning.Benämning.Trim()} har nu raderats");
                        Utrustning.Remove(ValdUtrustning);
                        UppdateraLista();
                        break;
                    case DialogResult.No:
                        break;
                }

            }
            catch
            {
                MessageBox.Show("Ingen utrustning vald. Markera utrustning i listan som du vill ta bort.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            labelbnm.Visible = false;
            labelsysadminanr.Visible = false;


            func(Controls);
        }
        public void GridDesign()
        {
            dataGridViewUtrustning.Columns["Tillgänglig"].Visible = false;
            dataGridViewUtrustning.Columns["Pris"].Visible = false;
            dataGridViewUtrustning.Columns["UtrustningBokning"].Visible = false;

            dataGridViewUtrustning.AllowUserToOrderColumns = true;
            Utrustning.Sort(
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
    }
}
