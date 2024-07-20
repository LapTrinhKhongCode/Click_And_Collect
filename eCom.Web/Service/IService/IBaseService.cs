using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
	public interface IBaseService
	{
		Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBear = true);
	}
}
