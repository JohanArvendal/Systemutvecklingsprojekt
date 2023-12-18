namespace SKICENTER
{
    public class KonferensPris : RumPris
    {
        public int Dygnpris1 { get; set; }
        public int Dygnpris2 { get; set; }
        public int Dygnpris3 { get; set; }
        public int Timpris1 { get; set; }
        public int Timpris2 { get; set; }
        public int Timpris3 { get; set; }

        public KonferensPris(string Typ, string Storlek, int Veckopris1, int Veckopris2, int Veckopris3, Rum Rum) : base(Typ, Storlek, Veckopris1, Veckopris2, Veckopris3, Rum)
        {
            this.Dygnpris1 = Dygnpris1;
            this.Dygnpris2 = Dygnpris2;
            this.Dygnpris3 = Dygnpris3;
            this.Timpris1 = Timpris1;
            this.Timpris2 = Timpris2;
            this.Timpris3 = Timpris3;
        }
        public KonferensPris() { }
        public int HämtaKonferensCampPris(int vecka)
        {
            if (vecka >= 5 && vecka <= 50)
            {
                return 3;
            }
            else if (vecka == 1 || vecka == 7 || vecka == 8 || vecka == 13)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }
}
