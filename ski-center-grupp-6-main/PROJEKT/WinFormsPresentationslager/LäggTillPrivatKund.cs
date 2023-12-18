using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class LäggTillPrivatKund : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        


        public LäggTillPrivatKund(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Kopplingar till textlådorna
            string fnamn = textBox1.Text;
            string enamn = textBox2.Text;
            string teletxt = textBox3.Text;
            long telenr = long.Parse(teletxt);
            string gata = textBox4.Text;
            string posttxt = textBox5.Text;
            int postnr = int.Parse(posttxt);
            string ort = textBox6.Text;
            string mejl = textBox7.Text;
            string kredgräns = "12000";
            textBoxKred.Text = kredgräns;
            string pnr = textBox9.Text;
            long persnr = long.Parse(pnr);

            #endregion Kopplingar till textlådorna




            if (fnamn == null || enamn == null || teletxt == null || gata == null || posttxt == null || ort == null || mejl == null || pnr == null)
            {
                MessageBox.Show("Kundinformation saknas, vänligen fyll i samtliga fält");
            }
            else
            {
                Kund k = kk.LäggTillKund(gata, mejl, fnamn, enamn, ort, postnr, telenr, 12000, persnr);
                MessageBox.Show("Kunden har lagts in i kundregistret", "Klart", MessageBoxButtons.OK);
                this.Hide();
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
