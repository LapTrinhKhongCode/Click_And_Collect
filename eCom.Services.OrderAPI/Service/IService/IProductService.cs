using eCom.Services.OrderAPI.Models.DTO;

namespace eCom.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
