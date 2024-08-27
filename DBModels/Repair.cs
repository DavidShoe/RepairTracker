using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class Repair
    {
        public int RepairId { get; set; }

        public int GameId { get; set; }

        public int TechnicianId { get; set; }

        public DateOnly? FinishedDate { get; set; }

        public DateOnly? ReceivedDate { get; set; }

        public int OwnerId { get; set; }

        public DateTime? StartDate { get; set; }

        public virtual Game Game { get; set; } = null!;

        public virtual Owner Owner { get; set; } = null!;

        public virtual ICollection<RepairNote> RepairNotes { get; set; } = new List<RepairNote>();

        public virtual ICollection<RepairPart> RepairParts { get; set; } = new List<RepairPart>();

        public virtual Technician Technician { get; set; } = null!;

        public virtual ICollection<WallTime> WallTimes { get; set; } = new List<WallTime>();
    }
}