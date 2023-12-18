namespace SKICENTER
{
    public class Person
    {
        public string FörNamn { get; set; }
        public string EfterNamn { get; set; }
        public string Adress { get; set; }
        public long PersonNr { get; set; }
        public int PostNr { get; set; }
        public long TelefonNr { get; set; }
        public string Ort { get; set; }
        public string Mail { get; set; }

        public Person(string FörNamn, string EfterNamn, string Adress, long PersonNr, int PostNr, long TelefonNr, string Ort, string Mail)
        {
            this.FörNamn = FörNamn;
            this.EfterNamn = EfterNamn;
            this.Adress = Adress;
            this.PersonNr = PersonNr;
            this.PostNr = PostNr;
            this.TelefonNr = TelefonNr;
            this.Ort = Ort;
            this.Mail = Mail;

        }
        public Person() { }

    }
}
