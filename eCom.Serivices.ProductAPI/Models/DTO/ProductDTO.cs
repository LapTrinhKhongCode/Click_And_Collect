
namespace eCom.Services.ProductAPI.Models.DTO
{
	public class ProductDTO
	{
		
		public int ProductId { get; set; }
		
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public int OrderCount { get; set; } 
        public double Rating { get; set; } 
        public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		public IFormFile? Image { get; set; }

	}
}
