using AutoMapper;
using eCom.Services.RestaurantAPI.Models;
using eCom.Services.RestaurantAPI.Models.DTO;

namespace eCom.Services.RestaurantAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<RestaurantDTO, Restaurant>();
				config.CreateMap<Restaurant, RestaurantDTO>();
			});
			return mappingConfig;
		}
	}
}
