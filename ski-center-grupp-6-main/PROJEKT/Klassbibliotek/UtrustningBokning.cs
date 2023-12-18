using System;
using System.ComponentModel.DataAnnotations;

namespace SKICENTER
{
    public class UtrustningBokning
    {
        [Required]
        public string UtrustningId { get; set; }
        public virtual Utrustning Utrustning { get; set; }
        public DateTime HyrdatumStart { get; set; }
        public DateTime HyrdatumSlut { get; set; }
        public bool Utlämnad { get; set; }
        public bool Återlämnad { get; set; }
        public int UtrustningPris { get; set; }
        [Required]
        public string BokningsId { get; set; }
        public virtual Bokning Bokning { get; set; }

        public UtrustningBokning(string UtrustningId, string BokningsId, Utrustning Utrustning, DateTime HyrdatumStart, DateTime HyrdatumSlut, int UtrustningPris)
        {
            this.UtrustningId = UtrustningId;
            this.BokningsId = BokningsId;
            this.Utrustning = Utrustning;
            this.HyrdatumStart = HyrdatumStart;
            this.HyrdatumSlut = HyrdatumSlut;
            this.UtrustningPris = UtrustningPris;
        }

        public UtrustningBokning() { }
    }
}
