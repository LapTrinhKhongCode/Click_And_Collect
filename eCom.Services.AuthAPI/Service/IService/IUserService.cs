using eCom.Services.AuthAPI.Models;
using eCom.Services.AuthAPI.Models.DTO;

namespace eCom.Services.AuthAPI.Service.IService
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUser();
        Task<RoleDTO> GetRoleByUserId(string id);
        Task<RoleDTO> AssignRole(RoleDTO model);
        Task<string> DeleteUser(string id);

    }
}
