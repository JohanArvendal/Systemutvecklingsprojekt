using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class BokningKontroller
    {
        public Personal Inloggad { get; private set; }
        List<Bokning> BokningPåBevakning = new List<Bokning>();
        public List<Rum> BokatRum { get; set; }
        public List<Lektion> BokadLektion { get; set; }
        public List<Utrustning> BokadUtrustning { get; set; }
        public List<UtrustningBokning> UtrustningBokningar { get; set; }
        public List<LektionBokning> LektionBokningar { get; set; }
        public List<RumBokning> RumBokningar { get; set; }
        public static Bokning PågåendeBokning { get; set; }
        public static int PågåendeKostnad { get; set; }
        public static Bokning LagdBokning { get; set; }
        private static Random random = new Random();
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();
        UnitOfWork unitOfWork = new UnitOfWork();
        //PersonalKontroller pk = new PersonalKontroller();

        /// <summary>
        /// Hämtar inloggad personal från personalkontroller
        /// </summary>
       /* public void HämtaInloggad()
        {
            Inloggad = pk.SparaPersonal();
        }*/

        /// <summary>
        /// Hämtar pågeånde rumbokning
        /// </summary>
        /// <returns></returns>
        public List<RumBokning> HämtaPågåenderBokning()
        {
            return RumBokningar;
        }
        /// <summary>
        /// Hämtar rum som är lagda för bokning men inte bokade än
        /// </summary>
        /// <returns></returns>
        public List<Rum> HämtaPågåendeRum()
        {
            return BokatRum;
        }

        /// <summary>
        /// Ändra lektionsbokning 
        /// </summary>
        /// <param name="kostnad"></param>
        public void ÄndraLektionsBokning(string kostnad, LektionBokning lb, Bokning b)
        {
            Faktura f = HämtaFaktura(b);
            double intkostnad = int.Parse(kostnad);
            f.Summa -= lb.LektionPris;
            lb.LektionPris = ((int)intkostnad);
            f.Summa += intkostnad;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.LektionBokningRepository.Update(lb);
            unitOfWork.Save();

        }

        /// <summary>
        /// tar bort lektionsbokning
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="lektion"></param>
        public void TaBortLektionBokning(LektionBokning lb, Bokning b)
        {
            Faktura f = HämtaFaktura(b);
            f.Summa -= lb.LektionPris;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.LektionBokningRepository.Delete(lb);
            unitOfWork.Save();
        }
        /// <summary>
        /// Tar bort utrustningsbokning (om den inte startats än)
        /// </summary>
        /// <param name="ub"></param>
        public void TaBortUtrustningBokning(UtrustningBokning ub, Bokning b)
        {
            Faktura f = HämtaFaktura(b);
            f.Summa -= ub.UtrustningPris;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.UtrustningBokningRepository.Delete(ub);
            unitOfWork.Save();

        }
        /// <summary>
        /// Ändrar datum eller kostnad för utrustningsbokning
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void ÄndraUtrustningsBokning(string kostnad, DateTime start, DateTime slut, UtrustningBokning ub, Bokning b)
        {
            Faktura f = HämtaFaktura(b);
            double intkostnad = int.Parse(kostnad);
            f.Summa -= ub.UtrustningPris;
            ub.UtrustningPris = ((int)intkostnad);
            f.Summa += intkostnad;
            ub.HyrdatumStart = start;
            ub.HyrdatumSlut = slut;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.UtrustningBokningRepository.Update(ub);
            unitOfWork.Save();
        }
        /// <summary>
        /// Hämtar rumbokning baserat på valt rum i gränssnittet
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public RumBokning HittarBokning(Rum valtRum, Bokning b)
        {
            RumBokning rb = unitOfWork.RumBokningRepository.Query(q => q).FirstOrDefault(t => t.RumId.Equals(valtRum.RumsNr) && t.BokningsId.Contains(b.BokningsNr));

            return rb;
        }
        /// <summary>
        /// Tar bort specifikt rum i bokning
        /// </summary>
        /// <param name="rb"></param>
        public void TaBortRumBokning(RumBokning rb, Bokning b)
        {
            Faktura f = HämtaFaktura(b);
            f.Summa -= rb.RumPris;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.RumBokningRepository.Delete(rb);
            unitOfWork.Save();
        }
        /// <summary>
        /// Ändrar datum eller kostnad för rumssbokning
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void ÄndraRumsBokning(string kostnad, DateTime start, DateTime slut, RumBokning rb, Bokning b)
        {
            double nykostnad = double.Parse(kostnad);
            Faktura f = HämtaFaktura(b);
            f.Summa -= rb.RumPris;
            rb.RumPris = ((int)nykostnad);
            f.Summa += nykostnad;
            if (f.DelBelopp != null && f.FörfalloDatumFöreBokning < DateTime.Now)
            {
                f.DelBelopp = f.Summa * 0.8;
            }
            rb.BokningsDatumStart = start;
            rb.BokningsDatumSlut = slut;
            if (start < b.FrånDatum)
            {
                b.FrånDatum = start;
            }
            if (slut > b.TillDatum)
            {
                b.TillDatum = slut;
            }
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.BokningRepository.Update(b);
            unitOfWork.RumBokningRepository.Update(rb);
            unitOfWork.Save();
        }
        /// <summary>
        /// Sparar befintlig vald bokning i arbetsminnet så man kan hämta den i andra gränssnitt
        /// </summary>
        /// <returns></returns>
        public void SkickaVidareBokning(Bokning b)
        {
            LagdBokning = b;
        }

        /// <summary>
        /// Hämtar lagd bokning och kopplat det till gränssnittet (användare väljer lägg till i befintlig bokning)
        /// </summary>
        /// <returns></returns>
        public Bokning HämtaBokning()
        {
            return LagdBokning;
        }
        /// <summary>
        /// Metod för att avsluta bokning efter kundens (förhoppningsvis) trevliga vistelse
        /// </summary>
        /// <param name="bokning"></param>
        public void AvslutaBokning(Bokning bokning)
        {
            Faktura f = HämtaFaktura(bokning);

            foreach (LektionBokning lb in unitOfWork.LektionBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                unitOfWork.LektionBokningRepository.Delete(lb);
            }
            foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                unitOfWork.UtrustningBokningRepository.Delete(ub);
            }
            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                if (rb.Avslutad != true)
                {
                    double sistaBet = f.Summa - f.DelBelopp ?? default(int);
                    f.DelBelopp = sistaBet;
                    unitOfWork.FakturaRepository.Update(f);
                }
                unitOfWork.RumBokningRepository.Delete(rb);
            }
            bokning.Aktiv = false;
            unitOfWork.BokningRepository.Update(bokning);
            unitOfWork.Save();
        }
        /// <summary>
        /// metod som avbryter bokning som har avbokningsskydd
        /// </summary>
        /// <param name="bokning"></param>
        public void AvbrytBokning(Bokning bokning)
        {
            Faktura f = HämtaFaktura(bokning);
            f.Aktiv = false;
            foreach (LektionBokning lb in unitOfWork.LektionBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                foreach (Lektion l in unitOfWork.LektionRepository.Query(q => q.Where(b => b.Benämning.Equals(lb.BokningsId))))
                {
                    unitOfWork.LektionBokningRepository.Delete(lb);
                }

            }

            foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                unitOfWork.UtrustningBokningRepository.Delete(ub);
            }

            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(b => b.BokningsId.Equals(bokning.BokningsNr))))
            {
                unitOfWork.RumBokningRepository.Delete(rb);
            }
            unitOfWork.BokningRepository.Delete(bokning);
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.Save();
        }
        /// <summary>
        /// Hämtar alla bokningar som inte har godkänts av marknadschef ännu
        /// </summary>
        /// <returns></returns>
        public List<Bokning> HittaBokningsBevakning()
        {
            BokningPåBevakning.Clear();
            foreach (Bokning b in unitOfWork.BokningRepository.Query(b => b).Where(b => b.Aktiv == true && b.Godkänd == false).ToList())
            {

                BokningPåBevakning.Add(b);


            }

            return BokningPåBevakning;
        }

        /// <summary>
        /// Metod för Marknadschef att godkänna företagskund
        /// </summary>
        /// <param name="valdKund"></param>
        /// <param name="kredgräns"></param>
        public void GodkännBokning(Bokning valdBokning, string kredgräns, string rabatt)
        {


            valdBokning.Godkänd = true;
            int kg = int.Parse(kredgräns);
            int r = int.Parse(rabatt);
            valdBokning.FKund.KreditGräns = kg;
            long nummer = RandomInt();

            Faktura f = HämtaFaktura(valdBokning);
            f.Aktiv = true;
            if (r != 0)
            {
                f.Rabatt = r;
                f.Summa -= r;
            }
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.BokningRepository.Update(valdBokning);

            int bNr = valdBokning.FKundNr;
            FöretagsKund fk = unitOfWork.FöretagsKundRepository.Query(q => q).FirstOrDefault(f => f.FKundNr.Equals(bNr));
            if (fk.Aktiv == false)
            {
                fk.Aktiv = true;
                unitOfWork.FöretagsKundRepository.Update(fk);
                unitOfWork.Save();
            }

            unitOfWork.Save();
        }

        /// <summary>
        /// Metod för Marknadschef att ändra kreditgräns och/eller rabatt för en företagskundsbokning
        /// </summary>
        /// <param name="valdKund"></param>
        /// <param name="kredgräns"></param>
        public void UppdateraBokning(string kredgräns, string rabatt, Bokning valdBokning)
        {
            int kg = int.Parse(kredgräns);
            valdBokning.FKund.KreditGräns = kg;
            //lägga till för rabatt när kostnad är löst
            unitOfWork.BokningRepository.Update(valdBokning);
            unitOfWork.Save();
        }
        /// <summary>
        /// Lägger till tjänst i redan befintlig bokning. 
        /// </summary>
        /// <param name="valtRum"></param>
        /// <returns></returns>
        public Bokning LäggTillIBokning(List<Lektion> valdLektion, List<Utrustning> valdUtrustning, List<Rum> valtRum, Bokning bokning, List<UtrustningBokning> ub, List<LektionBokning> lb, List<RumBokning> rb)
        {
            string bnr = bokning.BokningsNr;
            Faktura f = HämtaFaktura(bokning);
            double kostnad = 0;
            if (valtRum != null)
            {
                foreach (Rum r in valtRum)
                {
                    foreach (RumBokning b in rb.Where(x => x.RumId.Equals(r.RumsNr)))
                    {
                        b.BokningsId = bnr;
                        b.Avslutad = false;
                        b.Startad = false;
                        kostnad += b.RumPris;
                        if (b.BokningsDatumSlut > bokning.TillDatum)
                        {
                            bokning.TillDatum = b.BokningsDatumSlut;
                        }
                        if (b.BokningsDatumStart < bokning.FrånDatum)
                        {
                            bokning.FrånDatum = b.BokningsDatumStart;
                        }
                        unitOfWork.RumBokningRepository.Add(b);
                        unitOfWork.BokningRepository.Update(bokning);
                        unitOfWork.Save();
                    }
                    r.Pris = null;
                }
            }
            if (valdUtrustning != null)
            {

                foreach (Utrustning u in valdUtrustning)
                {
                    foreach (UtrustningBokning b in ub.Where(x => x.UtrustningId.Contains(u.Benämning)))
                    {
                        b.Återlämnad = false;
                        b.Utlämnad = false;
                        b.BokningsId = bnr;
                        kostnad += b.UtrustningPris;
                        unitOfWork.UtrustningBokningRepository.Add(b);
                        unitOfWork.Save();
                    }
                    u.Pris = null;
                }

            }
            if (valdLektion != null)
            {

                foreach (Lektion l in valdLektion)
                {
                    foreach (LektionBokning b in lb.Where(x => x.LektionId.Contains(l.Benämning)))
                    {

                        b.BokningsId = bnr;
                        kostnad += b.LektionPris;
                        unitOfWork.LektionBokningRepository.Add(b);
                        unitOfWork.Save();
                        l.AntalPlatser += b.AntalPersoner;
                    }
                    l.Pris = null;
                }

            }
            f.Summa += kostnad;
            double moms = f.Summa * 0.1071 ?? default(double);
            double m = Math.Ceiling(moms);
            f.Moms = m;
            unitOfWork.FakturaRepository.Update(f);
            unitOfWork.Save();
            LagdBokning = null;
            return bokning;
        }
        /// <summary>
        /// Sparar tillfälligt kund- och bokningsuppgifter så att man kan lägga till fler tjänster i pågående logibokningsregistrering
        /// </summary>
        /// <param name="valtRum"></param>
        /// <param name="bokning"></param>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <param name="kund"></param>
        /// <param name="fKund"></param>
        public void HämtaBokningsinnehåll(List<Rum> valtRum, List<Lektion> valdLektion, List<Utrustning> valdUtrustning, Bokning bokning, DateTime fDatum, DateTime tDatum, Kund kund, FöretagsKund fKund, bool avbok, List<UtrustningBokning> ub, List<LektionBokning> lb, List<RumBokning> rb, int kostnad)
        {
            if (BokatRum == null)
            {
                BokatRum = new List<Rum>();
            }
            if (RumBokningar == null)
            {
                RumBokningar = new List<RumBokning>();
            }
            if (valtRum != null)
            {
                foreach (Rum l in valtRum.ToList())
                {
                    if (!BokatRum.Contains(l))
                    {
                        BokatRum.Add(l);
                    }
                    if (rb != null)
                    {
                        foreach (RumBokning b in rb.ToList())
                        {
                            if (!RumBokningar.Contains(b))
                            {
                                RumBokningar.Add(b);
                            }
                        }
                    }


                }
            }

            if (BokadLektion == null)
            {
                BokadLektion = new List<Lektion>();
            }
            if (LektionBokningar == null)
            {
                LektionBokningar = new List<LektionBokning>();
            }
            if (valdLektion != null)
            {
                foreach (Lektion l in valdLektion.ToList())
                {
                    if (!BokadLektion.Contains(l))
                    {
                        BokadLektion.Add(l);
                    }
                }
                if (lb != null)
                {
                    foreach (LektionBokning b in lb.ToList())
                    {
                        if (!LektionBokningar.Contains(b))
                        {
                            LektionBokningar.Add(b);
                        }
                    }
                }

            }


            if (BokadUtrustning == null)
            {
                BokadUtrustning = new List<Utrustning>();
            }
            if (UtrustningBokningar == null)
            {
                UtrustningBokningar = new List<UtrustningBokning>();
            }
            if (valdUtrustning != null)
            {
                foreach (Utrustning u in valdUtrustning.ToList())
                {
                    if (!BokadUtrustning.Contains(u))
                    {
                        BokadUtrustning.Add(u);
                    }
                }
                if (ub != null)
                {
                    foreach (UtrustningBokning b in ub.ToList())
                    {
                        if (!UtrustningBokningar.Contains(b))
                        {
                            UtrustningBokningar.Add(b);
                        }
                    }
                }

            }

            bool aktiv = true;
            bool godkänd = true;
            string bnr = null;
            if (fKund != null)
            {
                godkänd = false;
            }
            PågåendeKostnad += kostnad;
            PågåendeBokning = new Bokning(bnr, fDatum, tDatum, Inloggad, kund, avbok, aktiv, fKund, godkänd);

        }
        /// <summary>
        /// Hämtar pågående bokning i nästa gränssnitt så att man kan fortsätta lägga till tjänster i pågående bokning
        /// </summary>
        /// <returns></returns>
        public Bokning HämtaPågåendeBokning()
        {
            return PågåendeBokning;
        }

        /// <summary>
        /// Skapar bokning på logi/konferensrum för privatkund och skapar bevakad bokning för företagskund (när marknadschef godkänner bokning via ärende ändras status och godkänd till true)
        /// </summary>
        /// <param name="rum"></param>
        /// <param name="kund"></param>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <param name="lektion"></param>
        /// <param name="utrustning"></param>
        /// <param name="kostnad"></param>
        /// <param name="moms"></param>
        /// <param name="rabatt"></param>
        /// <param name="avbokning"></param>
        /// <returns></returns>
        public Bokning SkapaBokning(ICollection<Rum> rum, Kund kund, FöretagsKund fKund, DateTime fDatum, DateTime tDatum, List<Lektion> lektion, List<Utrustning> utrustning, int rabatt, bool avbokning, string bokNr, List<UtrustningBokning> ub, List<LektionBokning> lb, List<RumBokning> rb)
        {

            if (bokNr == null)
            {
                bokNr = Bokning.RandomString(4);
            }
            long nummer = RandomInt();
            int delbelopp = 0;
            int kostnad = 0;
            foreach (Rum r in rum)
            {
                foreach (RumBokning b in rb.Where(x => x.RumId.Equals(r.RumsNr)))
                {
                    b.BokningsId = bokNr;
                    b.Startad = false;
                    b.Avslutad = false;
                    if (r.Typ.Contains("Logi"))
                    {
                        delbelopp += b.RumPris;
                        kostnad += b.RumPris;
                    }
                    unitOfWork.RumBokningRepository.Add(b);
                    unitOfWork.Save();
                    if (b.BokningsDatumSlut > tDatum)
                    {
                        tDatum = b.BokningsDatumSlut;
                    }
                    if (b.BokningsDatumStart < fDatum)
                    {
                        fDatum = b.BokningsDatumStart;
                    }
                    r.Pris = null;
                }
            }
            if (utrustning.Any())
            {
                foreach (Utrustning u in utrustning)
                {
                    foreach (UtrustningBokning b in ub.Where(x => x.UtrustningId.Contains(u.Benämning)))
                    {
                        b.BokningsId = bokNr;
                        b.Utlämnad = false;
                        b.Återlämnad = false;
                        kostnad += b.UtrustningPris;
                        unitOfWork.UtrustningBokningRepository.Add(b);
                        unitOfWork.Save();
                    }
                    u.Pris = null;

                }
            }
            if (lektion.Any())
            {
                foreach (Lektion l in lektion)
                {
                    foreach (LektionBokning b in lb.Where(x => x.LektionId.Contains(l.Benämning)))
                    {
                        b.BokningsId = bokNr;
                        kostnad += b.LektionPris;
                        unitOfWork.LektionBokningRepository.Add(b);
                        unitOfWork.Save();
                        l.AntalPlatser += b.AntalPersoner;
                    }
                    l.Pris = null;

                }
            }
            if (avbokning == true)
            {
                kostnad += 300;
            }
            DateTime idag = DateTime.Now.AddDays(-30);
            if ((fDatum - DateTime.Now).TotalDays > 30)
            {
                fDatum = idag;
            }
            bool status = true;
            bool godkänd = true;
            double moms = kostnad * 0.1071;
            double m = Math.Ceiling(moms);
            Faktura faktura = new Faktura(nummer, DateTime.Now, kostnad, moms, rabatt, fDatum, tDatum.AddDays(30), bokNr, status, delbelopp * 0.8);
            if (kund != null)
            {
                Bokning kundbokning = new Bokning(bokNr, fDatum, tDatum, Inloggad, kund, avbokning, status, fKund, godkänd);
                unitOfWork.FakturaRepository.Add(faktura);
                unitOfWork.BokningRepository.Add(kundbokning);
                unitOfWork.Save();
                ÅngraPågåendeBokning();
                return kundbokning;
            }
            else
            {
                faktura.Aktiv = false;
                godkänd = false;
                Bokning fKundbokning = new Bokning(bokNr, fDatum, tDatum, Inloggad, kund, avbokning, status, fKund, godkänd);
                unitOfWork.FakturaRepository.Add(faktura);
                unitOfWork.BokningRepository.Add(fKundbokning);
                unitOfWork.Save();
                ÅngraPågåendeBokning();
                return fKundbokning;

            }




        }

        /// <summary>
        /// Tar bort all data från listorna när bokningen är slutförd
        /// </summary>
        public void ÅngraPågåendeBokning()
        {
            PågåendeBokning = null;
            if (BokatRum != null)
            {
                BokatRum.Clear();
            }
            if (BokadLektion != null)
            {
                BokadLektion.Clear();
            }
            if (BokadUtrustning != null)
            {
                BokadUtrustning.Clear();
            }



        }
        /// <summary>
        /// Hämtar alla befintliga bokningar från databas och skickar de till gränssnittet
        /// </summary>
        /// <returns></returns>
        public List<Bokning> HämtaAllaBokningar(bool aktiv)
        {
            List<Bokning> HämtaBokning = new List<Bokning>();

            foreach (Bokning l in unitOfWork.BokningRepository.Query(q => q.Where(b => b.Aktiv.Equals(aktiv))).ToList())
            {
                HämtaBokning.Add(l);
            }
            return HämtaBokning;


        }
        /// <summary>
        /// Hämtar bokningar baserat på inmatad input från databas och skickar de till gränssnittet
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public List<Bokning> HämtaEnBokning(string input, bool aktiv)
        {
            List<Bokning> bokning = new List<Bokning>();

            bokning = unitOfWork.BokningRepository.Query(q => q).Where(b => b.Aktiv.Equals(aktiv) && (b.BokningsNr.Equals(input))).ToList();

            if (!bokning.Any())
            {
                int i = int.Parse(input);
                bokning = unitOfWork.BokningRepository.Query(q => q).Where(b => b.Aktiv.Equals(aktiv) && b.FKundNr.Equals(i) || b.KundNr.Equals(i)).ToList();
            }



            return bokning;
        }
        /// <summary>
        /// KOntroller och hämtar aktiv bokning för kund
        /// </summary>
        /// <param name="k"></param>
        /// <param name="fk"></param>
        /// <returns></returns>
        public Bokning HittaKundBokning(Kund k, FöretagsKund fk)
        {
            Bokning b = new Bokning();
            if (k != null)
            {
                b = unitOfWork.BokningRepository.Query(q => q).FirstOrDefault(b => b.Aktiv == true && (b.KundNr.Equals(k.KundNr)));
            }
            else
            {
                b = unitOfWork.BokningRepository.Query(q => q).FirstOrDefault(b => b.Aktiv == true && (b.FKundNr.Equals(fk.FKundNr)));
            }

            return b;
        }

        /// <summary>
        /// Hämtar aktiv bokning för vald privatkund
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Bokning SökAktuellKundBokning(Kund k)
        {
            int kNr = k.KundNr;
            Bokning bokning = unitOfWork.BokningRepository.Query(q => q).FirstOrDefault(t => t.KundNr == kNr);

            return bokning;
        }
        /// <summary>
        /// Hämtar aktiv bokning för vald företagskund
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public Bokning SökAktuellFKundBokning(FöretagsKund k)
        {
            int kNr = k.FKundNr;
            Bokning bokning = unitOfWork.BokningRepository.Query(q => q).FirstOrDefault(t => t.FKundNr.Equals(kNr));

            return bokning;
        }
        /// <summary>
        /// Skapar fakturanummer
        /// </summary>
        /// <returns></returns>
        public static long RandomInt()
        {
            var random = new Random();
            long fNr = random.Next(100000000, 999999999);
            return fNr;
        }

        /// <summary>
        /// Hämtar faktura för en specifik bokning
        /// </summary>
        /// <param name="bokning"></param>
        /// <returns></returns>
        public Faktura HämtaFaktura(Bokning bokning)
        {
            Faktura f = unitOfWork.FakturaRepository.Query(q => q).FirstOrDefault(r => r.BokningsNr.Trim().Equals(bokning.BokningsNr));

            return f;
        }
        /// <summary>
        /// Hämtar aktuell bokning för utrustning
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public UtrustningBokning HämtaUtrustningBokning(Utrustning u)
        {
            UtrustningBokning ub = unitOfWork.UtrustningBokningRepository.FirstOrDefault(q => q.UtrustningId.Contains(u.Benämning) && (q.HyrdatumStart < DateTime.Now && q.HyrdatumSlut > DateTime.Now));
            return ub;
        }

        /// <summary>
        /// Hämtar utrustningsbokning för vald utrustning för att se  hyrtiderna
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public UtrustningBokning HittauBokning(Utrustning b, Bokning bokning)
        {
            UtrustningBokning ub = unitOfWork.UtrustningBokningRepository.Query(q => q).FirstOrDefault(t => t.UtrustningId.Contains(b.Benämning) && t.BokningsId.Contains(bokning.BokningsNr));

            return ub;
        }
        /// <summary>
        /// hämtar all utrustningbokning för bokning
        /// </summary>
        /// <param name="bokning"></param>
        /// <returns></returns>
        public List<UtrustningBokning> KontrollerauBoking(Bokning bokning)
        {
            List<UtrustningBokning> ub = unitOfWork.UtrustningBokningRepository.Query(q => q.Where(t => t.BokningsId.Contains(bokning.BokningsNr))).ToList();

            return ub;
        }
    }
}
