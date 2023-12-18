using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class LektionKontroller
    {
        public Personal Inloggad { get; private set; }
        public List<Lektion> BokadLektion { get; set; }
        public List<LektionBokning> LektionBokningar { get; set; }
        List<Lektion> tillgängligLektion = new List<Lektion>();
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();
        UnitOfWork unitOfWork = new UnitOfWork();
        

        /// <summary>
        /// Hämtar lektionspriser
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="grupp"></param>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public int HämtaLektionPris(string typ, string grupp, DateTime fDatum, DateTime tDatum)
        {

            LektionPris lp = unitOfWork.LektionPrisRepository.FirstOrDefault(l => l.Grupp.Contains(grupp));
            if (typ.Contains("Skidskola"))
            {


                if ((int)fDatum.DayOfWeek == 1)
                {
                    return lp.MånOnsPris;
                }
                else
                {
                    return lp.TorsFrePris;
                }
            }
            else
            {
                return lp.PrivatlektionPris;
            }
        }
        /// <summary>
        /// Hämtar alla lektioner från databas
        /// </summary>
        /// <returns></returns>
        public List<Lektion> HittaLektion()
        {
            List<Lektion> lektion = new List<Lektion>();
            foreach (Lektion l in unitOfWork.LektionRepository.Query(b => b).ToList())
            {
                lektion.Add(l);
            }
            return lektion;
        }

        /// <summary>
        /// Hämtar lektion baserat på inmatad data i sökfält
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Lektion> HittaEnLektion(string input)
        {
            List<Lektion> lektion = new List<Lektion>();
            foreach (Lektion l in unitOfWork.LektionRepository.Query(q => q.Where(p => p.Benämning.Contains(input) || p.Typ.Contains(input) || p.Grupp.Contains(input))))
            {
                lektion.Add(l);
            }
            return lektion;
        }
        /// <summary>
        /// ändra data i befintlig lektion
        /// </summary>
        /// <param name="valdLektion"></param>
        /// <param name="typ"></param>
        /// <param name="grupp"></param>
        /// <param name="lärare"></param>
        /// <param name="antalp"></param>
        /// <param name="start"></param>
        /// <param name="slut"></param>
        /// <param name="tillgänglig"></param>
        public void UppdateraLektion(Lektion valdLektion, string typ, string grupp, string lärare, string antalp, string start,
                        string slut, string tid, bool tillgänglig)
        {
            int antal = int.Parse(antalp);
            TimeSpan tidD = TimeSpan.Parse(tid);


            valdLektion.Typ = typ;
            valdLektion.Grupp = grupp;
            valdLektion.AnställningsNr = lärare;
            valdLektion.AntalPlatser = antal;
            valdLektion.LektionStart = start;
            valdLektion.LektionSlut = slut;
            valdLektion.Tid = tidD;
            valdLektion.Tillgänglig = tillgänglig;

            unitOfWork.LektionRepository.Update(valdLektion);
            unitOfWork.Save();
        }
        /// <summary>
        /// Lägger till ny lektion i databas
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="grupp"></param>
        /// <param name="lärare"></param>
        /// <param name="antalp"></param>
        /// <param name="tid"></param>
        /// <param name="slut"></param>
        /// <param name="tillgänglig"></param>
        /// <returns></returns>
        public Lektion LäggTillLektion(string typ, string grupp, string lärare, string antalp, string tid,
                        string start, string slut, bool tillgänglig)
        {
            int antal = int.Parse(antalp);
            TimeSpan tidD = TimeSpan.Parse(tid);


            string bm = null;

            if (typ.Contains("Privatlektion"))
            {
                List<Lektion> privat = unitOfWork.LektionRepository.Query(q => q).Where(b => b.Typ.Contains("Privatlektion")).ToList();
                int bnm = privat.Count() + 1;
                bm = "P" + bnm.ToString();
                Lektion l = new Lektion(grupp, bm, start, slut, antal, typ, tillgänglig, Inloggad.AnställningsNr, lärare, 0, tidD);
                unitOfWork.LektionRepository.Add(l);
                unitOfWork.Save();
                return l;
            }
            else if (typ.Contains("Skidskola"))
            {
                if (grupp.Contains("Grön"))
                {
                    List<Lektion> ss = unitOfWork.LektionRepository.Query(q => q).Where(b => b.Grupp.Contains("Grön")).ToList();
                    int bnm = ss.Count() + 1;
                    bm = "G" + bnm.ToString();

                }
                else if (grupp.Contains("Blå"))
                {
                    List<Lektion> ss = unitOfWork.LektionRepository.Query(q => q).Where(b => b.Grupp.Contains("Blå")).ToList();
                    int bnm = ss.Count() + 1;
                    bm = "B" + bnm.ToString();
                }
                else if (grupp.Contains("Röd"))
                {
                    List<Lektion> ss = unitOfWork.LektionRepository.Query(q => q).Where(b => b.Grupp.Contains("Röd")).ToList();
                    int bnm = ss.Count() + 1;
                    bm = "R" + bnm.ToString();
                }
                else
                {
                    List<Lektion> ss = unitOfWork.LektionRepository.Query(q => q).Where(b => b.Grupp.Contains("Svart")).ToList();
                    int bnm = ss.Count() + 1;
                    bm = "S" + bnm.ToString();
                }
                Lektion l = new Lektion(grupp, bm, start, slut, antal, typ, tillgänglig, Inloggad.AnställningsNr, lärare, 0, tidD);
                unitOfWork.LektionRepository.Add(l);
                unitOfWork.Save();
                return l;
            }
            return null;
        }
        /// <summary>
        /// Tar bort lektion i databas
        /// </summary>
        /// <param name="lektion"></param>
        /// <returns></returns>
        public Lektion TaBortLektion(Lektion lektion)
        {
            unitOfWork.LektionRepository.Delete(lektion);
            unitOfWork.Save();

            return lektion;
        }
        /// <summary>
        /// Hämtar personal med roll lärare i databas
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Hämtar lektionsbokningen
        /// </summary>
        /// <param name="valdLektion"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public LektionBokning HittalBokning(Lektion valdLektion, Bokning b)
        {
            LektionBokning lb = unitOfWork.LektionBokningRepository.FirstOrDefault(t => t.LektionId.Contains(valdLektion.Benämning) && t.BokningsId.Contains(b.BokningsNr));
            return lb;
        }
        
        /// <summary>
        /// Hämtar lektionbokning
        /// </summary>
        /// <returns></returns>
        public List<LektionBokning> HämtaPågåendelBokning()
        {
            return LektionBokningar;
        }
        /// <summary>
        /// Hämtar lektion som håller på att bokas men som inte har bokats än
        /// </summary>
        /// <returns></returns>
        public List<Lektion> HämtaPågåendeLektion()
        {
            return BokadLektion;
        }
        /// <summary>
        /// Hämtar alla lediga lektioner från databas och skickar de till gränssnittet
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public IList<Lektion> HämtaLedigLektion(string grupp, DateTime fDatum, DateTime tDatum)
        {
            tillgängligLektion.Clear();
            CultureInfo svenska = new CultureInfo("sv-SE");
            int pers = 0;
            if (grupp == null)
            {
                foreach (Lektion l in unitOfWork.LektionRepository.Query(b => b.Where(t => t.Tillgänglig == true && t.LektionStart.Equals(fDatum.ToString("dddd", svenska).ToLower()))))
                {
                    tillgängligLektion.Add(l);

                    foreach (LektionBokning lb in unitOfWork.LektionBokningRepository.Query(q => q.Where(b => b.LektionId.Equals(l.Benämning.Trim()) && b.LektionStartDatum <= tDatum && fDatum <= b.LektionSlutDatum)))
                    {
                        pers += lb.AntalPersoner;
                    }
                    if (pers >= l.AntalPlatser)
                    {
                        tillgängligLektion.Remove(l);

                    }
                    l.AntalPlatser -= pers;
                    pers = 0;
                }
            }
            else if (grupp != null)
            {

                foreach (Lektion l in unitOfWork.LektionRepository.Query(b => b.Where(t => t.Tillgänglig == true && t.Grupp.Equals(grupp) && t.LektionStart.Equals(fDatum.ToString("dddd", svenska).ToLower()))))
                {
                    tillgängligLektion.Add(l);

                    foreach (LektionBokning lb in unitOfWork.LektionBokningRepository.Query(q => q.Where(b => b.LektionId.Equals(l.Benämning.Trim()) && b.LektionStartDatum <= tDatum && fDatum <= b.LektionSlutDatum)))
                    {
                        pers += lb.AntalPersoner;
                    }
                    if (pers >= l.AntalPlatser)
                    {
                        tillgängligLektion.Remove(l);

                    }
                    l.AntalPlatser -= pers;
                    pers = 0;


                }
            }

            return tillgängligLektion;
        }

        /// <summary>
        /// Hämtar lektionsbokning som matchar inmatad bokningsnr
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<Lektion> HämtaBokadLektion(Bokning b)
        {
            List<Lektion> bokadLektion = new List<Lektion>();
            string bNr = b.BokningsNr;
            foreach (LektionBokning lb in unitOfWork.LektionBokningRepository.Query(q => q.Where(w => w.BokningsId.Trim().Equals(bNr.Trim()))))
            {
                foreach (Lektion l in unitOfWork.LektionRepository.Query(q => q.Where(w => w.Benämning.Trim().Equals(lb.LektionId.Trim()))))
                {
                    bokadLektion.Add(l);
                }
            }
            return bokadLektion;
        }
    }
}
