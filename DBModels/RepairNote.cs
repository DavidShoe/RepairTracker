using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class RepairNote
    {
        public int RepairNoteId { get; set; }

        public int RepairId { get; set; }

        public DateTime NoteDate { get; set; }

        public int TechnicianId { get; set; }

        public string? Note { get; set; }

        public virtual Repair Repair { get; set; } = null!;

        public virtual Technician Technician { get; set; } = null!;
    }
}