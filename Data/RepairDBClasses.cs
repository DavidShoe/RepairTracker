using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace GameRepairApp.Models
{
    public class Owner
    {
        public int OwnerID { get; set; }
        public string? OwnerName { get; set; }
        public string? ContactInfo { get; set; }

        public ICollection<Game>? Games { get; set; }
    }

    public class Technician
    {
        public int TechnicianID { get; set; }
        public string? TechnicianName { get; set; }
        public decimal HourlyRate { get; set; }

        public ICollection<Repair>? Repairs { get; set; }
    }

    public class Part
    {
        public int PartID { get; set; }
        public string? PartName { get; set; }
        public decimal Cost { get; set; }

        public ICollection<RepairPart>? RepairParts { get; set; }
    }

    public class Game
    {
        public int GameID { get; set; }
        public string? GameName { get; set; }
        public int OwnerID { get; set; }

        public Owner? Owner { get; set; }
        public ICollection<Repair>? Repairs { get; set; }
    }

    public class Repair
    {
        public int RepairID { get; set; }
        public int GameID { get; set; }
        public int TechnicianID { get; set; }
        public DateTime RepairDate { get; set; } = DateTime.Now;
        public string? RepairNotes { get; set; }

        public Game? Game { get; set; }
        public Technician? Technician { get; set; }
        public ICollection<RepairPart>? RepairParts { get; set; }
    }

    public class RepairPart
    {
        public int RepairPartID { get; set; }
        public int RepairID { get; set; }
        public int PartID { get; set; }
        public int Quantity { get; set; }

        public Repair? Repair { get; set; }
        public Part? Part { get; set; }
    }
}
