using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class Part
    {
        public int PartId { get; set; }

        public string? PartName { get; set; }

        public decimal Cost { get; set; }

        public decimal Sale { get; set; }

        public string? Supplier { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<RepairPart> RepairParts { get; set; } = new List<RepairPart>();
    }
}