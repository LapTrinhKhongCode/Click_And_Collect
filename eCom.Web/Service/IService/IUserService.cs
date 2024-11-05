using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDTO?> GetAllUserAsync();
        Task<ResponseDTO?> DeleteUserByIdAsync(string id);
        Task<ResponseDTO?> GetRoleByUserIdAsync(string id);

    }
}
