using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairTracker.DBModels
{

    public partial class Repair
    {
        public int RepairId { get; set; }

        [Display(Name = "Game: ")]
        public int GameId { get; set; }

        [Display(Name = "Tech: ")]
        public int TechnicianId { get; set; }

        public DateOnly? FinishedDate { get; set; }
        [Display(Name = "Received Date")]

        public DateOnly? ReceivedDate { get; set; }

        [Display(Name = "Owner: ")]
        public int OwnerId { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Customer States")]
        public String? CustomerStates { get; set; }

        public virtual Game Game { get; set; } = null!;

        public virtual Owner Owner { get; set; } = null!;

        public virtual ICollection<RepairNote> RepairNotes { get; set; } = new List<RepairNote>();

        public virtual ICollection<RepairPart> RepairParts { get; set; } = new List<RepairPart>();

        public virtual Technician Technician { get; set; } = null!;

        public virtual ICollection<WallTime> WallTimes { get; set; } = new List<WallTime>();
    }
}