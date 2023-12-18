using System.Collections.Generic;

namespace SKICENTER
{
    public class Utrustning : Tjänst
    {
        public string UtrustningsArtikel { get; set; }
        public int Storlek { get; set; }
        public virtual ICollection<UtrustningBokning> UtrustningBokning { get; set; }


        public Utrustning(string UtrustningsArtikel, int Storlek, string Benämning, int Pris, string Typ, bool Tillgänglig, string RegistreradAv) : base(Benämning, Pris, Typ, Tillgänglig, RegistreradAv)
        {
            this.Storlek = Storlek;
            this.UtrustningsArtikel = UtrustningsArtikel;

        }

        public Utrustning() : base() { }
    }
}
