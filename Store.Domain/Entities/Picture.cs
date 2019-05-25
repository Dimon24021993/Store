using Store.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Domain.Entities
{
    public class Picture : Entity
    {
        public Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public PictureType Type { get; set; }
        public SourceType SourceType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
        public int Sort { get; set; }


    }
}