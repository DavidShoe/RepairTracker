using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepairTracker.DBModels
{
    public partial class Game
    {
        public int GameId { get; set; }

        public string? GameName { get; set; }
        [Display(Name = "Game: ")]


        public virtual ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}