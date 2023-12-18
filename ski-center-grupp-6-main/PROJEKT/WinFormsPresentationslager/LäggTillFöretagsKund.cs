using SKICENTER;
using SkiCenterKontroller;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class LäggTillFöretagsKund : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
       

        public LäggTillFöretagsKund(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string orgid = textBox1.Text;
            long persnr = long.Parse(orgid);
            string fnamn = textBox4.Text;
            string gata = textBox3.Text;
            string posttxt = postNr.Text;
            int postnr = int.Parse(posttxt);
            string ort = ortNamn.Text;
            string tnrtext = telNr.Text;
            long telenr = long.Parse(tnrtext);
            string mejl = mejlText.Text;
            string kred = kgr.Text;
            int kredgräns = int.Parse(kred);

            if (fnamn == null || orgid == null || tnrtext == null || gata == null || posttxt == null || ort == null || mejl == null || kred == null)
            {
                MessageBox.Show("Kundinformation saknas, vänligen fyll i samtliga fält");
            }
            else
            {
                FöretagsKund f = kk.LäggTillFKund(gata, mejl, fnamn, ort, postnr, telenr, kredgräns, persnr);
                MessageBox.Show("Kunden har lagts in i kundregistret", "Klart", MessageBoxButtons.OK);
                this.Hide();
            }

        }

        private void buttonBack_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
