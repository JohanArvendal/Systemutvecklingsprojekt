using System.ComponentModel.DataAnnotations.Schema;

namespace SKICENTER
{
    public class LektionPris
    {
        public string Typ { get; set; }
        public string Grupp { get; set; }
        public int MånOnsPris { get; set; }
        public int TorsFrePris { get; set; }
        public int PrivatlektionPris { get; set; }
        [NotMapped]
        public virtual Lektion Lektion { get; set; }

        public LektionPris(string Typ, string Grupp, int MånOnsPris, int TorsFrePris, int PrivatlektionPris)
        {
            this.Typ = Typ;
            this.Grupp = Grupp;
            this.MånOnsPris = MånOnsPris;
            this.TorsFrePris = TorsFrePris;
            this.PrivatlektionPris = PrivatlektionPris;
        }

        public LektionPris() { }
    }
}
