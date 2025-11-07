using System.ComponentModel.DataAnnotations;

namespace RowdyBattery.Api.DTOs
{
    public class UpdateItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
    }
}
