using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace GameRepairApp.Models
{
    public class Owner
    {
        public int OwnerID { get; set; }
        [Display(Name = "Name")]
        public string? OwnerName { get; set; }
        [Display(Name = "Contact")]
        public string? ContactInfo { get; set; }

        public ICollection<Game>? Games { get; set; }
    }

    public class Technician
    {
        public int TechnicianID { get; set; }
        [Display(Name = "Name")]
        public string? TechnicianName { get; set; }
        [Display(Name = "Rate")]
        public decimal HourlyRate { get; set; }

        public ICollection<Repair>? Repairs { get; set; }
    }

    public class Part
    {
        public int PartID { get; set; }
        [Display(Name = "Name")]
        public string? PartName { get; set; }
        public decimal Cost { get; set; }
        public decimal Sale { get; set; }

        public string? Supplier { get; set; }
        public int CategoryID { get; set; }

        public Category Category { get; set; }


        public ICollection<RepairPart>? RepairParts { get; set; }
    }
    public class Category
    {
        public int CategoryID { get; set; }
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        public ICollection<Part> Parts { get; set; }
    }

    public class Game
    {
        public int GameID { get; set; }
        [Display(Name = "Name")]
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
        public DateOnly? ReceivedDate { get; set; }
        public DateOnly? FinishedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public Game? Game { get; set; }
        public Technician? Technician { get; set; }
        public ICollection<RepairPart>? RepairParts { get; set; }

        public ICollection<RepairNote>? RepairNotes { get; set; }
        public ICollection<WallTime>? WallTimes { get; set; }
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

    public class RepairNote
    {
        public int RepairNoteID { get; set; }
        public int RepairID { get; set; }
        public DateTime NoteDate { get; set; } = DateTime.Now;
        public string? Note { get; set; }

        public Repair? Repair { get; set; }
    }

    public class WallTime
    {
        public int WallTimeID { get; set; }
        public int RepairID { get; set; }
        public int TechnicianID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Repair? Repair { get; set; }
    }
}