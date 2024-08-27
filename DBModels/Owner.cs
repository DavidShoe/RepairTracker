using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class Owner
    {
        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public string? ContactInfo { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}