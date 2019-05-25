using Store.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Rate : Entity
    {
        public RateType RateType { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }

        public Guid ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}