using System;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}