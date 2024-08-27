using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class WallTime
    {
        public int WallTimeId { get; set; }

        public int RepairId { get; set; }

        public int TechnicianId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual Repair Repair { get; set; } = null!;

        public virtual Technician Technician { get; set; } = null!;
    }
}