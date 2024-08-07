using eCom.Web.Models;
using eCom.Web.Service.IService;
using eCom.Web.Utility;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace eCom.Web.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;   
        }

        public IActionResult OrderIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeaderDTO> list;
            string userId = "7a77af99-fda9-4282-a937-d7d0d5c6bcf1";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(temp => temp.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponseDTO response = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDTO>>(Convert.ToString(response.Result));

            }
            else
            {
                list = new List<OrderHeaderDTO>();  
            }
            return Json(new { data = list });
        }
        
    }
}
