using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairTracker.DBModels
{

    public partial class Technician
    {
        public int TechnicianId { get; set; }

        [Display(Name = "Tech: ")]
        public string? TechnicianName { get; set; }

        public decimal HourlyRate { get; set; }

        public string? Password { get; set; }

        public virtual ICollection<RepairNote> RepairNotes { get; set; } = new List<RepairNote>();

        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        public virtual ICollection<WallTime> WallTimes { get; set; } = new List<WallTime>();
    }
}