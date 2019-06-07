using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class ItemGroup : Entity
    {
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

    }
}