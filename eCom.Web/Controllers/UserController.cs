using eCom.Web.Models;
using eCom.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace eCom.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService; 
        }
        public async Task<IActionResult> Index()
        {
            List<UserDTO>? list = new List<UserDTO>();
            ResponseDTO? response = await _userService.GetAllUserAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ResponseDTO? response = await _userService.DeleteUserByIdAsync(id);
            if (response != null && response.IsSuccess)
            {
				TempData["success"] = "User deleted successfully";
				return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpGet]
		public async Task<IActionResult> ManageRole()
		{
            string userId = Request.Query["userId"].ToString();
            ResponseDTO? response = await _userService.GetRoleByUserIdAsync(userId);

            if (response != null && response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<RoleDTO>(Convert.ToString(response.Result));
                return View(result);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(Index)); 
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole(RoleDTO model)
        {
            ResponseDTO? response = await _userService.AssignRoleAsync(model);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Role updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
        }   

    }
}
