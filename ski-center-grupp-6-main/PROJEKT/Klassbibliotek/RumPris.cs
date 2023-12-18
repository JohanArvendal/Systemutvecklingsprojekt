using System.ComponentModel.DataAnnotations.Schema;

namespace SKICENTER
{
    public class RumPris
    {
        public string Typ { get; set; }
        public string Storlek { get; set; }
        public int Veckopris1 { get; set; }
        public int Veckopris2 { get; set; }
        public int Veckopris3 { get; set; }
        [NotMapped]
        public virtual Rum Rum { get; set; }
        public RumPris(string Typ, string Storlek, int Veckopris1, int Veckopris2, int Veckopris3, Rum Rum)
        {
            this.Typ = Typ;
            this.Storlek = Storlek;
            this.Veckopris1 = Veckopris1;
            this.Veckopris2 = Veckopris2;
            this.Veckopris3 = Veckopris3;
            this.Rum = Rum;
        }

        public RumPris() { }
    }
}
