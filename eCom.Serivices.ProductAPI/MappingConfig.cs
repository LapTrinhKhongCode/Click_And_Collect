using AutoMapper;
using eCom.Services.ProductAPI.Models;
using eCom.Services.ProductAPI.Models.DTO;

namespace eCom.Services.ProductAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<ProductDTO, Product>();
				config.CreateMap<Product, ProductDTO>();
			});
			return mappingConfig;
		}
	}
}
