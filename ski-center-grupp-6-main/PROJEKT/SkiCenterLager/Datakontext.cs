using Microsoft.EntityFrameworkCore;
using SKICENTER;

namespace SkiCenterLager
{
    public class BookingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server = sqlutb2.hb.se, 56077; Database = suht2306; User ID = suht2306; Password = klom99; MultipleActiveResultSets=true"); // "Server=(localdb)\\mssqllocaldb;Database=SkiCenter;Trusted_Connection=True;"
            base.OnConfiguring(optionsBuilder);

        }
        public DbSet<Bokning> Bokning { get; set; }
        public DbSet<Faktura> Faktura { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Kund> Kund { get; set; }
        public DbSet<FöretagsKund> FöretagsKund { get; set; }
        public DbSet<Utrustning> Utrustning { get; set; }
        public DbSet<UtrustningPris> UtrustningPris { get; set; }
        public DbSet<UtrustningBokning> UtrustningBokning { get; set; }
        public DbSet<Lektion> Lektion { get; set; }
        public DbSet<LektionPris> LektionPris { get; set; }
        public DbSet<LektionBokning> LektionBokning { get; set; }
        public DbSet<Rum> Rum { get; set; }
        public DbSet<LogiPris> LogiPris { get; set; }
        public DbSet<KonferensPris> KonferensPris { get; set; }
        public DbSet<RumBokning> RumBokning { get; set; }

        public BookingContext()
        {
            //ResetSeed(); //bocka ur efter första körning
        }
        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bokning>()
                .HasKey(b => b.BokningsNr);



            modelBuilder.Entity<Bokning>()
                 .HasOne(r => r.FKund)
                 .WithMany(bo => bo.Bokningar)
                 .HasForeignKey(f => f.FKundNr);


            modelBuilder.Entity<Faktura>()
                .HasKey(f => f.FakturaNr);


            modelBuilder.Entity<FöretagsKund>()
                .HasKey(fö => fö.FKundNr);

            modelBuilder.Entity<Kund>()
                .HasKey(k => k.KundNr);

            modelBuilder.Entity<Lektion>()
                .HasKey(l => l.Benämning);

            modelBuilder.Entity<Lektion>()
                .HasOne(l => l.Lärare)
                .WithMany(le => le.Lektion)
                .HasForeignKey(lek => lek.Benämning);

            modelBuilder.Entity<Personal>()
               .HasKey(p => p.AnställningsNr);

            modelBuilder.Entity<Rum>()
               .HasKey(r => r.RumsNr);

            modelBuilder.Entity<LogiPris>()
               .HasKey(r => r.Storlek);

            modelBuilder.Entity<KonferensPris>()
               .HasKey(r => r.Storlek);

            modelBuilder.Entity<UtrustningPris>()
               .HasKey(o => new { o.Typ, o.Artikel });

            modelBuilder.Entity<LektionPris>()
               .HasKey(o => o.Grupp);

            modelBuilder.Entity<Utrustning>()
               .HasKey(u => u.Benämning);

            modelBuilder.Entity<LektionBokning>()
            .HasKey(o => new { o.BokningsId, o.LektionId });

            modelBuilder.Entity<LektionBokning>()
                .HasOne(di => di.Bokning)
                .WithMany(d => d.LektionBokning)
                .HasForeignKey(di => di.BokningsId);
            modelBuilder.Entity<LektionBokning>()
                .HasOne(di => di.Lektion)
                .WithMany(i => i.LektionBokning)
                .HasForeignKey(di => di.LektionId);

            modelBuilder.Entity<UtrustningBokning>()
            .HasKey(o => new { o.BokningsId, o.UtrustningId });

            modelBuilder.Entity<UtrustningBokning>()
                .HasOne(di => di.Bokning)
                .WithMany(d => d.UtrustningBokning)
                .HasForeignKey(di => di.BokningsId);
            modelBuilder.Entity<UtrustningBokning>()
                .HasOne(di => di.Utrustning)
                .WithMany(i => i.UtrustningBokning)
                .HasForeignKey(di => di.UtrustningId);

            modelBuilder.Entity<RumBokning>()
            .HasKey(o => new { o.BokningsId, o.RumId });

            modelBuilder.Entity<RumBokning>()
                .HasOne(di => di.Bokning)
                .WithMany(d => d.RumBokning)
                .HasForeignKey(di => di.BokningsId);
            modelBuilder.Entity<RumBokning>()
                .HasOne(di => di.Rum)
                .WithMany(i => i.RumBokning)
                .HasForeignKey(di => di.RumId);

            base.OnModelCreating(modelBuilder);
        }

        private void ResetSeed()
        {
            /* Personal p = new Personal("1264GH", "Sommar20", 1, "Bokningspersonal", "Lisa", "Johansson", "Fredgatan 21", 19956754, "34536", 07654587);
             Personal.Add(p);*/

            /*Kund k = new Kund(12345, 12000, "Boris", "Persson", "Ragnars gata 2", 1987654321, "3456", 07654867);
            Kund.Add(k);
           /*FöretagsKund fk = new FöretagsKund(124568975, "ICA", "Hallegatan 1", "5687", 07545653);
           FöretagsKund.Add(fk);*/
            /* Rum t = new Rum(1,"Konferens", "bastu", "20", false, "MFg", 123, 3);
             Rum.Add(t);
             Rum e = new Rum(2, "Logi", "bastu", "LGH11", true, "lgrs", 123, 3);
             Rum.Add(e);
            Rum z = new Rum(3, "Logi", "bastu", "LGH1", false, "jgjr", 123, 3);
            Rum.Add(z);
            Rum y = new Rum(4, "Konferens", "bastu", "50", true, "kgkr", 123, 3);
            Rum.Add(y);
            Utrustning u = new Utrustning(123, 1, 2, "ght", 123, 10);
            Utrustning.Add(u);
            Lektion l = new Lektion(123, true, p, "Grön", "ghtd", 123, new DateTime(2023, 08, 17), new DateTime(2023, 09, 17), new DateTime(2023, 09, 17), 10);
            Lektion.Add(l);
            Bokning b = new Bokning("BO123", 3, new DateTime(2023, 09, 30), new DateTime(2023, 10, 10), new List<Rum>() { t }, p, k, new List<Lektion>() { l }, new List<Utrustning>() { u });
            Bokning.Add(b);
            Bokning bo = new Bokning("BO124", 3, new DateTime(2023, 10, 30), new DateTime(2023, 11, 10), new List<Rum>() { e }, p, k, new List<Lektion>() { l }, new List<Utrustning>() { u });
            Bokning.Add(bo);*/
            SaveChanges();

        }
    }
}
