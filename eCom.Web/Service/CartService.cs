
using eCom.Web.Models;
using eCom.Web.Service.IService;
using System.Security.Cryptography.Xml;
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
                Url = ShoppingCartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTO?> GetCartByUserIdAsync(string userId)
        {

            var a = await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = ShoppingCartAPIBase + "/api/cart/GetCart/" + userId
            });

            return a;
        }

        public async Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Url = ShoppingCartAPIBase + "/api/cart/RemoveCart/" + cartDetailsId
            });
        }

        public async Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data= cartDTO,
                Url = ShoppingCartAPIBase + "/api/cart/CartUpsert"
            });
        }
    }
}
