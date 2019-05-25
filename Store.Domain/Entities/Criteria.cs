using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Criteria : Entity
    {
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Sort { get; set; }

    }
}