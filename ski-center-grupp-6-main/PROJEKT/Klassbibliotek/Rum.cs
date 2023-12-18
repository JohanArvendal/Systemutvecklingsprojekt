using System.Collections.Generic;

namespace SKICENTER
{
    public class Rum : Tjänst
    {
        public int RumsNr { get; set; }
        public string RumsStorlek { get; set; }
        public string Beskrivning { get; set; }
        public string Prestanda { get; set; }
        public virtual ICollection<RumBokning> RumBokning { get; set; }

        public Rum(int RumsNr, string Beskrivning, string RumsStorlek, bool Tillgänglig, string Benämning, string Typ, string RegistreradAv, string Prestanda, int Pris) : base(Benämning, Pris, Typ, Tillgänglig, RegistreradAv)
        {
            this.RumsNr = RumsNr;
            this.Beskrivning = Beskrivning;
            this.RumsStorlek = RumsStorlek;
            this.Tillgänglig = Tillgänglig;
            this.Prestanda = Prestanda;
        }

        public Rum() : base() { }
    }
}
