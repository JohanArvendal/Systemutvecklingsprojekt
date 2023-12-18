using SkiCenterKontroller;
using System;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class LoggaIn : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;





        public LoggaIn(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;

        }

        public void buttonLoggaIn_Click(object sender, EventArgs e)
        {
            string AnställningsNr = txtAnvNamn.Text;
            string lösenord = Lösenord.Text;


            if (pk.LoggaIn(AnställningsNr, lösenord) == true)
            {
                new Bokningsvy(pk, bk, kk, rk, uk, lk).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Fel användarnamn och/eller lösenord.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtAnvNamn_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

      

        private void LoggaIn_Load(object sender, EventArgs e)
        {

        }

        private void Lösenord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonLoggaIn.PerformClick();
            }
        }
    }
}
