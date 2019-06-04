using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Article : Entity
    {

        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }

        public virtual List<Picture> Pictures { get; set; } = new List<Picture>();

    }
}