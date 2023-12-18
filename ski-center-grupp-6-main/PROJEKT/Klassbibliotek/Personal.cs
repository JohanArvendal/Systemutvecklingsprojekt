using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SKICENTER
{
    public class Personal : Person
    {
        [Key]
        public string AnställningsNr { get; set; }
        public string Lösenord { get; set; }
        public int Behörighet { get; set; }
        public string Roll { get; set; }
        public string RegistreradAv { get; set; }
        public virtual ICollection<Lektion> Lektion { get; set; }

        public Personal(string AnställningsNr, string Lösenord, int Behörighet, string Roll, string FörNamn, string EfterNamn, string Adress, long PersonNr, int PostNr, long TelefonNr, string Ort, string Mail, string RegistreradAv) : base(FörNamn, EfterNamn, Adress, PersonNr, PostNr, TelefonNr, Ort, Mail)
        {
            this.AnställningsNr = AnställningsNr;
            this.Lösenord = Lösenord;
            this.Behörighet = Behörighet;
            this.Roll = Roll;
            this.RegistreradAv = RegistreradAv;
        }
        public Personal() { }

    }
}
