using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Item : Entity
    {
        public string ItemNo { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }
        public virtual List<ItemGroup> ItemGroups { get; set; } = new List<ItemGroup>();
        public virtual List<Rate> Rates { get; set; } = new List<Rate>();
        public virtual List<Picture> Pictures { get; set; } = new List<Picture>();
    }
}