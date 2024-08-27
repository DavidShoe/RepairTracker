using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{
    public partial class Game
    {
        public int GameId { get; set; }

        public string? GameName { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}