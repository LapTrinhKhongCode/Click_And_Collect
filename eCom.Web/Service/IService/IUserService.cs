using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDTO?> GetAllUserAsync();
    }
}
