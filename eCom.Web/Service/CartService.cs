
using eCom.Web.Models;
using eCom.Web.Service.IService;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
	public class CartService : ICartService
	{
		private readonly IBaseService _baseService;

		public CartService(IBaseService baseService)
		{
			_baseService = baseService;	
		}

        public async Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = cartDTO,
                Url = CouponAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTO?> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = CouponAPIBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Url = CouponAPIBase + "/api/cart/RemoveCart/" + cartDetailsId
            });
        }

        public async Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data= cartDTO,
                Url = CouponAPIBase + "/api/cart/CartUpsert"
            });
        }
    }
}
