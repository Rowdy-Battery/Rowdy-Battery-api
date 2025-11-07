using System.ComponentModel.DataAnnotations;

namespace RowdyBattery.Api.DTOs
{
    public class CreateItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
    }
}
