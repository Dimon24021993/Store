using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class CriteriaItem : Entity
    {
        public Guid? CriteriaId { get; set; }
        [ForeignKey("CriteriaId")]
        public virtual Criteria Criteria { get; set; }
        public Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public string Value { get; set; }
    }
}