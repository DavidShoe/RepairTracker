using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class Technician
    {
        public int TechnicianId { get; set; }

        public string? TechnicianName { get; set; }

        public decimal HourlyRate { get; set; }

        public virtual ICollection<RepairNote> RepairNotes { get; set; } = new List<RepairNote>();

        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        public virtual ICollection<WallTime> WallTimes { get; set; } = new List<WallTime>();
    }
}