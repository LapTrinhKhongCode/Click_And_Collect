using eCom.Services.ShoppingCartAPI.Models.DTO;

namespace eCom.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
