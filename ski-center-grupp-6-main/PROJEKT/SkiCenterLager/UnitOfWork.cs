using SKICENTER;

namespace SkiCenterLager
{
    public class UnitOfWork
    {
        private BookingContext context;
        #region Repositories
        public Repository<Bokning> BokningRepository
        {
            get; private set;
        }
        public Repository<LektionBokning> LektionBokningRepository
        {
            get; private set;
        }
        public Repository<UtrustningBokning> UtrustningBokningRepository
        {
            get; private set;
        }
        public Repository<RumBokning> RumBokningRepository
        {
            get; private set;
        }
        public Repository<LogiPris> LogiPrisRepository
        {
            get; private set;
        }
        public Repository<KonferensPris> KonferensPrisRepository
        {
            get; private set;
        }
        public Repository<UtrustningPris> UtrustningPrisRepository
        {
            get; private set;
        }
        public Repository<LektionPris> LektionPrisRepository
        {
            get; private set;
        }
        public Repository<Personal> PersonalRepository
        {
            get; private set;
        }
        public Repository<Kund> KundRepository
        {
            get; private set;
        }
        public Repository<FöretagsKund> FöretagsKundRepository
        {
            get; private set;
        }
        public Repository<Faktura> FakturaRepository
        {
            get; private set;
        }

        public Repository<Rum> RumRepository
        {
            get; private set;
        }
        public Repository<Utrustning> UtrustningRepository
        {
            get; private set;
        }
        public Repository<Lektion> LektionRepository
        {
            get; private set;
        }
        #endregion Repositories

        /// <summary>
        ///  Skapar ny instans
        /// </summary>
        public UnitOfWork()
        {
            context = new BookingContext();
            BokningRepository = new Repository<Bokning>(context);
            PersonalRepository = new Repository<Personal>(context);
            KundRepository = new Repository<Kund>(context);
            FöretagsKundRepository = new Repository<FöretagsKund>(context);
            RumRepository = new Repository<Rum>(context);
            LektionRepository = new Repository<Lektion>(context);
            UtrustningRepository = new Repository<Utrustning>(context);
            FakturaRepository = new Repository<Faktura>(context);
            LektionBokningRepository = new Repository<LektionBokning>(context);
            UtrustningBokningRepository = new Repository<UtrustningBokning>(context);
            RumBokningRepository = new Repository<RumBokning>(context);
            LogiPrisRepository = new Repository<LogiPris>(context);
            KonferensPrisRepository = new Repository<KonferensPris>(context);
            UtrustningPrisRepository = new Repository<UtrustningPris>(context);
            LektionPrisRepository = new Repository<LektionPris>(context);
        }

        /// <summary>
        ///  Sparar ändringar
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
