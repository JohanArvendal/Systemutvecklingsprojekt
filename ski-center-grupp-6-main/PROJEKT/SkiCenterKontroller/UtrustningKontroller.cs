using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class UtrustningKontroller
    {

        public Personal Inloggad { get; private set; }
        public List<Utrustning> BokadUtrustning { get; set; }
        public List<UtrustningBokning> UtrustningBokningar { get; set; }
        List<Utrustning> tillgängligUtrustning = new List<Utrustning>();
        private static Random random = new Random();
        List<Lektion> tillgängligLektion = new List<Lektion>();
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();

        UnitOfWork unitOfWork = new UnitOfWork();
        PersonalKontroller pk = new PersonalKontroller();

     

        /// <summary>
        /// Hämtar utrustningspris
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public int HämtaUtrustningPris(string typ, string artikel, DateTime fDatum, DateTime tDatum)
        {
            UtrustningPris up = unitOfWork.UtrustningPrisRepository.FirstOrDefault(u => u.Typ.Contains(typ) && u.Artikel.Contains(artikel));

            int dagar = (fDatum.Date - tDatum.Date).Days;
            if (dagar == 0 || dagar == -1)
            {
                return up.Pris1;
            }
            else if (dagar == -2)
            {
                return up.Pris2;
            }
            else if (dagar == -3)
            {
                return up.Pris3;
            }
            else if (dagar == -4)
            {
                return up.Pris4;
            }
            else
            {
                return up.Pris5;
            }

        }
        /// <summary>
        /// Hämtar all utrustning oavsett bokad eller ej (för sysadmin)
        /// </summary>
        /// <returns></returns>
        public IList<Utrustning> HittaUtrustning()
        {
            List<Utrustning> utrustning = new List<Utrustning>();
            foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(b => b).ToList())
            {
                utrustning.Add(u);
            }
            return utrustning;

        }
        /// <summary>
        /// Hämtar utrustning baserat på sökkriterier (sysadmin)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Utrustning> HittaEnUtrustning(string input)
        {
            List<Utrustning> enUtrustning = new List<Utrustning>();
            foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(q => q.Where(p => p.Benämning.Contains(input) || p.UtrustningsArtikel.Contains(input))))
            {
                enUtrustning.Add(u);
            }
            return enUtrustning;
        }
        /// <summary>
        /// Uppdaterar utrustningsuppgifter som ex pris (sysadmin)
        /// </summary>
        /// <param name="valdUtrustning"></param>
        /// <param name="typ"></param>
        /// <param name="artikel"></param>
        /// <param name="boknr"></param>
        /// <param name="pris1"></param>
        /// <param name="pris2"></param>
        /// <param name="pris3"></param>
        /// <param name="pris4"></param>
        /// <param name="pris5"></param>
        /// <param name="tillgänglig"></param>
        /// <param name="storlek"></param>
        public void UppdateraUtrustning(Utrustning valdUtrustning, string typ, string artikel, string boknr, bool tillgänglig, string storlek)
        {
            int intstorlek = int.Parse(storlek);


            valdUtrustning.Storlek = intstorlek;
            valdUtrustning.Typ = typ;
            valdUtrustning.UtrustningsArtikel = artikel;
            valdUtrustning.Tillgänglig = tillgänglig;

            unitOfWork.UtrustningRepository.Update(valdUtrustning);
            unitOfWork.Save();
        }
        /// <summary>
        /// Lägger till ny utrustning i databas (sysadmin)
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="artikel"></param>
        /// <param name="boknr"></param>
        /// <param name="pris1"></param>
        /// <param name="pris2"></param>
        /// <param name="pris3"></param>
        /// <param name="pris4"></param>
        /// <param name="pris5"></param>
        /// <param name="tillgänglig"></param>
        /// <param name="storlek"></param>
        /// <returns></returns>
        public Utrustning LäggTillUtrustning(string typ, string artikel, string boknr, bool tillgänglig, string storlek)
        {
            int intstorlek = int.Parse(storlek);

            string benämning = null;
            benämning = Bokning.RandomString(3);
            Utrustning u = new Utrustning(artikel, intstorlek, benämning, 0, typ, tillgänglig, Inloggad.AnställningsNr);
            unitOfWork.UtrustningRepository.Add(u);
            unitOfWork.Save();

            return u;
        }
        /// <summary>
        /// Tar bort utrustning från databas (sysadmin)
        /// </summary>
        /// <param name="utrustning"></param>
        /// <returns></returns>
        public Utrustning TaBortUtrustning(Utrustning utrustning)
        {
            unitOfWork.UtrustningRepository.Delete(utrustning);
            unitOfWork.Save();

            return utrustning;
        }
        
        /// <summary>
        /// Markerar utrustningen som utlämnad
        /// </summary>
        /// <param name="valdUtrustning"></param>
        /// <param name="bokning"></param>
        /// <param name="ub"></param>
        public void LämnaUtUtrustning(UtrustningBokning ub)
        {
            ub.Utlämnad = true;
            unitOfWork.UtrustningBokningRepository.Update(ub);
            unitOfWork.Save();
        }
        /// <summary>
        /// Markerar utrustning som återlämnad i databas
        /// </summary>
        /// <param name="ValdUtrustning"></param>
        /// <param name="bokning"></param>
        public void ÅterlämnaUtrustning(Utrustning ValdUtrustning, Bokning bokning, UtrustningBokning ub)
        {
            ub.Utlämnad = false;
            ub.Återlämnad = true;
            ub.HyrdatumSlut = DateTime.Now;
            unitOfWork.UtrustningRepository.Update(ValdUtrustning);
            unitOfWork.UtrustningBokningRepository.Update(ub);
            unitOfWork.Save();
        }
        
        /// <summary>
        /// Hämtar utrustning som håller på att bokas men inte bokats än
        /// </summary>
        /// <returns></returns>
        public List<Utrustning> HämtaPågåendeUtrustning()
        {
            return BokadUtrustning;
        }
        /// <summary>
        /// Hämtar utrustningsbokning som håller på att bokas men inte bokats än
        /// </summary>
        /// <returns></returns>
        public List<UtrustningBokning> HämtaPågåendeuBokning()
        {
            return UtrustningBokningar;
        }
        /// <summary>
        /// Hämtar all ledig utrustning baserat på parametervärden från databas och skickar de till gränssnittet
        /// </summary>
        /// <param name="fDatum"></param>
        /// <param name="tDatum"></param>
        /// <returns></returns>
        public IList<Utrustning> HittaLedigUtrustning(DateTime fDatum, DateTime tDatum, string typ, string artikel)
        {
            tillgängligUtrustning.Clear();

            if (artikel == null && typ == null)
            {
                foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(b => b).Where(b => b.Tillgänglig == true).ToList())
                {
                    tillgängligUtrustning.Add(u);
                }
                foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(t => t.HyrdatumStart < tDatum && fDatum < t.HyrdatumSlut)))
                {
                    tillgängligUtrustning.Remove(ub.Utrustning);
                }



            }
            else if (artikel == null)
            {
                foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(b => b).Where(b => b.Typ.Contains(typ) && b.Tillgänglig == true).ToList())
                {
                    tillgängligUtrustning.Add(u);
                }
                foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(t => t.Utrustning.Typ.Contains(typ) && (t.HyrdatumStart < tDatum && fDatum < t.HyrdatumSlut))))
                {
                    tillgängligUtrustning.Remove(ub.Utrustning);
                }
            }
            else
            {
                foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(b => b).Where(b => b.Typ.Contains(typ) && b.UtrustningsArtikel.Contains(artikel) && b.Tillgänglig == true).ToList())
                {
                    tillgängligUtrustning.Add(u);
                }
                foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(t => t.Utrustning.Typ.Contains(typ) && (t.HyrdatumStart < tDatum && fDatum < t.HyrdatumSlut))))
                {
                    tillgängligUtrustning.Remove(ub.Utrustning);
                }
            }

            return tillgängligUtrustning;
        }
        /// <summary>
        /// Hämtar utrustningsbokning som matchar inmatad bokningsnr
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public List<Utrustning> HämtaBokadUtrustning(Bokning b)
        {
            List<Utrustning> bokadUtrustning = new List<Utrustning>();
            string bNr = b.BokningsNr;
            foreach (UtrustningBokning ub in unitOfWork.UtrustningBokningRepository.Query(q => q.Where(w => w.BokningsId.Trim().Equals(bNr.Trim()))))
            {
                foreach (Utrustning u in unitOfWork.UtrustningRepository.Query(q => q.Where(w => w.Benämning.Contains(ub.UtrustningId))))
                    bokadUtrustning.Add(u);
            }
            return bokadUtrustning;
        }
       
    }
}
