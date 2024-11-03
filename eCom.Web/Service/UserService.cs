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

        public async Task<ResponseDTO?> GetAllUserAsync()
        {

            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = ApiType.GET,
                Url = AuthAPIBase + "/api/user/"    
            });
        }
    }
}
