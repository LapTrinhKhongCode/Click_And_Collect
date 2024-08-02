using AutoMapper;
using eCom.Services.ShoppingCartAPI.Models;
using eCom.Services.ShoppingCartAPI.Models.DTO;

namespace eCom.Services.ShoppingCartAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
				config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
				
			});
			return mappingConfig;
		}
	}
}
