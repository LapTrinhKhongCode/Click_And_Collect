
namespace eCom.Services.RestaurantAPI.Models.DTO
{
	public class RestaurantDTO
	{
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public double RestaurantRating { get; set; }
        public string? RestaurantLocation { get; set; }
        public string? RestaurantDescription { get; set; }
    }
}
