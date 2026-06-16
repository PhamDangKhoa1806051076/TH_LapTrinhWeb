using System.ComponentModel.DataAnnotations;

namespace Bai6.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 100000000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        // Category
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
