
using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
	public interface IProductService
	{
		Task<ResponseDTO?> GetAllProductAsync();
		Task<ResponseDTO?> GetProductByIdAsync(int id);
		Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
		Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
		Task<ResponseDTO?> DeleteProductAsync(int id);
	}
}
