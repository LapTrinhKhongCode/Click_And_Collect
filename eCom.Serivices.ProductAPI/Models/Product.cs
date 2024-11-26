using System.ComponentModel.DataAnnotations;

namespace eCom.Services.ProductAPI.Models
{
	public class Product
	{
		[Key]
		public int ProductId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int RestaurantId { get; set; }

        [Range(0, 1000)]
		public double Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
        public int OrderCount { get; set; }
        public double Rating { get; set; }
        public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }

	}
}
