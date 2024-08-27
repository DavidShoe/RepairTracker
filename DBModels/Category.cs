using System;
using System.Collections.Generic;

namespace RepairTracker.DBModels
{


    public partial class Category
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}