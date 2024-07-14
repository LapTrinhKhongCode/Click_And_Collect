using AutoMapper;
using eCom.Services.CouponAPI.Models;
using eCom.Services.CouponAPI.Models.DTO;
namespace eCom.Services.CouponAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<CouponDTO,Coupon>();
				config.CreateMap<Coupon,CouponDTO>();
			});	
			return mappingConfig;
		}
	}
}
