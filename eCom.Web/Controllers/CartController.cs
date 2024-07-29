using eCom.Web.Models;
using eCom.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace eCom.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) { 
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDTOBaseOnLoggedInUser());
        }

        public async Task<CartDTO> LoadCartDTOBaseOnLoggedInUser()
        {
            var userId = User.Claims.
                Where(temp => temp.Type == JwtRegisteredClaimNames.Sub)?.
                FirstOrDefault()?.Value;
            ResponseDTO? responseDTO = await _cartService.GetCartByUserIdAsync(userId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {

                CartDTO cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(responseDTO.Result));
                return cartDTO;
            }
            return new CartDTO();
        }





    }
}
