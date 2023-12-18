using System.ComponentModel.DataAnnotations;

namespace SKICENTER
{
    public class Kund : Person
    {
        [Key]
        public int KundNr { get; set; }
        public int KreditGräns { get; set; }

        /*[ForeignKey("Bokning")]
        public string BokningsNr { get; set; }
        public virtual Bokning Bokning { get; set; }*/

        public Kund(int KundNr, int KreditGräns, string FörNamn, string EfterNamn, string Adress, long PersonNr, int PostNr, long TelefonNr, string Ort, string Mail) : base(FörNamn, EfterNamn, Adress, PersonNr, PostNr, TelefonNr, Ort, Mail)
        {
            this.KundNr = KundNr;
            this.KreditGräns = KreditGräns;
        }
        public Kund() : base() { }

    }
}
