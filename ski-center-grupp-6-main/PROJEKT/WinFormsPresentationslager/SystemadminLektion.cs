using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class SystemadminLektion : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
       

        public List<Lektion> Lektion { get; set; }
        public Lektion ValdLektion { get; set; }
        public SystemadminLektion(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
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
            Lektion = HämtaLektion();
            FyllLista();
            GridDesign();
        }
        private void FyllLista()
        {
            dataGridViewLektion.DataSource = Lektion;
            GridDesign();
        }

        private List<Lektion> HämtaLektion()
        {
            var lista = new List<Lektion>();

            foreach (Lektion l in lk.HittaLektion())
            {
                lista.Add(l);
            }
            if (lista == null)
            {
                MessageBox.Show("Det finns ingen inlagd Lektion");
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

        private void buttonUtrustningReg_Click(object sender, EventArgs e)
        {
            new SystemadminUtrustning(pk, bk, kk, rk, uk, lk).Show();
            this.Close();
        }

        private void buttonSök_Click(object sender, EventArgs e)
        {
            Lektion.Clear();
            string input = textBoxSök.Text;
            Lektion = lk.HittaEnLektion(input);
            dataGridViewLektion.DataSource = null;
            dataGridViewLektion.Refresh();
            dataGridViewLektion.DataSource = Lektion;
            FyllLista();
            GridDesign();

        }
        public void UppdateraLista()
        {
            dataGridViewLektion.DataSource = null;
            dataGridViewLektion.Refresh();
            dataGridViewLektion.DataSource = Lektion;
            Lektion = HämtaLektion();
            FyllLista();
            GridDesign();


        }

        private void buttonVälj_Click(object sender, EventArgs e)
        {
            try
            {
                ValdLektion = dataGridViewLektion.SelectedRows[0].DataBoundItem as Lektion;
                FyllLektionInfo();
            }
            catch
            {
                MessageBox.Show("Ingen lektion vald", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FyllLektionInfo()
        {
            comboBoxTyp.Text = ValdLektion.Typ;
            comboBoxGrupp.Text = ValdLektion.Grupp;
            comboBoxLärare.Text = ValdLektion.AnställningsNr;
            textBoxStorlek.Text = ValdLektion.AntalPlatser.ToString();
            comboBoxTid.Text = ValdLektion.Tid.ToString();
            if (ValdLektion.LektionStart == "måndag" && ValdLektion.LektionStart == "onsdag")
            {
                comboBoxPeriod.Text = "Mån-ons";
            }
            else if (ValdLektion.LektionStart == "torsdag" && ValdLektion.LektionStart == "fredag")
            {
                comboBoxPeriod.Text = "Tors-fre";
            }
            else
            {
                comboBoxPeriod.Text = ValdLektion.LektionStart;
            }
            labelsysadminanr.Visible = true;
            labelsysadminanr.Text = ValdLektion.RegistreradAv;
            labelbnm.Text = ValdLektion.Benämning;
            labelbnm.Visible = true;
            if (ValdLektion.Tillgänglig == true)
            {
                radioButtonJa.Checked = true;
            }
            else
            {
                radioButtonNej.Checked = true;
            }
            GridDesign();
        }

        private void buttonSpara_Click(object sender, EventArgs e)
        {
            if (ValdLektion == null)
            {
                MessageBox.Show("Ingen lektion har valts. Markera lektion i listan och klicka på välj innan du ändrar uppgifter.");
            }
            else
            {
                string typ = comboBoxTyp.Text;
                string grupp = comboBoxGrupp.Text;
                string lärare = comboBoxLärare.Text;
                string antalp = textBoxStorlek.Text;
                string tid = comboBoxTid.Text;
                string start = null;
                string slut = null;
                bool tillgänglig = false;
                if (radioButtonJa.Checked == true)
                {
                    tillgänglig = true;
                }
                else if (radioButtonNej.Checked == false)
                {
                    tillgänglig = false;
                }
                if (comboBoxPeriod.Text == "Mån-ons")
                {
                    start = "måndag";
                    slut = "onsdag";
                }
                else if (comboBoxPeriod.Text == "Tors-fre")
                {
                    start = "måndag";
                    slut = "onsdag";
                }
                else
                {
                    start = comboBoxPeriod.Text;
                    slut = comboBoxPeriod.Text;
                }
                if (typ == "" || grupp == "" || lärare == "" || antalp == "" || start == null || slut == null || tid == ""
                    || (!radioButtonNej.Checked && !radioButtonJa.Checked))
                {
                    MessageBox.Show("Lektionsinformation saknas. Fyll i samtliga fält");
                }
                else
                {
                    lk.UppdateraLektion(ValdLektion, typ, grupp, lärare, antalp, start,
                        slut, tid, tillgänglig);
                    MessageBox.Show($"Ändrade uppgifter för lektion med benämning {ValdLektion.Benämning.Trim()} har nu sparats.");
                }
                UppdateraLista();

            }
        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            if (comboBoxTyp.SelectedItem !=null)
            {


                string typ = comboBoxTyp.Text;
                string grupp = comboBoxGrupp.Text;
                string lärare = comboBoxLärare.Text;
                string antalp = textBoxStorlek.Text;
                string tid = comboBoxTid.Text;
                string start = null;
                string slut = null;
                bool tillgänglig = false;
                if (radioButtonJa.Checked == true)
                {
                    tillgänglig = true;
                }
                else if (radioButtonNej.Checked == false)
                {
                    tillgänglig = false;
                }
                if (comboBoxTyp.SelectedItem.ToString() == "Mån-ons")
                {
                    start = "måndag";
                    slut = "onsdag";
                }
                else if (comboBoxTyp.SelectedItem.ToString() == "Tors-fre")
                {
                    start = "måndag";
                    slut = "onsdag";
                }
                else
                {
                    start = comboBoxPeriod.Text;
                    slut = comboBoxPeriod.Text;
                }

                if (typ == "" || grupp == "" || lärare == "" || antalp == "" || tid == "" || start == null || slut == null
                        || (!radioButtonNej.Checked && !radioButtonJa.Checked))
                {
                    MessageBox.Show("Lektionsinformation saknas. Fyll i samtliga fält");
                }
                else
                {

                    Lektion l = lk.LäggTillLektion(typ, grupp, lärare, antalp, tid,
                        start, slut, tillgänglig);


                    MessageBox.Show($"Ny lektion har registrerats och tilldelats benämning {l.Benämning.Trim()}");


                }
                UppdateraLista();
            }
            else
            {
                MessageBox.Show("Lektionsinformation saknas. Fyll i samtliga fält.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            try
            {
                ValdLektion = dataGridViewLektion.SelectedRows[0].DataBoundItem as Lektion;
                DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort lektion med benämning " +
                    $"{ValdLektion.Benämning.Trim()}? Lektionen kommer raderas permanent.", "Yes", MessageBoxButtons.YesNo);

                switch (dr)
                {
                    case DialogResult.Yes:
                        lk.TaBortLektion(ValdLektion);
                        MessageBox.Show($"Lektion med benämning {ValdLektion.Benämning.Trim()} har nu raderats");
                        Lektion.Remove(ValdLektion);
                        UppdateraLista();
                        break;
                    case DialogResult.No:
                        break;
                }
            }

            catch
            {
                MessageBox.Show("Ingen lektion vald. Markera lektion i listan som du vill ta bort.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dataGridViewLektion.Columns["Tillgänglig"].Visible = false;
            dataGridViewLektion.Columns["Lärare"].Visible = false;
            dataGridViewLektion.Columns["LektionBokning"].Visible = false;
            dataGridViewLektion.Columns["Pris"].Visible = false;
            dataGridViewLektion.AllowUserToOrderColumns = true;
            Lektion.Sort(
            delegate (Lektion p1, Lektion p2)
            {
                int compare = p1.Grupp.CompareTo(p2.Grupp);
                if (compare == 0)
                {
                    return p2.Benämning.CompareTo(p1.Benämning);
                }
                return compare;
            });
        }

        private void SystemadminLektion_Load(object sender, EventArgs e)
        {
            comboBoxTyp.Items.Add("Privatlektion");
            comboBoxTyp.Items.Add("Skidskola");

            foreach (Personal p in pk.HämtaLärare())
            {
                comboBoxLärare.Items.Add(p.AnställningsNr);
            }


        }

        private void comboBoxGrupp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTyp_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxTyp.SelectedItem.ToString() == "Skidskola")
            {
                comboBoxGrupp.Items.Clear();
                comboBoxGrupp.Items.Add("Grön");
                comboBoxGrupp.Items.Add("Blå");
                comboBoxGrupp.Items.Add("Röd");
                comboBoxGrupp.Items.Add("Svart");

                comboBoxPeriod.Items.Clear();
                comboBoxPeriod.Items.Add("Mån-ons");
                comboBoxPeriod.Items.Add("Tors-fre");

            }
            else if (comboBoxTyp.SelectedItem.ToString() == "Privatlektion")
            {
                comboBoxGrupp.Items.Clear();
                comboBoxGrupp.Items.Add("Privatlektion");

                comboBoxPeriod.Items.Clear();
                comboBoxPeriod.Items.Add("Måndag");
                comboBoxPeriod.Items.Add("Tisdag");
                comboBoxPeriod.Items.Add("Onsdag");
                comboBoxPeriod.Items.Add("Torsdag");
                comboBoxPeriod.Items.Add("Fredag");
            }
        }

        private void dateTimePickerTid_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTid.Items.Clear();
            comboBoxTid.Items.Add("09:00:00");
            comboBoxTid.Items.Add("10:00:00");
            comboBoxTid.Items.Add("11:00:00");
            comboBoxTid.Items.Add("12:00:00");
            comboBoxTid.Items.Add("13:00:00");
            comboBoxTid.Items.Add("14:00:00");
            comboBoxTid.Items.Add("15:00:00");
            comboBoxTid.Items.Add("16:00:00");
            comboBoxTid.Items.Add("17:00:00");
            comboBoxTid.Items.Add("18:00:00");
        }
    }

}
