
using eCom.Web.Models;
using eCom.Web.Service.IService;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
	public class RestaurantService : IRestaurantService
	{
		private readonly IBaseService _baseService;
		public RestaurantService(IBaseService baseService)
		{
			_baseService = baseService;
		}

        public async Task<ResponseDTO?> GetAllRestaurantsAsync()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.GET,
                Url = RestaurantAPIBase + "/api/restaurant"
            });
        }


        public Task<ResponseDTO?> GetRestaurantByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task<ResponseDTO?> CreateRestaurantAsync(RestaurantDTO restaurantDTO)
        {
            throw new NotImplementedException();
        }



        public Task<ResponseDTO?> UpdateRestaurantAsync(RestaurantDTO restaurantDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO?> DeleteRestaurantAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
