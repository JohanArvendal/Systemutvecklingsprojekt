using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class ValdRumVy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
        


        /// <summary>
        /// Visar all data om valt rum 
        /// </summary>
        /// <param name="kontroller"></param>
        /// <param name="valtRum"></param>
        public ValdRumVy(PersonalKontroller pk, BokningKontroller bk, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk, Rum valtRum, DateTime fDatum)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;

            textBoxBenäm.Text = valtRum.Benämning;
            textBoxRNr.Text = valtRum.RumsNr.ToString();
            textBoxRStorlek.Text = valtRum.RumsStorlek;
            textBoxRTyp.Text = valtRum.Typ;
            int vecka = RumKontroller.HämtaVecka(fDatum);
            if (valtRum.Typ.Contains("Logi"))
            {
                int dygn = rk.HämtaLogiPris(vecka, 1, valtRum.RumsStorlek, fDatum, fDatum);
                int veckpris = rk.HämtaLogiPris(vecka, 7, valtRum.RumsStorlek, fDatum, fDatum.AddDays(8));
                textBoxD.Text = dygn.ToString();
                textBoxPrisV.Text = veckpris.ToString();
            }
            else
            {
                int dygn = rk.HämtaKonferensPris(vecka, 1, valtRum.RumsStorlek, fDatum, fDatum.AddDays(1));
                int veckpris = rk.HämtaKonferensPris(vecka, 7, valtRum.RumsStorlek, fDatum, fDatum.AddDays(8));
                textBoxD.Text = dygn.ToString();
                textBoxPrisV.Text = veckpris.ToString();
            }
            textBoxBeskrivning.Text = valtRum.Beskrivning;
            textBoxPrestanda.Text = valtRum.Prestanda;
        }

        private void ValdRumVy_Load(object sender, EventArgs e)
        {

        }

        private void textBoxRNr_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxBenäm_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRTyp_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPrisD_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRStorlek_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPrisV_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPers_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxBeskrivning_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonTillbaka_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
