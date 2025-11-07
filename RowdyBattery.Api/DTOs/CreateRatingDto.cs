using System.ComponentModel.DataAnnotations;

namespace RowdyBattery.Api.DTOs
{
    public class CreateRatingDto
    {
        [Range(1, 5)]
        public int Stars { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        // Optional review
        public string? Review { get; set; }
    }
}
