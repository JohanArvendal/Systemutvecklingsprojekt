namespace SKICENTER
{
    public class LogiPris : RumPris
    {
        public int Veckopris4 { get; set; }
        public int Veckopris5 { get; set; }
        public int Veckopris6 { get; set; }
        public int SönFreDygnPris1 { get; set; }
        public int SönFreDygnPris2 { get; set; }
        public int SönFreDygnPris3 { get; set; }
        public int SönFreDygnPris4 { get; set; }
        public int SönFreDygnPris5 { get; set; }
        public int SönFreDygnPris6 { get; set; }
        public int FreSönDygnPris1 { get; set; }
        public int FreSönDygnPris2 { get; set; }
        public int FreSönDygnPris3 { get; set; }
        public int FreSönDygnPris4 { get; set; }
        public int FreSönDygnPris5 { get; set; }
        public int FreSönDygnPris6 { get; set; }



        public LogiPris(string Typ, string Storlek, int Veckopris1, int Veckopris2, int Veckopris3, int Veckopris4, int Veckopris5, int Veckopris6, int SönFreDygnPris1, int SönFreDygnPris2, int SönFreDygnPris3, int SönFreDygnPris4, int SönFreDygnPris5, int SönFreDygnPris6, int FreSönDygnPris1, int FreSönDygnPris2, int FreSönDygnPris3, int FreSönDygnPris4, int FreSönDygnPris5, int FreSönDygnPris6, Rum Rum) : base(Typ, Storlek, Veckopris1, Veckopris2, Veckopris3, Rum)
        {
            this.Veckopris4 = Veckopris4;
            this.Veckopris5 = Veckopris5;
            this.Veckopris6 = Veckopris6;
            this.SönFreDygnPris1 = SönFreDygnPris1;
            this.SönFreDygnPris2 = SönFreDygnPris2;
            this.SönFreDygnPris3 = SönFreDygnPris3;
            this.SönFreDygnPris4 = SönFreDygnPris4;
            this.SönFreDygnPris5 = SönFreDygnPris5;
            this.SönFreDygnPris6 = SönFreDygnPris6;
            this.FreSönDygnPris1 = FreSönDygnPris1;
            this.FreSönDygnPris2 = FreSönDygnPris2;
            this.FreSönDygnPris3 = FreSönDygnPris3;
            this.FreSönDygnPris4 = FreSönDygnPris4;
            this.FreSönDygnPris5 = FreSönDygnPris5;
            this.FreSönDygnPris6 = FreSönDygnPris6;
        }
        public LogiPris() { }


        public int HämtaLogiPris(int vecka)
        {
            if (vecka == 5 || vecka == 6)
            {
                return 4;
            }
            else if (vecka == 7 || vecka == 8)
            {
                return 1;
            }
            else if (vecka >= 23 && vecka <= 50)
            {
                return 6;
            }
            else if (vecka >= 10 && vecka <= 12)
            {
                return 3;
            }
            else if ((vecka == 1 || vecka == 9 || vecka == 13))
            {
                return 2;
            }
            else
            {
                return 5;
            }

        }

    }
}
