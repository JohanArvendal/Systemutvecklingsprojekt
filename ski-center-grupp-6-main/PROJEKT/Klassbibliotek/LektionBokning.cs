using System;
using System.ComponentModel.DataAnnotations;

namespace SKICENTER

{
    public class LektionBokning
    {
        [Required]
        public string LektionId { get; set; }
        public virtual Lektion Lektion { get; set; }
        public int AntalPersoner { get; set; }
        public DateTime LektionStartDatum { get; set; }
        public DateTime LektionSlutDatum { get; set; }
        public int LektionPris { get; set; }

        [Required]
        public string BokningsId { get; set; }
        public virtual Bokning Bokning { get; set; }

        public LektionBokning(string LektionId, string BokningsId, Lektion Lektion, int AntalPersoner, DateTime LektionStartDatum, DateTime LektionSlutDatum, int LektionPris)
        {
            this.LektionId = LektionId;
            this.BokningsId = BokningsId;
            this.Lektion = Lektion;
            this.AntalPersoner = AntalPersoner;
            this.LektionStartDatum = LektionStartDatum;
            this.LektionSlutDatum = LektionSlutDatum;
            this.LektionPris = LektionPris;
        }

        public LektionBokning() { }
    }
}
