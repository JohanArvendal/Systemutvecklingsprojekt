using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKICENTER
{
    public class Faktura
    {
        [Key]
        public long FakturaNr { get; set; }
        public DateTime FakturaDatum { get; set; }
        public Nullable<double> Summa { get; set; }
        public Nullable<double> DelBelopp { get; set; }
        public double Moms { get; set; }
        public int Rabatt { get; set; }
        public DateTime FörfalloDatumFöreBokning { get; set; }
        public DateTime FörfalloDatumEfterBokning { get; set; }

        [ForeignKey("Bokning")]
        public string BokningsNr { get; set; }
        public virtual Bokning Bokning { get; set; }
        public bool Aktiv { get; set; }

        public Faktura(long FakturaNr, DateTime FakturaDatum, double Summa, double Moms, int Rabatt, DateTime FörfalloDatumFöreBokning, DateTime FörfalloDatumEfterBokning, string BokningsNr, bool Aktiv, Nullable<double> DelBelopp)
        {
            this.FakturaNr = FakturaNr;
            this.FakturaDatum = FakturaDatum;
            this.Summa = Summa;
            this.Moms = Moms;
            this.Rabatt = Rabatt;
            this.FörfalloDatumFöreBokning = FörfalloDatumFöreBokning;
            this.FörfalloDatumEfterBokning = FörfalloDatumEfterBokning;
            this.BokningsNr = BokningsNr;
            this.Aktiv = Aktiv;
            this.DelBelopp = DelBelopp;
        }
        public Faktura() { }

    }
}
