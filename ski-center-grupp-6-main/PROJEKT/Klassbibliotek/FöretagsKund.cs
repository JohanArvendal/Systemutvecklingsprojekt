using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKICENTER
{
    public class FöretagsKund
    {
        [Key]
        public int FKundNr { get; set; }
        public long OrganisationsId { get; set; }
        public string FöretagsNamn { get; set; }
        public string Adress { get; set; }
        public int PostNr { get; set; }
        public long TelefonNr { get; set; }
        public int KreditGräns { get; set; }

        public string Ort { get; set; }

        public string Mail { get; set; }

        public bool Aktiv { get; set; }
        public virtual List<Bokning> Bokningar { get; set; }

        [ForeignKey("Personal")]
        public string AnställningsNr { get; set; }
        public virtual Personal Personal { get; set; }

        public FöretagsKund(bool Aktiv, int FKundNr, long OrganisationsId, string FöretagsNamn, string Adress, int PostNr, long TelefonNr, int KreditGräns, string Ort, string Mail, Personal Personal)
        {
            this.FKundNr = FKundNr;
            this.OrganisationsId = OrganisationsId;
            this.FöretagsNamn = FöretagsNamn;
            this.Adress = Adress;
            this.PostNr = PostNr;
            this.TelefonNr = TelefonNr;
            this.KreditGräns = KreditGräns;
            this.Ort = Ort;
            this.Mail = Mail;
            this.Aktiv = Aktiv;
            this.Personal = Personal;
        }
        public FöretagsKund() { }
    }
}
