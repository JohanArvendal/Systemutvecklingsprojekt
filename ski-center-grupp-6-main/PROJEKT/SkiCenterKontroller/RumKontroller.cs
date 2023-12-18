using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class RumKontroller
    {
        public Personal Inloggad { get; private set; }
        List<Rum> tillgängligaRum = new List<Rum>();
        public List<Rum> BokatRum { get; set; }
        public List<RumBokning> RumBokningar { get; set; }
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();

        UnitOfWork unitOfWork = new UnitOfWork();
       

        // <summary>
        /// Hämtar vecka för givet datum för att ta fram pris
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        public static int HämtaVecka(DateTime datum)
        {

            DayOfWeek dag = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(datum);
            if (dag >= DayOfWeek.Monday && dag <= DayOfWeek.Wednesday)
            {
                datum = datum.AddDays(3);
            }

            int vecka = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return vecka;


        }
        /// <summary>
        /// hämtar pris för rum
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int HämtaLogiPris(int vecka, int längd, string storlek, DateTime fDatum, DateTime tDatum)
        {

            LogiPris lp = unitOfWork.LogiPrisRepository.FirstOrDefault(t => t.Storlek.Contains(storlek));
            int pris = 0;
            int prisvecka = lp.HämtaLogiPris(vecka);


            if (prisvecka == 1)
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris1;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris1;
                    int prisFreSön = lp.FreSönDygnPris1;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 1, fDatum, tDatum);
                }


            }
            else if (prisvecka == 2)
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris2;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris2;
                    int prisFreSön = lp.FreSönDygnPris2;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 2, fDatum, tDatum);
                }
            }
            else if (prisvecka == 3)
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris3;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris3;
                    int prisFreSön = lp.FreSönDygnPris3;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 3, fDatum, tDatum);
                }
            }
            else if (prisvecka == 4)
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris4;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris4;
                    int prisFreSön = lp.FreSönDygnPris4;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 4, fDatum, tDatum);
                }
            }
            else if (prisvecka == 5)
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris5;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris5;
                    int prisFreSön = lp.FreSönDygnPris5;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 5, fDatum, tDatum);
                }
            }
            else
            {
                if (längd == 7)
                {
                    pris = lp.Veckopris6;
                }
                else if (längd < 7)
                {
                    int prisSönFre = lp.SönFreDygnPris6;
                    int prisFreSön = lp.FreSönDygnPris6;
                    pris = HämtaDygnPris(prisSönFre, prisFreSön, 6, fDatum, tDatum);
                }
            }

            return pris;
        }
        /// <summary>
        /// Hämtar pris för konferensrum och camp
        /// </summary>
        /// <param name="vecka"></param>
        /// <param name="längd"></param>
        /// <param name="storlek"></param>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public int HämtaKonferensPris(int vecka, int längd, string storlek, DateTime fDatum, DateTime tDatum)
        {
            KonferensPris kp = unitOfWork.KonferensPrisRepository.FirstOrDefault(t => t.Storlek.Contains(storlek));
            int pris = 0;
            int prisvecka = kp.HämtaKonferensCampPris(vecka);


            if (prisvecka == 1)
            {
                if (längd == 7)
                {
                    pris = kp.Veckopris1;
                }
                else if (längd > 1 && längd < 7)
                {
                    int prisDygn1 = kp.Dygnpris1;
                    int prisDygn2 = kp.Dygnpris1;
                    pris = HämtaDygnPris(prisDygn1, prisDygn2, 1, fDatum, tDatum);
                }
                else
                {
                    int timpris = kp.Timpris1;
                    pris = HämtaTimPris(timpris, 1, fDatum, tDatum);
                }

            }
            else if (prisvecka == 2)
            {
                if (längd == 7)
                {
                    pris = kp.Veckopris2;
                }
                else if (längd > 1 && längd < 7)
                {
                    int prisDygn1 = kp.Dygnpris2;
                    int prisDygn2 = kp.Dygnpris2;
                    pris = HämtaDygnPris(prisDygn1, prisDygn2, 2, fDatum, tDatum);
                }
                else
                {
                    int timpris = kp.Timpris2;
                    pris = HämtaTimPris(timpris, 1, fDatum, tDatum);
                }
            }
            else if (prisvecka == 3)
            {
                if (längd == 7)
                {
                    pris = kp.Veckopris3;
                }
                else if (längd >= 1 && längd < 7)
                {
                    int prisDygn1 = kp.Dygnpris3;
                    int prisDygn2 = kp.Dygnpris3;
                    pris = HämtaDygnPris(prisDygn1, prisDygn2, 3, fDatum, tDatum);
                }
                else
                {
                    int timpris = kp.Timpris3;
                    pris = HämtaTimPris(timpris, 1, fDatum, tDatum);
                }
            }


            return pris;
        }
        /// <summary>
        /// Hämtar totalpris för antal valda dygn
        /// </summary>
        /// <param name="prisSönFre"></param>
        /// <param name="prisFreSön"></param>
        /// <param name="vecka"></param>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public int HämtaDygnPris(int prisSönFre, int prisFreSön, int vecka, DateTime fDatum, DateTime tDatum)
        {
            var datum = new List<DateTime>();
            List<int> dagar = new List<int>();
            int pris = 0;
            //datum.Add(fDatum);
            for (var dt = fDatum; dt <= tDatum; dt = dt.AddDays(1))
            {
                datum.Add(dt);
            }
            foreach (DateTime dt in datum)
            {
                int d = (int)dt.DayOfWeek;
                dagar.Add(d);
            }
            if (dagar.Count() == 0)
            {
                int d = (int)fDatum.DayOfWeek;
                dagar.Add(d);
            }
            while (dagar.Any())
            {
                if ((dagar.Contains(0)))
                {
                    if (dagar.Contains(0) && dagar.Contains(1))
                    {
                        pris += prisSönFre;
                    }
                    else
                    {
                        pris += prisFreSön;
                    }
                    dagar.Remove(0);
                }
                else if ((dagar.Contains(1)))
                {
                    pris += prisSönFre;
                    dagar.Remove(1);
                }
                else if ((dagar.Contains(2)))
                {
                    pris += prisSönFre;
                    dagar.Remove(2);
                }
                else if ((dagar.Contains(3)))
                {
                    pris += prisSönFre;
                    dagar.Remove(3);
                }
                else if ((dagar.Contains(5)))
                {
                    if (dagar.Contains(5) && dagar.Contains(4))
                    {
                        pris += prisSönFre;
                    }
                    else
                    {
                        pris += prisFreSön;
                    }
                    dagar.Remove(5);
                }
                else if ((dagar.Contains(4)))
                {
                    pris += prisSönFre;
                    dagar.Remove(4);
                }

                else if ((dagar.Contains(6)))
                {
                    pris += prisFreSön;
                    dagar.Remove(6);
                }
            }
            return pris;
        }
        /// <summary>
        /// Hämtar konferensrummens timpris
        /// </summary>
        /// <returns></returns>
        public int HämtaTimPris(int timpris, int vecka, DateTime fDatum, DateTime tDatum)
        {
            var timmar = (fDatum - tDatum).TotalHours;
            int pris = (int)timmar * timpris;
            return pris - (2 * pris);
        }
        /// <summary>
        /// Hämtar alla rum i databasen
        /// </summary>
        /// <returns></returns>
        public List<Rum> HittaRum()
        {
            List<Rum> rum = new List<Rum>();
            foreach (Rum r in unitOfWork.RumRepository.Query(b => b).ToList())
            {
                rum.Add(r);
            }
            return rum;
        }
        /// <summary>
        /// Hämtar rum baserat på användarens inmation i sökfält
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Rum> HittaEttRum(string input)
        {
            List<Rum> ettRum = new List<Rum>();
            foreach (Rum r in unitOfWork.RumRepository.Query(q => q.Where(p => p.Typ.Contains(input.ToString()))))
            {
                ettRum.Add(r);
            }
            if (!ettRum.Any())
            {
                foreach (Rum r in unitOfWork.RumRepository.Query(q => q.Where(p => p.RumsNr.Equals(int.Parse(input)))))
                {
                    ettRum.Add(r);
                }
            }
            return ettRum;
        }
        /// <summary>
        /// Uppdaterar rum i databas med inmatad data
        /// </summary>
        /// <param name="valtRum"></param>
        /// <param name="typ"></param>
        /// <param name="boknr"></param>
        /// <param name="beskrivning"></param>
        /// <param name="prestanda"></param>
        /// <param name="tillgänglig"></param>
        /// <param name="storlek"></param>
        public void UppdateraRum(Rum valtRum, string typ, string boknr, string beskrivning, string prestanda, bool tillgänglig, string storlek)
        {

            valtRum.Typ = typ;
            valtRum.Beskrivning = beskrivning;
            valtRum.Prestanda = prestanda;
            valtRum.RumsStorlek = storlek;


            unitOfWork.RumRepository.Update(valtRum);
            unitOfWork.Save();
        }
        /// <summary>
        /// Lägger till nytt rum i databas
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="boknr"></param>
        /// <param name="beskrivning"></param>
        /// <param name="prestanda"></param>
        /// <param name="tillgänglig"></param>
        /// <param name="storlek"></param>
        /// <returns></returns>
        public Rum LäggTillRum(string typ, string boknr, string beskrivning, string prestanda, bool tillgänglig, string storlek)
        {


            int ru = unitOfWork.RumRepository.Query(q => q).Max(k => k.RumsNr);

            int rNr = ru += 1;
            string bm = null;

            if (typ.Contains("Camp"))
            {
                List<Rum> camp = unitOfWork.RumRepository.Query(q => q).Where(b => b.RumsStorlek.Contains("Camp")).ToList();
                int bnm = camp.Count() + 1;
                bm = "Camp" + bnm.ToString();
            }
            else if (typ.Contains("Logi"))
            {

                if (storlek.ToUpper().Contains("LGH2"))
                {
                    List<Rum> logi2 = unitOfWork.RumRepository.Query(q => q).Where(b => b.RumsStorlek.Contains("LGH2")).ToList();
                    int bnm = logi2.Count() + 1;
                    bm = "Lll" + bnm.ToString();
                }
                else
                {
                    List<Rum> logi = unitOfWork.RumRepository.Query(q => q).Where(b => b.RumsStorlek.Contains("LGH1")).ToList();
                    int bnm = logi.Count() + 1;
                    bm = "Ll" + bnm.ToString();
                }

            }
            else
            {
                if (storlek.Contains("20"))
                {
                    List<Rum> konfl = unitOfWork.RumRepository.Query(q => q).Where(b => b.Benämning.Contains("KLL")).ToList();
                    int bnm = konfl.Count() + 1;
                    bm = "KLL" + bnm.ToString();
                }
                else
                {
                    List<Rum> konfs = unitOfWork.RumRepository.Query(q => q).Where(b => b.Benämning.Contains("KLS")).ToList();
                    int bnm = konfs.Count() + 1;
                    bm = "KLS" + bnm.ToString();
                }
            }

            Rum r = new Rum(rNr, beskrivning, storlek, tillgänglig, bm, typ, Inloggad.AnställningsNr, prestanda, 0);
            unitOfWork.RumRepository.Add(r);
            unitOfWork.Save();
            return r;
        }
        /// <summary>
        /// Tar bort rum från databas
        /// </summary>
        /// <param name="rum"></param>
        /// <returns></returns>
        public Rum TaBortRum(Rum rum)
        {
            unitOfWork.RumRepository.Delete(rum);
            unitOfWork.Save();

            return rum;
        }
        /// <summary>
        /// Hämtar aktuell bokning för rummet
        /// </summary>
        /// <param name="r"></param>
        public RumBokning HämtaRumBokning(Rum r)
        {
            RumBokning rb = unitOfWork.RumBokningRepository.FirstOrDefault(q => q.RumId.Equals(r.RumsNr) && (q.BokningsDatumStart < DateTime.Now && q.BokningsDatumSlut > DateTime.Now));
            return rb;
        }
        /// <summary>
        /// hämtar all rumsbokning för bokning
        /// </summary>
        /// <param name="bokning"></param>
        /// <returns></returns>
        public List<RumBokning> KontrollerarBoking(Bokning bokning)
        {
            List<RumBokning> rb = unitOfWork.RumBokningRepository.Query(q => q.Where(t => t.BokningsId.Contains(bokning.BokningsNr))).ToList();

            return rb;
        }
        /// <summary>
        /// Startar rumbokningen när kund hämtar ut nyckeln
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public void StartaRumBokning(RumBokning rb)
        {
            rb.Startad = true;
            unitOfWork.RumBokningRepository.Update(rb);
            unitOfWork.Save();
        }
        
        /// <summary>
        /// Avslutar bokningen för specifikt rum
        /// </summary>
        /// <param name="valtRum"></param>
        /// <param name="bokning"></param>
        /// <param name="rb"></param>
        public void AvslutaRumBokning(Rum valtRum, Bokning bokning, RumBokning rb)
        {
            rb.Startad = false;
            rb.Avslutad = true;
            rb.BokningsDatumSlut = DateTime.Now;
            unitOfWork.RumRepository.Update(valtRum);
            unitOfWork.RumBokningRepository.Update(rb);
            unitOfWork.Save();
        }
      
        /// <summary>
        /// Hämtar alla lediga rum från databas och skickar de till gränssnittet
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public IList<Rum> HittaLedigaRum(DateTime fDatum, DateTime tDatum)
        {

            tillgängligaRum.Clear();
            foreach (Rum l in unitOfWork.RumRepository.Query(b => b).Where(b => b.Tillgänglig == true).ToList())
            {
                tillgängligaRum.Add(l);
            }
            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(t => t.BokningsDatumStart < tDatum && fDatum < t.BokningsDatumSlut)))
            {
                tillgängligaRum.Remove(rb.Rum);
            }


            return tillgängligaRum;
        }
        /// <summary>
        /// Hämtar alla lediga logi från databas och skickar de till gränssnittet 
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public IList<Rum> HittaLedigLogi(DateTime fDatum, DateTime tDatum, string storlek)
        {
            tillgängligaRum.Clear();
            foreach (Rum l in unitOfWork.RumRepository.Query(b => b).Where(b => b.RumsStorlek.Contains(storlek) && b.Tillgänglig == true).ToList())
            {
                tillgängligaRum.Add(l);
            }
            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(t => t.Rum.RumsStorlek.Contains(storlek) && (t.BokningsDatumStart < tDatum && fDatum < t.BokningsDatumSlut))))
            {
                tillgängligaRum.Remove(rb.Rum);
            }


            return tillgängligaRum;


        }
        /// <summary>
        /// Hämtar alla lediga konferensrum från databas och skickar de till gränssnittet
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public IList<Rum> HittaLedigKonferens(DateTime fDatum, DateTime tDatum, string storlek)
        {
            tillgängligaRum.Clear();
            foreach (Rum l in unitOfWork.RumRepository.Query(b => b).Where(b => b.RumsStorlek.Contains(storlek) && b.Tillgänglig == true).ToList())
            {
                tillgängligaRum.Add(l);
            }
            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(t => t.Rum.RumsStorlek.Contains(storlek) && (t.BokningsDatumStart < tDatum && fDatum < t.BokningsDatumSlut))))
            {
                tillgängligaRum.Remove(rb.Rum);
            }


            return tillgängligaRum;
        }
        /// <summary>
        /// KOntrollerar om kund har logibokning redan för att lägga till tjänst
        /// </summary>
        /// <param name="rNr"></param>
        /// <returns></returns>
        public Bokning KontrolleraRumsbokning(Kund hämtadKund, FöretagsKund hämtadfKund, DateTime fDatum, DateTime tDatum)
        {



            if (hämtadKund != null)
            {
                Bokning b = unitOfWork.BokningRepository.FirstOrDefault(b => b.KundNr.Equals(hämtadKund.KundNr) && b.Aktiv == true);
                if (b == null || (b.FrånDatum <= tDatum && b.TillDatum >= fDatum))
                {
                    return b;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                {


                    Bokning b = unitOfWork.BokningRepository.Query(q => q).FirstOrDefault(b => b.FKundNr.Equals(hämtadfKund.FKundNr) && b.Aktiv == true);
                    if (b == null || b.FrånDatum > fDatum || tDatum > b.TillDatum.AddDays(3))
                    {
                        return null;
                    }
                    else
                    {
                        return b;
                    }
                }

            }



        }
        /// <summary>
        /// Hämtar rumsbokning som matchar inmatad bokningsnr
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<Rum> HämtaBokadeRum(Bokning b)
        {
            List<Rum> bokadeRum = new List<Rum>();
            string bNr = b.BokningsNr;
            foreach (RumBokning rb in unitOfWork.RumBokningRepository.Query(q => q.Where(w => w.BokningsId.Trim().Equals(bNr.Trim()))))
            {
                foreach (Rum r in unitOfWork.RumRepository.Query(q => q.Where(p => p.RumsNr.Equals(rb.RumId))))
                {
                    bokadeRum.Add(r);
                }
            }
            return bokadeRum;
        }
    }
}
