using SKICENTER;
using SkiCenterLager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiCenterKontroller
{
    public class PersonalKontroller
    {
        public Personal Inloggad { get; private set; }
        public List<Kund> kunder = new List<Kund>();
        public List<FöretagsKund> företagskunder = new List<FöretagsKund>();
        List<Personal> allPersonal = new List<Personal>();

        UnitOfWork unitOfWork = new UnitOfWork();

        public Personal SparaPersonal()
        {
            return Inloggad;
        }
        /// <summary>
        /// Hämtar all personal (sysadmin)
        /// </summary>
        /// <returns></returns>
        public List<Personal> HittaPersonal()
        {
            allPersonal.Clear();
            foreach (Personal p in unitOfWork.PersonalRepository.Query(q => q).ToList())
            {
                allPersonal.Add(p);
            }
            return allPersonal;
        }
        /// <summary>
        /// Hämtar personal baserat på inmatad personalnamn eller anställningsnr
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<Personal> HittaEnPersonal(string input)
        {
            List<Personal> enPersonal = new List<Personal>();
            foreach (Personal p in unitOfWork.PersonalRepository.Query(q => q.Where(p => p.FörNamn.Contains(input) || p.EfterNamn.Contains(input) || p.AnställningsNr.Contains(input))))
            {
                enPersonal.Add(p);
            }
            return enPersonal;
        }
        /// <summary>
        /// Uppdaterar personal med nytillagda uppgifter (sysadmin)
        /// </summary>
        /// <param name="personal"></param>
        /// <param name="behörighet"></param>
        /// <param name="eNamn"></param>
        /// <param name="fNamn"></param>
        /// <param name="gata"></param>
        /// <param name="lösen"></param>
        /// <param name="persnr"></param>
        /// <param name="mejl"></param>
        /// <param name="ort"></param>
        /// <param name="postnr"></param>
        /// <param name="roll"></param>
        /// <param name="telenr"></param>
        public void UppdateraPersonal(Personal personal, string behörighet, string eNamn, string fNamn, string gata, string lösen, string persnr, string mejl, string ort, string postnr, string roll, string telenr)
        {
            int intbehörighet = int.Parse(behörighet);
            long intpersnr = long.Parse(persnr);
            int intpostnr = int.Parse(postnr);
            long inttelenr = long.Parse(telenr);

            personal.Behörighet = intbehörighet;
            personal.EfterNamn = eNamn;
            personal.FörNamn = fNamn;
            personal.Adress = gata;
            personal.Lösenord = lösen;
            personal.PersonNr = intpersnr;
            personal.Mail = mejl;
            personal.Ort = ort;
            personal.PostNr = intpostnr;
            personal.Roll = roll;
            personal.TelefonNr = inttelenr;

            unitOfWork.PersonalRepository.Update(personal);
            unitOfWork.Save();
        }
        /// <summary>
        /// Lägger till ny personal i databas (sysadmin)
        /// </summary>
        /// <param name="behörighet"></param>
        /// <param name="eNamn"></param>
        /// <param name="fNamn"></param>
        /// <param name="gata"></param>
        /// <param name="lösen"></param>
        /// <param name="persnr"></param>
        /// <param name="mejl"></param>
        /// <param name="ort"></param>
        /// <param name="postnr"></param>
        /// <param name="roll"></param>
        /// <param name="telenr"></param>
        /// <returns></returns>
        public Personal LäggTillPersonal(string behörighet, string eNamn, string fNamn, string gata, string lösen, string persnr, string mejl, string ort, string postnr, string roll, string telenr)
        {
            int intbehörighet = int.Parse(behörighet);
            long intpersnr = long.Parse(persnr);
            int intpostnr = int.Parse(postnr);
            long inttelenr = long.Parse(telenr);

            string aNr = Bokning.RandomString(3);

            Personal p = new Personal(aNr, lösen, intbehörighet, roll, fNamn, eNamn, gata, intpersnr, intpostnr, inttelenr, ort, mejl, Inloggad.AnställningsNr);
            unitOfWork.PersonalRepository.Add(p);
            unitOfWork.Save();

            return p;
        }
        /// <summary>
        /// Ta bort personal från databas (sysadmin)
        /// </summary>
        /// <param name="personal"></param>
        /// <returns></returns>
        public Personal TaBortPersonal(Personal personal)
        {
            unitOfWork.PersonalRepository.Delete(personal);
            unitOfWork.Save();

            return personal;
        }
        /// <summary>
        /// Hämtar personal med roll lärare i databas
        /// </summary>
        /// <returns></returns>
        public List<Personal> HämtaLärare()
        {
            List<Personal> personal = unitOfWork.PersonalRepository.Query(q => q.Where(l => l.Roll.Contains("Lärare"))).ToList();

            return personal;
        }
        /// <summary>
        /// Metod för inloggning som kollar om anställningsnr finns i databas
        /// </summary>
        /// <param name="anställningsNr"></param>
        /// <param name="lösen"></param>
        /// <returns></returns>
        public bool LoggaIn(string anställningsNr, string lösen)
        {
            Personal personal = unitOfWork.PersonalRepository.Query(q => q).FirstOrDefault(e => e.AnställningsNr.Equals(anställningsNr));

            if (personal != null && VerifieraLösen(lösen))
            {
                Inloggad = personal;
                return true;
            }
            else
            {
                Inloggad = null;
                return false;
            }

        }
        /// <summary>
        /// metod för att hämta behörigheten som den inloggade användaren har
        /// </summary>
        /// <param name="behörighet"></param>
        /// <returns></returns>
        public int HämtaBehörighet()
        {
            int b = 0;
            try
            {


                Personal behörig = unitOfWork.PersonalRepository.Query(q => q).FirstOrDefault(e => e.AnställningsNr.Equals(Inloggad.AnställningsNr));
                if (behörig != null)
                {

                    b = behörig.Behörighet;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HämtaBehörighet: {ex.Message}");
            }
            return b; //just nu returnerar 0/null, oklart vad som är problemet

        }


        /// <summary>
        /// tar emot lösen från loggain-metod och kontrollerar om inmatat lösen för personalen är korrekt
        /// </summary>
        /// <param name="lösen"></param>
        /// <returns></returns>
        public bool VerifieraLösen(string lösen)
        {

            Personal lösenOrd = unitOfWork.PersonalRepository.Query(q => q).FirstOrDefault(e => e.Lösenord.Equals(lösen));

            if (lösenOrd == null)
            {
                return false;
            }
            if (lösenOrd.Lösenord.Equals(lösen))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
