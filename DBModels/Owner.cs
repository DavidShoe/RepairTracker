using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{

    public partial class Owner
    {
        public int OwnerId { get; set; }

        public string? OwnerName { get; set; }

        public string? Password { get; set; }

        public string? UserName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }


        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}