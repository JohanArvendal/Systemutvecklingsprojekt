using System;
using System.Collections.Generic;

namespace SKICENTER
{
    public class Lektion : Tjänst
    {
        public int AntalPlatser { get; set; }
        public string LektionStart { get; set; }
        public string LektionSlut { get; set; }
        public string AnställningsNr { get; set; }
        public virtual Personal Lärare { get; set; }
        public TimeSpan Tid { get; set; }
        public string Grupp { get; set; }

        public virtual ICollection<LektionBokning> LektionBokning { get; set; }

        public Lektion(string Grupp, string Benämning, string LektionStartDatum, string LektionSlutDatum, int AntalPersoner, string Typ, bool Tillgänglig, string RegistreradAv, string AnställningsNr, int Pris, TimeSpan Tid) : base(Benämning, Pris, Typ, Tillgänglig, RegistreradAv)
        {

            this.AnställningsNr = AnställningsNr;
            this.Grupp = Grupp;
            this.AntalPlatser = AntalPersoner;
            this.LektionSlut = LektionSlutDatum;
            this.LektionStart = LektionStartDatum;
            this.RegistreradAv = RegistreradAv;
            this.Tid = Tid;

        }

        public Lektion() : base() { }
    }
}
