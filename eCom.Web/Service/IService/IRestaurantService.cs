
using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
	public interface IRestaurantService
    {
        Task<ResponseDTO?> GetAllRestaurantsAsync();
        Task<ResponseDTO?> GetRestaurantByIdAsync(int id);
        Task<ResponseDTO?> CreateRestaurantAsync(RestaurantDTO restaurantDTO);
        Task<ResponseDTO?> UpdateRestaurantAsync(RestaurantDTO restaurantDTO);
        Task<ResponseDTO?> DeleteRestaurantAsync(int id);
    }
}
