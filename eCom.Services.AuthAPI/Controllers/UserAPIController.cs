using eCom.Services.AuthAPI.Models.DTO;
using eCom.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace eCom.Services.AuthAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IUserService _userService;
        public UserAPIController(IUserService userService)
        {
            _userService = userService;
            _response = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> Index()
        {
            try
            {               
                _response.Result = await _userService.GetAllUser();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("ManageRole/{id}")]
        public async Task<ResponseDTO> GetRoleByUserId(string id)
        {
            try
            {
                _response.Result = await _userService.GetRoleByUserId(id);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        [Route("ManageRole")]
        public async Task<ResponseDTO> AssignRole(RoleDTO model)
        {
            try
            {
                _response.Result = await _userService.AssignRole(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ResponseDTO> Delete(string id)
        {
            try
            {
                _response.Result = await _userService.DeleteUser(id);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }





    }
}
