using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class SystemadminPersonal : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        

        public List<Personal> Personal { get; set; }
        public Personal ValdPersonal { get; set; }
        public SystemadminPersonal(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
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

            Personal = HämtaPersonal();
            FyllLista();
        }

        private void FyllLista()
        {
            dataGridViewPersonal.DataSource = Personal;
        }

        private List<Personal> HämtaPersonal()
        {
            var lista = new List<Personal>();

            foreach (Personal p in pk.HittaPersonal())
            {
                lista.Add(p);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns ingen inlagd personal", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void buttonRumReg_Click(object sender, EventArgs e)
        {
            new SystemadminRum(pk, bk, kk, rk, uk, lk).Show();
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

        private void gåTillAdministrationssidanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void dataGridViewPersonal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxSök_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSök_Click(object sender, EventArgs e)
        {
            Personal.Clear();
            string input = textBoxSök.Text;
            Personal = pk.HittaEnPersonal(input);
            dataGridViewPersonal.DataSource = null;
            dataGridViewPersonal.Refresh();
            dataGridViewPersonal.DataSource = Personal;
            FyllLista();
            GridDesign();

        }
        public void GridDesign()
        {
            dataGridViewPersonal.Columns["Lektion"].Visible = false;

        }
        public void UppdateraLista()
        {
            Personal.Clear();
            dataGridViewPersonal.DataSource = null;
            dataGridViewPersonal.Refresh();
            dataGridViewPersonal.DataSource = Personal;
            Personal = HämtaPersonal();
            FyllLista();
            GridDesign();
        }

        private void buttonVälj_Click(object sender, EventArgs e)
        {
            try
            {
                ValdPersonal = dataGridViewPersonal.SelectedRows[0].DataBoundItem as Personal;
                FyllPersonalInfo();
            }
            catch
            {
                MessageBox.Show("Ingen personal vald.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FyllPersonalInfo()
        {
            textBoxBehörighet.Text = ValdPersonal.Behörighet.ToString();
            textBoxenamn.Text = ValdPersonal.EfterNamn;
            textBoxfnamn.Text = ValdPersonal.FörNamn;
            textBoxGata.Text = ValdPersonal.Adress;
            textBoxLösen.Text = ValdPersonal.Lösenord;
            textBoxPersnr.Text = ValdPersonal.PersonNr.ToString();
            textBoxMejl.Text = ValdPersonal.Mail;
            textBoxOrt.Text = ValdPersonal.Ort;
            textBoxPostnr.Text = ValdPersonal.PostNr.ToString();
            textBoxRoll.Text = ValdPersonal.Roll;
            textBoxTelenr.Text = ValdPersonal.TelefonNr.ToString();
            labelsysadminanr.Text = ValdPersonal.RegistreradAv;
            labelsysadminanr.Visible = true;
            labelAnr.Text = ValdPersonal.AnställningsNr;
            labelAnr.Visible = true;


        }

        private void buttonSpara_Click(object sender, EventArgs e)
        {
            if (ValdPersonal == null)
            {
                MessageBox.Show("Ingen personal har valts. Markera personal i listan och klicka på välj innan du ändrar uppgifter.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string behörighet = textBoxBehörighet.Text;
                string eNamn = textBoxenamn.Text;
                string fNamn = textBoxfnamn.Text;
                string gata = textBoxGata.Text;
                string lösen = textBoxLösen.Text;
                string persnr = textBoxPersnr.Text;
                string mejl = textBoxMejl.Text;
                string ort = textBoxOrt.Text;
                string postnr = textBoxPostnr.Text;
                string roll = textBoxRoll.Text;
                string telenr = textBoxTelenr.Text;

                if (behörighet == "" || eNamn == "" || fNamn == "" || gata == "" || lösen == "" || persnr == "" || mejl == ""
                    || ort == "" || postnr == "" || roll == "" || telenr == "")
                {
                    MessageBox.Show("Personalinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    pk.UppdateraPersonal(ValdPersonal, behörighet, eNamn, fNamn, gata, lösen, persnr, mejl, ort, postnr, roll, telenr);
                    MessageBox.Show($"Ändrade uppgifter för personal med anställningsnr {ValdPersonal.AnställningsNr} har nu sparats.");
                }
                UppdateraLista();
            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            string behörighet = textBoxBehörighet.Text;
            string eNamn = textBoxenamn.Text;
            string fNamn = textBoxfnamn.Text;
            string gata = textBoxGata.Text;
            string lösen = textBoxLösen.Text;
            string persnr = textBoxPersnr.Text;
            string mejl = textBoxMejl.Text;
            string ort = textBoxOrt.Text;
            string postnr = textBoxPostnr.Text;
            string roll = textBoxRoll.Text;
            string telenr = textBoxTelenr.Text;

            if (behörighet == "" || eNamn == "" || fNamn == "" || gata == "" || lösen == "" || persnr == "" || mejl == ""
                || ort == "" || postnr == "" || roll == "" || telenr == "")
            {
                MessageBox.Show("Personalinformation saknas. Fyll i samtliga fält", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Personal p = pk.LäggTillPersonal(behörighet, eNamn, fNamn, gata, lösen, persnr, mejl, ort, postnr, roll, telenr);

                MessageBox.Show($"Ny personal har registrerats och tilldelats anställningsnr {p.AnställningsNr}");
            }
            UppdateraLista();
        }

        private void SystemadminPersonal_Load(object sender, EventArgs e)
        {

        }

        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            try
            {
                ValdPersonal = dataGridViewPersonal.SelectedRows[0].DataBoundItem as Personal;

                DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort personal med anställningsnr " +
                    $"{ValdPersonal.AnställningsNr}? Personalen kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                switch (dr)
                {
                    case DialogResult.Yes:
                        pk.TaBortPersonal(ValdPersonal);
                        MessageBox.Show($"Personal med anställningsnr {ValdPersonal.AnställningsNr} har nu raderats");
                        Personal.Remove(ValdPersonal);
                        UppdateraLista();
                        break;
                    case DialogResult.No:
                        break;
                }


            }
            catch
            {
                MessageBox.Show("Ingen Personal vald. Markera personal i listan som du vill ta bort.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void labelsysadminanr_Click(object sender, EventArgs e)
        {

        }

        private void textBoxRoll_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxRoll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMejl_TextChanged(object sender, EventArgs e)
        {

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
            labelAnr.Visible = false;
            labelsysadminanr.Visible = false;
            func(Controls);
        }
    }
}
