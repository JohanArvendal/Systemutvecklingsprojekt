using System;

namespace SKICENTER
{
    public class Tjänst
    {
        public string Benämning { get; set; }
        public Nullable<int> Pris { get; set; }
        public string Typ { get; set; }
        public bool Tillgänglig { get; set; }
        public string RegistreradAv { get; set; }

        public Tjänst(string Benämning, int Pris, string Typ, bool Tillgänglig, string RegistreradAv)
        {
            this.Benämning = Benämning;
            this.Pris = Pris;
            this.Typ = Typ;
            this.Tillgänglig = Tillgänglig;
            this.RegistreradAv = RegistreradAv;
        }
        public Tjänst() { }
    }
}
