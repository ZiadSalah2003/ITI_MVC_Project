using System.ComponentModel.DataAnnotations;

namespace ITI_MVC_Project.Models.Entities
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}