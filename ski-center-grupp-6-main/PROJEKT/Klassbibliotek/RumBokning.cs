using System;
using System.ComponentModel.DataAnnotations;

namespace SKICENTER
{
    public class RumBokning
    {
        [Required]
        public int RumId { get; set; }
        public virtual Rum Rum { get; set; }
        public DateTime BokningsDatumStart { get; set; }
        public DateTime BokningsDatumSlut { get; set; }
        public bool Startad { get; set; }
        public bool Avslutad { get; set; }
        public int RumPris { get; set; }
        [Required]
        public string BokningsId { get; set; }
        public virtual Bokning Bokning { get; set; }

        public RumBokning(int RumId, string BokningsId, Rum Rum, DateTime BokningsDatumStart, DateTime BokningsDatumSlut, int RumPris)
        {
            this.RumId = RumId;
            this.BokningsId = BokningsId;
            this.Rum = Rum;
            this.BokningsDatumStart = BokningsDatumStart;
            this.BokningsDatumSlut = BokningsDatumSlut;
            this.RumPris = RumPris;
        }

        public RumBokning() { }
    }
}
