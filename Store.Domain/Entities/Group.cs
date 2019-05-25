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
        public int Sort { get; set; }
        public GroupType GroupType { get; set; }
        public virtual List<Group> Children { get; set; } = new List<Group>();
        public virtual List<Picture> Pictures { get; set; } = new List<Picture>();
        public bool Active { get; set; }
    }
}