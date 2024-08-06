
using eCom.Web.Models;
using eCom.Web.Service.IService;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
	public class OrderService : IOrderService
    {
		private readonly IBaseService _baseService;

		public OrderService(IBaseService baseService)
		{
			_baseService = baseService;	
		}

        public async Task<ResponseDTO?> CreateOrder(CartDTO cartDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = cartDTO,
                Url = OrderAPIBase + "/api/order/CreateOrder"
            });
        }
	}
}
