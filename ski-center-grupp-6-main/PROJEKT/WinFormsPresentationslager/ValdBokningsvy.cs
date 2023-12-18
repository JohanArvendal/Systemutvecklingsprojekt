using SKICENTER;
using SkiCenterKontroller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsPresentationslager
{
    public partial class ValdBokningsvy : Form
    {
        private LektionKontroller lk;
        private BokningKontroller bk;
        private PersonalKontroller pk;
        private KundKontroller kk;
        private RumKontroller rk;
        private UtrustningKontroller uk;
       

        public Bokning Bokning { get; set; }
        public UtrustningBokning uBokning { get; set; }
        public RumBokning rBokning { get; set; }
        public LektionBokning lBokning { get; set; }
        public Utrustning ValdUtrustning { get; set; }
        public Rum ValtRum { get; set; }
        public Lektion ValdLektion { get; set; }
        public List<Rum> BokadeRum { get; set; }
        public List<Utrustning> BokadUtrustning { get; set; }
        public List<Lektion> BokadLektion { get; set; }
        public ValdBokningsvy(BokningKontroller bk, Bokning valdBokning, KundKontroller kk, RumKontroller rk, UtrustningKontroller uk, LektionKontroller lk, PersonalKontroller pk)
        {
            InitializeComponent();
            this.pk = pk;
            this.bk = bk;
            this.rk = rk;
            this.kk = kk;
            this.uk = uk;
            this.lk = lk;
            Bokning = valdBokning;
            if (valdBokning.Kund != null)
            {
                textBoxKundnr.Text = Bokning.Kund.KundNr.ToString();
                textBoxNamn.Text = Bokning.Kund.FörNamn + " " + Bokning.Kund.EfterNamn;

            }
            else
            {
                textBoxKundnr.Text = Bokning.FKund.FKundNr.ToString();
                textBoxNamn.Text = Bokning.FKund.FöretagsNamn;
            }
            if (Bokning.Avbokningsskydd == true)
            {
                radioButtonJa.Checked = true;
            }
            else
            {
                radioButtonNej.Checked = false;
            }
            Faktura f = bk.HämtaFaktura(Bokning);
            BokadeRum = rk.HämtaBokadeRum(Bokning);
            BokadUtrustning = uk.HämtaBokadUtrustning(Bokning);
            BokadLektion = lk.HämtaBokadLektion(Bokning);

        }

        private void FyllRumLista()
        {
            dataGridViewTjänster.DataSource = BokadeRum;
        }
        private void FyllUtrustningLista()
        {
            dataGridViewTjänster.DataSource = BokadUtrustning;
        }
        private void FyllLektionLista()
        {
            dataGridViewTjänster.DataSource = BokadLektion;
        }

        private void dataGridViewTjänster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void buttonTillbaka_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSpara_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
                {
                    if (ValdUtrustning == null)
                    {
                        MessageBox.Show("Ingen utrustning har valts. Markera utrustning i listan du vill ändra uppgifter för och klicka på välj.");
                    }
                    else
                    {
                        string kostnad = textBoxKostnad.Text;
                        DateTime start = dateTimePickerHstart.Value;
                        DateTime slut = dateTimePickerHslut.Value;
                        if (start < Bokning.FrånDatum || slut > Bokning.TillDatum)
                        {
                            MessageBox.Show("Ogiltiga datum. Utrustningsbokningen behöver vara inom rumsbokningsperioden");
                        }
                        else
                        {
                            bk.ÄndraUtrustningsBokning(kostnad, start, slut, uBokning, Bokning);
                            MessageBox.Show("Utrustningsbokning är nu uppdaterad.");
                        }
                    }
                }
                else if (buttonRum.BackColor == SystemColors.ControlLightLight)
                {
                    if (ValtRum == null)
                    {
                        MessageBox.Show("Inget rum har valts. Markera rum i listan du vill ändra uppgifter för och klicka på välj.");
                    }
                    else
                    {
                        string kostnad = textBoxRumKostnad.Text;
                        DateTime start = dateTimePickerBstart.Value;
                        DateTime slut = dateTimePickerBslut.Value;
                        if (ValtRum.Typ.Contains("Konferens") && (start < Bokning.FrånDatum || slut > Bokning.TillDatum))
                        {
                            MessageBox.Show("Ogiltiga datum. Konferensrumssbokningen behöver vara inom rumsbokningsperioden");
                        }
                        else
                        {
                            bk.ÄndraRumsBokning(kostnad, start, slut, rBokning, Bokning);
                            MessageBox.Show("Rumsbokning är nu uppdaterad.");
                        }
                    }
                }
                else if (buttonLektion.BackColor == SystemColors.ControlLightLight)
                {
                    if (ValdLektion == null)
                    {
                        MessageBox.Show("Ingen lektion har valts. Markera lektion i listan du vill ändra uppgifter för och klicka på välj.");
                    }
                    else
                    {
                        string kostnad = textBoxLektionKostnad.Text;

                        bk.ÄndraLektionsBokning(kostnad, lBokning, Bokning);
                        MessageBox.Show("Lektionsbokning är nu uppdaterad.");
                    }



                }
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }



        }

        private void buttonLäggTill_Click(object sender, EventArgs e)
        {
            bk.SkickaVidareBokning(Bokning);
            if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
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
            else if (buttonLektion.BackColor == SystemColors.ControlLightLight)
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
            else if (buttonRum.BackColor == SystemColors.ControlLightLight)
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
            else
            {
                MessageBox.Show("Ingen tjänst vald.");

            }
        }

        private void buttonÅterlämnad_Click(object sender, EventArgs e)
        {

            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {


                if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {
                        ValdUtrustning = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Utrustning;
                        uBokning = bk.HittauBokning(ValdUtrustning, Bokning);


                        if (uBokning.Utlämnad == true)
                        {
                            uk.ÅterlämnaUtrustning(ValdUtrustning, Bokning, uBokning);
                            dataGridViewTjänster.DataSource = null;
                            dataGridViewTjänster.Refresh();
                            dataGridViewTjänster.DataSource = BokadUtrustning;
                            GridDesignUtrustning();
                            MessageBox.Show($"Utrustning med utrustningsID {ValdUtrustning.Benämning.Trim()} har nu rapporterats som återlämnad.");
                        }
                        else
                        {
                            if (uBokning.Återlämnad == true)
                            {
                                MessageBox.Show("Utrustningen har redan markerats som återlämnad.");
                            }
                            else
                            {
                                MessageBox.Show("Utrustningen är inte möjlig att återlämna innan bokningen har startat. Vill du avbryta bokningen, klicka istället på ta bort");
                            }



                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else if (buttonRum.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValtRum = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Rum;
                        rBokning = bk.HittarBokning(ValtRum, Bokning);
                        List<RumBokning> rb = new List<RumBokning>();
                        List<Rum> konf = new List<Rum>();
                        List<Rum> logi = new List<Rum>();
                        foreach (Rum r in BokadeRum)
                        {
                            foreach (RumBokning t in r.RumBokning.Where(q => q.BokningsId.Contains(Bokning.BokningsNr)))
                            {
                                if (!rb.Contains(t))
                                {
                                    rb.Add(t);
                                }
                            }

                            if (r.Typ.Contains("Konferens"))
                            {
                                if (!konf.Contains(r))

                                {

                                    konf.Add(r);
                                }
                            }
                            else
                            {
                                if (!logi.Contains(r))
                                {
                                    logi.Add(r);
                                }
                            }
                        }
                        foreach (RumBokning t in rb.Where(b => b.Avslutad == true))
                        {
                            if (t.Rum.Typ.Contains("Konferens"))
                            {

                                konf.Remove(t.Rum);

                            }
                            else
                            {

                                logi.Remove(t.Rum);

                            }
                        }
                    
                     
                        
                        if (rBokning.Startad == true && rBokning.Avslutad == false)
                        {
                            if (konf.Any() && logi.Count() == 1 && (ValtRum.Typ.Contains("Logi") || ValtRum.Typ.Contains("Camp")))
                            {

                                MessageBox.Show($"Bokningen är kopplat till logibokningen du försöker avsluta. " +
                                       $"Avslutar du rumbokningen så avslutas hela bokningen. För att avsluta hela bokningen behöver" +
                                       $"konferensbokningarna rapporteras som avslutade först.");
                            }
                            else if (konf.Count() == 0 && logi.Count() == 1)
                            {
                                List<UtrustningBokning> ub = bk.KontrollerauBoking(Bokning);
                                if (ub.Any(t => t.Återlämnad == false))
                                {
                                    MessageBox.Show($"Bokningen är kopplat till rumbokningen du försöker avsluta. " +
                                       $"Avslutar du rumbokningen så avslutas hela bokningen. För att avsluta hela bokningen behöver" +
                                       $"utrustningen rapporteras som återlämnad först.");
                                }
                                else
                                {
                                    DialogResult dr = MessageBox.Show($"Detta rum är den sista aktiva tjänsten i bokningen. Avslutar du rummet avslutas hela bokningen." +
                                        $" Är du säker på att du vill avsluta bokning {Bokning.BokningsNr.Trim()}?", "Yes", MessageBoxButtons.YesNo);

                                    switch (dr)
                                    {
                                        case DialogResult.Yes:
                                            bk.AvslutaBokning(Bokning);
                                            Faktura f = bk.HämtaFaktura(Bokning);
                                            MessageBox.Show($"Bokning med bokningsnr {Bokning.BokningsNr.Trim()} har nu avslutats. " +
                                            $"Fakturan har uppdaterats med en efterskottsbelopp på {f.DelBelopp}. Förfallodatum: {f.FörfalloDatumEfterBokning.ToString("dd-MM-yyyy")}");
                                            BokadeRum.Clear();
                                            BokadLektion.Clear();
                                            BokadUtrustning.Clear();
                                            dataGridViewTjänster.DataSource = null;
                                            dataGridViewTjänster.Refresh();
                                            break;
                                        case DialogResult.No:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                rk.AvslutaRumBokning(ValtRum, Bokning, rBokning);
                                dataGridViewTjänster.DataSource = null;
                                dataGridViewTjänster.Refresh();
                                dataGridViewTjänster.DataSource = BokadeRum;
                                GridDesignRum();
                                MessageBox.Show($"Rum med Rumsnr {ValtRum.RumsNr} har nu rapporterats som avslutad.");
                            }
                        }
                        else if(rBokning.Avslutad == true)
                        {
                            MessageBox.Show("Rumbokningen har redan rapporterats som avslutad.");
                        }
                        else
                        {

                            MessageBox.Show("Rumbokningen är inte möjlig att avsluta innan rumsbokningen har startat. Vill du avbryta bokningen, klicka istället på ta bort (OBS! Endast möjligt om avbokningsskydd ingår).");

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void GridDesignUtrustning()
        {
            dataGridViewTjänster.Columns["Tillgänglig"].Visible = false;
            dataGridViewTjänster.Columns["UtrustningBokning"].Visible = false;
            dataGridViewTjänster.Columns["Pris"].Visible = false;
            dataGridViewTjänster.AllowUserToOrderColumns = true;
            BokadUtrustning.Sort(
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
        private void GridDesignLektion()
        {
            dataGridViewTjänster.Columns["Tillgänglig"].Visible = false;
            dataGridViewTjänster.Columns["LektionBokning"].Visible = false;
            dataGridViewTjänster.Columns["Pris"].Visible = false;
            dataGridViewTjänster.AllowUserToOrderColumns = true;
            BokadLektion.Sort(
            delegate (Lektion p1, Lektion p2)
            {
                int compare = p1.Typ.CompareTo(p2.Typ);
                if (compare == 0)
                {
                    return p2.Benämning.CompareTo(p1.Benämning);
                }
                return compare;
            });
        }

        private void GridDesignRum()
        {
            dataGridViewTjänster.Columns["Tillgänglig"].Visible = false;
            dataGridViewTjänster.Columns["Pris"].Visible = false;
            dataGridViewTjänster.Columns["Beskrivning"].Visible = false;
            dataGridViewTjänster.Columns["RumBokning"].Visible = false;
            dataGridViewTjänster.AllowUserToOrderColumns = true;
            BokadeRum.Sort(
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

        private void buttonTaBort_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValdUtrustning = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Utrustning;
                        uBokning = bk.HittauBokning(ValdUtrustning, Bokning);
                        if (uBokning.Utlämnad == false && uBokning.HyrdatumStart > DateTime.Now)
                        {
                            DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort utrustningen med benämning " +
                            $"{ValdUtrustning.Benämning.Trim()}?", "Yes", MessageBoxButtons.YesNo);


                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    bk.TaBortUtrustningBokning(uBokning, Bokning);
                                    MessageBox.Show($"Utrustning med benämning {ValdUtrustning.Benämning.Trim()} har nu raderats");
                                    BokadUtrustning.Remove(ValdUtrustning);
                                    dataGridViewTjänster.DataSource = null;
                                    dataGridViewTjänster.Refresh();
                                    dataGridViewTjänster.DataSource = BokadUtrustning;
                                    FyllUtrustningLista();
                                    GridDesignUtrustning();
                                    break;
                                case DialogResult.No:
                                    break;
                            }


                        }
                        else
                        {
                            MessageBox.Show("Utrustningen går inte att ta bort när den är utlämnad eller när hyrstartdatumet passerat. Klicka på Återlämnad om du vill rapportera utrustningen som återlämnad.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else if (buttonRum.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValtRum = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Rum;
                        rBokning = bk.HittarBokning(ValtRum, Bokning);

                        if (rBokning.Startad == false && rBokning.BokningsDatumStart > DateTime.Now)
                        {
                            if ((BokadeRum.Count() == 1 && (BokadLektion.Count() != 0 || BokadUtrustning.Count() != 0) || (ValtRum.Typ.Contains("Logi") || ValtRum.Typ.Contains("Camp")) && (BokadeRum.Any(t => t.Typ.Contains("Konferens") && BokadeRum.Where(t=>t.Typ.Contains("Logi") || t.Typ.Contains("Camp")).Count() == 1))))
                            {
                                MessageBox.Show("Bokningen innehåller andra tjänster som måste avbrytas först innan detta rum kan tas bort.");
                            }
                            else if (BokadeRum.Count() == 1 && (BokadLektion.Count() == 0 || BokadUtrustning.Count() == 0))
                            {
                                DialogResult dr = MessageBox.Show("Detta är den sista tjänsten i bokningen. Om du tar bort denna tjänst kommer hela bokningen att avbrytas. " +
                                    "Är du säker på att du vill avbryta bokningen?", "Yes", MessageBoxButtons.YesNo);

                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        if (Bokning.Avbokningsskydd == true)
                                        {
                                            bk.AvslutaBokning(Bokning);
                                            Faktura f = bk.HämtaFaktura(Bokning);
                                            MessageBox.Show($"Bokning med bokningsnr {Bokning.BokningsNr.Trim()} har nu avbrutits.");
                                            BokadeRum.Clear();
                                            BokadLektion.Clear();
                                            BokadUtrustning.Clear();
                                            dataGridViewTjänster.DataSource = null;
                                            dataGridViewTjänster.Refresh();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Rummet bokades utan avbokningsskydd och går därför inte att ta bort.");
                                        }

                                        break;
                                    case DialogResult.No:
                                        break;
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort rummet med rumsnr " +
                            $"{ValtRum.RumsNr}?", "Yes", MessageBoxButtons.YesNo);

                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        if (Bokning.Avbokningsskydd == true)
                                        {
                                            bk.TaBortRumBokning(rBokning, Bokning);
                                            MessageBox.Show($"Rum med rumsnr {ValtRum.RumsNr} har nu raderats");
                                            BokadeRum.Remove(ValtRum);
                                            dataGridViewTjänster.DataSource = null;
                                            dataGridViewTjänster.Refresh();
                                            dataGridViewTjänster.DataSource = BokadeRum;
                                            FyllRumLista();
                                            GridDesignRum();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Rummet bokades utan avbokningsskydd och går därför inte att ta bort.");
                                        }

                                        break;
                                    case DialogResult.No:
                                        break;
                                }
                            }



                        }
                        else
                        {
                            MessageBox.Show("Rummet går inte att ta bort när den är rumsbokningen är påbörjad. Klicka på avsluta om du vill avsluta rumsbokningen.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else if (buttonLektion.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValdLektion = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Lektion;
                        lBokning = lk.HittalBokning(ValdLektion, Bokning);
                        if (lBokning.LektionStartDatum > DateTime.Now)
                        {
                            DialogResult dr = MessageBox.Show($"Är du säker på att du vill ta bort lektionsbokningen med benämning " +
                            $"{ValdLektion.Benämning.Trim()}?", "Yes", MessageBoxButtons.YesNo);

                            switch (dr)
                            {
                                case DialogResult.Yes:
                                    bk.TaBortLektionBokning(lBokning, Bokning);
                                    MessageBox.Show($"Lektionsbokning med benämning {ValdLektion.Benämning.Trim()} har nu raderats");
                                    BokadLektion.Remove(ValdLektion);
                                    dataGridViewTjänster.DataSource = null;
                                    dataGridViewTjänster.Refresh();
                                    dataGridViewTjänster.DataSource = BokadLektion;
                                    FyllLektionLista();
                                    GridDesignLektion();
                                    break;
                                case DialogResult.No:
                                    break;
                            }


                        }
                        else
                        {
                            MessageBox.Show("Lektionsbokningen går inte att ta bort när startdatumet har passerat.");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }

                else
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }

            }

            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void buttonRum_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() == 2)
            {
                dataGridViewTjänster.DataSource = BokadeRum;
                buttonRum.BackColor = SystemColors.ControlLightLight;
                buttonUtrustning.BackColor = SystemColors.Control;
                buttonLektion.BackColor = SystemColors.Control;
                buttonÅterlämnad.Visible = true;
                buttonÅterlämnad.Text = "Avsluta";
                button1.Visible = true;
                button1.Text = "Påbörja";
                panelUtrustning.Visible = true;
                panelRum.Visible = true;
                panelLektion.Visible = false;
                BokadeRum = rk.HämtaBokadeRum(Bokning);
                FyllRumLista();
                GridDesignRum();
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


        }

        private void buttonUtrustning_Click(object sender, EventArgs e)
        {
            dataGridViewTjänster.DataSource = BokadUtrustning;
            buttonUtrustning.BackColor = SystemColors.ControlLightLight;
            buttonRum.BackColor = SystemColors.Control;
            buttonLektion.BackColor = SystemColors.Control;
            buttonÅterlämnad.Text = "Återlämnad";
            buttonÅterlämnad.Visible = true;
            button1.Text = "Utlämnad";
            button1.Visible = true;
            panelUtrustning.Visible = true;
            panelRum.Visible = false;
            panelLektion.Visible = false;
            BokadUtrustning = uk.HämtaBokadUtrustning(Bokning);
            FyllUtrustningLista();
            GridDesignUtrustning();
        }

        private void buttonLektion_Click(object sender, EventArgs e)
        {
            dataGridViewTjänster.DataSource = BokadLektion;
            buttonLektion.BackColor = SystemColors.ControlLightLight;
            buttonUtrustning.BackColor = SystemColors.Control;
            buttonRum.BackColor = SystemColors.Control;
            buttonÅterlämnad.Visible = false;
            button1.Visible = false;
            panelUtrustning.Visible = false;
            panelRum.Visible = true;
            panelLektion.Visible = true;
            BokadLektion = lk.HämtaBokadLektion(Bokning);
            FyllLektionLista();
            GridDesignLektion();
        }
        private void ValdBokningsvy_Load(object sender, EventArgs e)
        {
            dateTimePickerBstart.Format = DateTimePickerFormat.Custom;
            dateTimePickerBstart.CustomFormat = "MM/dd/yyyy hh:mm";

            dateTimePickerBslut.Format = DateTimePickerFormat.Custom;
            dateTimePickerBslut.CustomFormat = "MM/dd/yyyy hh:mm";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pk.HämtaBehörighet() <= 3 || pk.HämtaBehörighet() > 1)
            {
                if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValdUtrustning = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Utrustning;
                        uBokning = bk.HittauBokning(ValdUtrustning, Bokning);
                        if (uBokning.Utlämnad == false && uBokning.HyrdatumStart.Hour <= DateTime.Now.Hour && DateTime.Now.Hour >= uBokning.HyrdatumSlut.Hour)
                        {
                            if (uBokning.Återlämnad == true)
                            {
                                MessageBox.Show("Utrustningen är inte möjlig att lämna ut igen efter att den blivit återlämnad. En ny utrustningsbokning behöver göras.");
                            }
                            else
                            {
                                uk.LämnaUtUtrustning(uBokning);
                                dataGridViewTjänster.DataSource = null;
                                dataGridViewTjänster.Refresh();
                                dataGridViewTjänster.DataSource = BokadUtrustning;
                                GridDesignUtrustning();
                                MessageBox.Show($"Utrustning med utrustningsID {ValdUtrustning.Benämning.Trim()} har nu rapporterats som utlämnad. " +
                                    $"Utrustningen återlämnas senast {uBokning.HyrdatumSlut.ToString("dd-MM-yyyy")}");
                            }

                        }

                        else
                        {



                            MessageBox.Show("Utrustningen är inte möjlig att lämna ut innan bokningen har startat.");



                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else if (buttonRum.BackColor == SystemColors.ControlLightLight)
                {
                    try
                    {

                        ValtRum = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Rum;
                        rBokning = bk.HittarBokning(ValtRum, Bokning);
                        if (rBokning.Startad == false && rBokning.BokningsDatumStart <= DateTime.Now && DateTime.Now <= rBokning.BokningsDatumSlut)
                        {
                            rk.StartaRumBokning(rBokning);
                            dataGridViewTjänster.DataSource = null;
                            dataGridViewTjänster.Refresh();
                            dataGridViewTjänster.DataSource = BokadeRum;
                            GridDesignRum();
                            MessageBox.Show($"Rum med rumsnr {ValtRum.RumsNr} har nu rapporterats som Startad." +
                                $" Rumsnyckeln bör återlämnas senast {rBokning.BokningsDatumSlut.ToString("dd-MM-yyyy")}");
                        }
                        else
                        {

                            MessageBox.Show("Rumsbokningen är inte möjlig att starta innan bokningen har startat eller efter att bokningstiden har passerat.");

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ingen tjänst vald.");
                    }
                }
                else
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }
            }
            else
            {
                MessageBox.Show("Obehörig användare", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void buttonVälj_Click(object sender, EventArgs e)
        {
            if (buttonUtrustning.BackColor == SystemColors.ControlLightLight)
            {
                try
                {

                    ValdUtrustning = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Utrustning;
                    uBokning = bk.HittauBokning(ValdUtrustning, Bokning);
                    labelTyp.Visible = true;
                    labelTyp.Text = ValdUtrustning.Typ.Trim();
                    labelArtikel.Visible = true;
                    labelArtikel.Text = ValdUtrustning.UtrustningsArtikel.Trim();
                    labelStorlek.Visible = true;
                    labelStorlek.Text = ValdUtrustning.Storlek.ToString().Trim();
                    dateTimePickerHstart.Value = uBokning.HyrdatumStart;
                    dateTimePickerHslut.Value = uBokning.HyrdatumSlut;
                    textBoxKostnad.Text = uBokning.UtrustningPris.ToString();
                    if (uBokning.Utlämnad == true)
                    {
                        radioButtonUtlämnad.Checked = true;
                    }
                    else if (uBokning.Återlämnad == true)
                    {
                        radioButtonÅterlämnad.Checked = true;

                    }
                    else
                    {
                        radioButtonÅterlämnad.Checked = false;
                        radioButtonUtlämnad.Checked = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }

            }
            else if (buttonRum.BackColor == SystemColors.ControlLightLight)
            {
                try
                {

                    ValtRum = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Rum;
                    rBokning = bk.HittarBokning(ValtRum, Bokning);
                    labelrtyp.Visible = true;
                    labelrtyp.Text = ValtRum.Typ.Trim();
                    labelrNr.Visible = true;
                    labelrNr.Text = ValtRum.RumsNr.ToString().Trim();
                    labelrstorlek.Visible = true;
                    labelrstorlek.Text = ValtRum.RumsStorlek.Trim();
                    dateTimePickerBstart.Value = rBokning.BokningsDatumStart;
                    dateTimePickerBslut.Value = rBokning.BokningsDatumSlut;
                    textBoxRumKostnad.Text = rBokning.RumPris.ToString();
                    if (rBokning.Startad == true)
                    {
                        radioButtonPåbörjad.Checked = true;
                    }
                    else if (rBokning.Avslutad == true)
                    {
                        radioButtonAvslutad.Checked = true;

                    }
                    else
                    {
                        radioButtonPåbörjad.Checked = false;
                        radioButtonAvslutad.Checked = false;
                    }
                }
                catch
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }
            }
            else if (buttonLektion.BackColor == SystemColors.ControlLightLight)
            {
                try
                {

                    ValdLektion = dataGridViewTjänster.SelectedRows[0].DataBoundItem as Lektion;
                    lBokning = lk.HittalBokning(ValdLektion, Bokning);
                    labelLtyp.Visible = true;
                    labelLtyp.Text = ValdLektion.Typ.Trim();
                    labelPlatser.Visible = true;
                    labelPlatser.Text = lBokning.AntalPersoner.ToString().Trim();
                    labelGrupp.Visible = true;
                    labelGrupp.Text = ValdLektion.Grupp.Trim();
                    labelStart.Visible = true;
                    labelStart.Text = lBokning.LektionStartDatum.ToString("dd-MM-yyyy");
                    labelSlut.Visible = true;
                    labelSlut.Text = lBokning.LektionSlutDatum.ToString("dd-MM-yyyy");
                    labelLnamn.Visible = true;
                    textBoxLektionKostnad.Text = lBokning.LektionPris.ToString();
                    List<Personal> p = pk.HämtaLärare();
                    Personal l = p.FirstOrDefault(t => t.AnställningsNr.Trim().Equals(ValdLektion.AnställningsNr.Trim()));
                    labelLnamn.Text = l.FörNamn.Trim() + " " + l.EfterNamn.Trim();
                    labelanr.Visible = true;
                    labelanr.Text = ValdLektion.AnställningsNr;
                }
                catch
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }
            }
                else
                {
                    MessageBox.Show("Ingen tjänst vald.");
                }
            
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void panelRum_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
