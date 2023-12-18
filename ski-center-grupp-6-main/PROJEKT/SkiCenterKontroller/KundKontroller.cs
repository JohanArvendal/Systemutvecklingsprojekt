using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class KundKontroller
    {
        public Personal Inloggad { get; private set; }
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();

        UnitOfWork unitOfWork = new UnitOfWork();
       
        /// <summary>
        /// Metod för att ta bort privat kund (ex GDPR-skäl)
        /// </summary>
        /// <param name="k"></param>
        public void TaBortKund(Kund k)
        {
            unitOfWork.KundRepository.Delete(k);
            unitOfWork.Save();
        }
        /// <summary>
        /// Metod för att ta bort företagskund (ex GDPR-skäl)
        /// </summary>
        /// <param name="k"></param>
        public void TaBortFöretagsKund(FöretagsKund fk)
        {
            unitOfWork.FöretagsKundRepository.Delete(fk);
            unitOfWork.Save();
        }
        /// <summary>
        /// Söka fram företagskunder för marknadschef att godkänna via ärenden. 
        /// </summary>
        /// <returns></returns>
        public List<FöretagsKund> HittaKundBevakning()
        {
            List<FöretagsKund> fKunder = new List<FöretagsKund>();
            foreach (FöretagsKund f in unitOfWork.FöretagsKundRepository.Query(b => b).Where(b => b.Aktiv == false).ToList())
            {

                fKunder.Add(f);


            }

            return fKunder;
        }
        /// <summary>
        /// Metod för Marknadschef att godkänna företagskund
        /// </summary>
        /// <param name="valdKund"></param>
        /// <param name="kredgräns"></param>
        public void GodkännKund(FöretagsKund valdKund, string kredgräns)
        {
            int kg = int.Parse(kredgräns);
            valdKund.KreditGräns = kg;
            valdKund.Aktiv = true;
            unitOfWork.FöretagsKundRepository.Update(valdKund);
            unitOfWork.Save();
        }
        /// <summary>
        /// Metod för Marknadschef att ändra kreditgräns för företagskund
        /// </summary>
        /// <param name="valdKund"></param>
        /// <param name="kredgräns"></param>
        public void UppdateraFKund(string kredgräns, FöretagsKund fKund)
        {
            int kg = int.Parse(kredgräns);
            fKund.KreditGräns = kg;
            unitOfWork.FöretagsKundRepository.Update(fKund);
            unitOfWork.Save();
        }
        /// <summary>
        /// Hämtar alla kunder från databas och skickar de till gränssnittet
        /// </summary>
        /// <returns></returns>
        public List<Kund> HämtaKund()
        {


            List<Kund> registreradeKunder = new List<Kund>();
            foreach (Kund l in unitOfWork.KundRepository.Query(q => q).ToList())
            {
                registreradeKunder.Add(l);
            }
            return registreradeKunder;


        }
        /// <summary>
        /// Hämtar en specifik kund baserat på inmatad data i gränssnittet 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Kund SökKund(long input)
        {
            Kund kund = unitOfWork.KundRepository.Query(k => k).FirstOrDefault(k => k.PersonNr == input);

            if (kund == null)
            {
                return null;
            }
            else
            {
                return kund;
            }


        }
        /// <summary>
        /// Söker fram kund baserat på personnummer som användare matat in
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Kund> SökKunderPerPersonNr(long input)
        {

            foreach (Kund kund in unitOfWork.KundRepository.Query(k => k).Where(k => k.PersonNr == input))
            {
                kunder.Add(kund);
            }


            if (kunder == null)
            {

                return null;
            }
            else
            {
                return kunder;
            }
        }

        /// <summary>
        /// Söker fram kunden med inmatat bokningsnummer och skickar kunduppgifterna till gränssnittet
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Kund SökKundPerBokningsNr(string input)
        {
            Bokning bokning = unitOfWork.BokningRepository.Query(k => k).FirstOrDefault(k => k.BokningsNr == input && k.Aktiv == true);

            if (bokning == null)
            {
                return null;
            }
            else
            {
                int kNr = bokning.KundNr;
                Kund kund = unitOfWork.KundRepository.Query(k => k).FirstOrDefault(k => k.KundNr == kNr);
                if (kund == null)
                {
                    return null;
                }
                else
                {
                    return kund;
                }
            }

        }
        /// <summary>
        /// Söker fram företagskunden med inmatat personnummer och skickar kunduppgifterna till gränssnittet
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FöretagsKund SökFöretagsKundPerOrgNr(long input)
        {
            FöretagsKund fkund = unitOfWork.FöretagsKundRepository.Query(k => k).FirstOrDefault(k => k.OrganisationsId == input);

            if (fkund == null)
            {
                return null;
            }
            else
            {
                return fkund;
            }
        }
        /// <summary>
        /// Söker fram företagskund på orgnr som användare matat in
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<FöretagsKund> SökFöretagsKunderPerOrganisationsNr(long input)
        {

            foreach (FöretagsKund företagskund in unitOfWork.FöretagsKundRepository.Query(k => k).Where(k => k.OrganisationsId == input))
            {
                företagskunder.Add(företagskund);
            }


            if (företagskunder == null)
            {

                return null;
            }
            else
            {
                return företagskunder;
            }
        }
        /// <summary>
        /// Söker fram kunden med inmatat bokningsnummer och skickar kunduppgifterna till gränssnittet
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FöretagsKund SökFöretagsKundPerBokningsNr(string input)
        {
            Bokning bokning = unitOfWork.BokningRepository.Query(k => k).FirstOrDefault(k => k.BokningsNr == input && k.Aktiv == true);

            if (bokning == null)
            {
                return null;
            }
            else
            {
                int kNr = bokning.FKund.FKundNr;
                FöretagsKund fkund = unitOfWork.FöretagsKundRepository.Query(k => k).FirstOrDefault(k => k.FKundNr == kNr);
                if (fkund == null)
                {
                    return null;
                }
                else
                {
                    return fkund;
                }
            }
        }
        /// <summary>
        /// Hämtar alla företagskunder
        /// </summary>
        /// <returns></returns>
        public List<FöretagsKund> HämtaFöretagsKund()
        {
            List<FöretagsKund> registreradeFKunder = new List<FöretagsKund>();
            foreach (FöretagsKund l in unitOfWork.FöretagsKundRepository.Query(q => q).ToList())
            {
                registreradeFKunder.Add(l);
            }
            return registreradeFKunder;


        }
        /// <summary>
        /// Lägger till ny företagskund i kundregister
        /// </summary>
        /// <param name="gata"></param>
        /// <param name="mejl"></param>
        /// <param name="namn"></param>
        /// <param name="ort"></param>
        /// <param name="postnr"></param>
        /// <param name="telenr"></param>
        /// <param name="kredgräns"></param>
        /// <param name="persnr"></param>
        /// <returns></returns>
        public FöretagsKund LäggTillFKund(string gata, string mejl, string namn, string ort, int postnr, long telenr, int kredgräns, long persnr)
        {
            int fk = unitOfWork.FöretagsKundRepository.Query(q => q).Max(k => k.FKundNr);

            int fkNr = fk += 1;
            bool aktiv = false;

            FöretagsKund fkund = new FöretagsKund(aktiv, fkNr, persnr, namn, gata, postnr, telenr, kredgräns, ort, mejl, Inloggad);
            unitOfWork.FöretagsKundRepository.Add(fkund);
            unitOfWork.Save();

            return fkund;

        }

        /// <summary>
        /// lägger till ny kund i kundregister
        /// </summary>
        /// <param name="gata"></param>
        /// <param name="mejl"></param>
        /// <param name="fnamn"></param>
        /// <param name="enamn"></param>
        /// <param name="ort"></param>
        /// <param name="postnr"></param>
        /// <param name="telenr"></param>
        /// <param name="kredgräns"></param>
        /// <param name="persnr"></param>
        /// <returns></returns>
        public Kund LäggTillKund(string gata, string mejl, string fnamn, string enamn, string ort, int postnr, long telenr, int kredgräns, long persnr)
        {
            int k = unitOfWork.KundRepository.Query(q => q).Max(k => k.KundNr);

            int kNr = k += 1;

            Kund kund = new Kund(kNr, kredgräns, fnamn, enamn, gata, persnr, postnr, telenr, ort, mejl);
            unitOfWork.KundRepository.Add(kund);
            unitOfWork.Save();

            return kund;

        }
    }
}
