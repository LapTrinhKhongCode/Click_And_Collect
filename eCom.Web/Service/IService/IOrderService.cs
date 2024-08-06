
using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
	public interface IOrderService
	{
		Task<ResponseDTO?> CreateOrder(CartDTO cartDTO);	
	}
}
