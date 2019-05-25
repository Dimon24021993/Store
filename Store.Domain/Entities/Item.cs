using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }

        public virtual List<Rate> Rated { get; set; } = new List<Rate>();
        public virtual List<Picture> Pictures { get; set; } = new List<Picture>();
    }
}