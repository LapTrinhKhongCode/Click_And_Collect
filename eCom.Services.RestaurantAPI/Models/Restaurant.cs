using System.ComponentModel.DataAnnotations;

namespace eCom.Services.RestaurantAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }
        [Required]
        public string RestaurantName { get; set; }

        [Range(0, 5)]
        public double RestaurantRating { get; set; }
        public string? RestaurantLocation { get; set; }
        public string? RestaurantDescription { get; set; }
    }
}
