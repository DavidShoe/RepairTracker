using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class RepairPart
    {
        public RepairPart()
        {
        }
        public RepairPart(int repairId, int quantity, Part part)
        {
            RepairId = repairId;
            Quantity = quantity;
            Cost = part.Cost;
            Sale = part.Sale;
            PartId = part.PartId;
            LineTotal = Sale * Quantity;
        }

        public int RepairPartId { get; set; }

        public int RepairId { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }

        // seeded from the part table, but can be modified in special cases
        public decimal Cost { get; set; }

        // seeded from the part table, but can be modified in special cases
        public decimal Sale { get; set; }

        public decimal LineTotal { get; set; }

        public virtual Part Part { get; set; } = null!;

        public virtual Repair Repair { get; set; } = null!;
    }
}