using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class RepairPart
    {
        public int RepairPartId { get; set; }

        public int RepairId { get; set; }

        public int PartId { get; set; }

        public int Quantity { get; set; }

        public virtual Part Part { get; set; } = null!;

        public virtual Repair Repair { get; set; } = null!;
    }
}