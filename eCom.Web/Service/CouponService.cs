using eCom.Services.CouponAPI.Models.DTO;
using eCom.Web.Models;
using eCom.Web.Service.IService;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
	public class CouponService : ICouponService
	{
		private readonly IBaseService _baseService;

		public CouponService(IBaseService baseService)
		{
			_baseService = baseService;	
		}

		public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO couponDTO)
		{
			return await _baseService.SendAsync(new RequestDTO()
			{
				ApiType = ApiType.POST,
				Data = couponDTO,
				Url = CouponAPIBase + "/api/coupon/"
			});
		}

		public async Task<ResponseDTO?> DeleteCouponAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDTO() {
				ApiType= ApiType.DELETE,
				Url = CouponAPIBase + "/api/coupon/" + id
			});
		}

		public async Task<ResponseDTO?> GetAllCouponAsync()
		{
			return await _baseService.SendAsync(new RequestDTO() {
				ApiType = ApiType.GET,
				Url = CouponAPIBase + "/api/coupon/"
			});
		}

		public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
		{
			return await _baseService.SendAsync(new RequestDTO()
			{
				ApiType = ApiType.GET,
				Url = CouponAPIBase + "/api/coupon/" + couponCode
			});
		}

		public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDTO() { 
				ApiType = ApiType.GET,
				Url = CouponAPIBase + "/api/coupon/" + id
			});
			
		}

		public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO couponDTO)
		{
			return await _baseService.SendAsync(new RequestDTO() { 
				ApiType = ApiType.PUT, 
				Data = couponDTO,
				Url = CouponAPIBase + "/api/coupon/"
			});
		}
	}
}
