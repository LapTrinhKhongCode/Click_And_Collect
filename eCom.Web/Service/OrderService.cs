
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

        public async Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = stripeRequestDTO,
                Url = OrderAPIBase + "/api/order/CreateStripeSession"
            });
        }

        public async Task<ResponseDTO?> GetAllOrder(string? userId)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,            
                Url = OrderAPIBase + "/api/order/GetAllOrder?userId=" + userId
            });
        }

        public async Task<ResponseDTO?> GetOrder(int orderId)
        {

            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.GET,
                Url = OrderAPIBase + "/api/order/GetOrders/" + orderId
            });
        }

        //public async Task<ResponseDTO?> GetOrderByUserId(string userId)
        //{
        //    return await _baseService.SendAsync(new RequestDTO()
        //    {
        //        ApiType = ApiType.GET,
        //        Url = OrderAPIBase + "/api/order/GetOrders/" + userId
        //    });
        //}

        public async Task<ResponseDTO?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = newStatus,
                Url = OrderAPIBase + "/api/order/UpdateOrderStatus/" + orderId
            });
        }

        public async Task<ResponseDTO?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = ApiType.POST,
                Data = orderHeaderId,
                Url = OrderAPIBase + "/api/order/ValidateStripeSession"
            });
        }
    }
}
