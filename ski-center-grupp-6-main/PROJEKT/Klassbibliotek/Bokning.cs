using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SKICENTER
{
    public class Bokning
    {
        [Key]
        public string BokningsNr { get; set; }
        public DateTime FrånDatum { get; set; }
        public DateTime TillDatum { get; set; }
        public virtual ICollection<RumBokning> RumBokning { get; set; }
        public virtual ICollection<UtrustningBokning> UtrustningBokning { get; set; }

        [ForeignKey("Personal")]
        public string AnställningsNr { get; set; }
        public virtual Personal Personal { get; set; }

        [ForeignKey("Kund")]
        public int KundNr { get; set; }
        public virtual Kund Kund { get; set; }
        public virtual ICollection<LektionBokning> LektionBokning { get; set; }
        public bool Avbokningsskydd { get; set; }
        public bool Aktiv { get; set; }
        [ForeignKey("FöretagsKund")]
        public int FKundNr { get; set; }
        public virtual FöretagsKund FKund { get; set; }
        public bool Godkänd { get; set; }
        public virtual Faktura Faktura { get; set; }
        public Bokning(string BokningsNr, DateTime FrånDatum, DateTime TillDatum, Personal Personal, Kund Kund, bool Avbokningsskydd, bool Aktiv, FöretagsKund FKund, bool Godkänd)
        {
            this.BokningsNr = BokningsNr;
            this.FrånDatum = FrånDatum;
            this.TillDatum = TillDatum;
            this.Personal = Personal;
            this.Kund = Kund;
            this.Avbokningsskydd = Avbokningsskydd;
            this.Aktiv = Aktiv;
            this.FKund = FKund;
            this.Godkänd = Godkänd;
        }
        /// <summary>
        /// Skapar bokningsnummer
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            Random random = new Random();
        const string chars = "ADBCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public Bokning() { }




    }
}
