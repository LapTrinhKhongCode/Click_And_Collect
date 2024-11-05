using eCom.Web.Models;
using eCom.Web.Service.IService;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
    public class UserService : IUserService
    {

        private readonly IBaseService _baseService;
        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RoleDTO model)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.POST,
                Data = model,
                Url = AuthAPIBase + "/api/user/ManageRole/"
            });
        }

        public async Task<ResponseDTO?> DeleteUserByIdAsync(string id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.DELETE,
                Url = AuthAPIBase + "/api/user/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllUserAsync()
        {

            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.GET,
                Url = AuthAPIBase + "/api/user/"    
            });
        }

        public async Task<ResponseDTO?> GetRoleByUserIdAsync(string id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.GET,
                Url = AuthAPIBase + "/api/user/ManageRole/" + id
            });
        }
    }
}
