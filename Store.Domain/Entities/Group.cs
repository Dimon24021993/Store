using Store.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Group : Entity
    {
        public Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Group ParentGroup { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int Sort { get; set; }
        public GroupType GroupType { get; set; }
        public virtual List<Group> Children { get; set; } = new List<Group>();
        public virtual List<Picture> Pictures { get; set; } = new List<Picture>();
        public virtual List<Criteria> Criterias { get; set; } = new List<Criteria>();
        public bool Active { get; set; }
        public string Href { get; set; }
    }
}