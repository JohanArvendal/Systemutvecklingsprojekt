using System.ComponentModel.DataAnnotations.Schema;

namespace SKICENTER
{
    public class UtrustningPris
    {
        public string Typ { get; set; }
        public string Artikel { get; set; }
        public int Pris1 { get; set; }
        public int Pris2 { get; set; }
        public int Pris3 { get; set; }
        public int Pris4 { get; set; }
        public int Pris5 { get; set; }
        [NotMapped]
        public virtual Utrustning Utrustning { get; set; }

        public UtrustningPris(string Typ, string Artikel, int Pris1, int Pris2, int Pris3, int Pris4, int Pris5)
        {
            this.Typ = Typ;
            this.Artikel = Artikel;
            this.Pris1 = Pris1;
            this.Pris2 = Pris2;
            this.Pris3 = Pris3;
            this.Pris4 = Pris4;
            this.Pris5 = Pris5;

        }

        public UtrustningPris() { }

        public int HämtaUtrustningPris(int dag)
        {
            if (dag == 1)
            {
                return 1;
            }
            else if (dag == 2)
            {
                return 2;
            }
            else if (dag == 3)
            {
                return 3;
            }
            else if (dag == 4)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }
}
