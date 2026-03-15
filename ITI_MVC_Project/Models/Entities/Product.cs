using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_MVC_Project.Models.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}