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
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService) { 
            _cartService = cartService;

            _orderService = orderService;
        }
        //
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDTOBaseOnLoggedInUser());
        }

		[Authorize]
		public async Task<IActionResult> Checkout()
		{
			return View(await LoadCartDTOBaseOnLoggedInUser());
		}

        [Authorize]
        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {

            CartDTO cart = await LoadCartDTOBaseOnLoggedInUser();
            cart.CartHeader.Phone = cartDTO.CartHeader.Phone;
            cart.CartHeader.Email = cartDTO.CartHeader.Email;
            cart.CartHeader.Name = cartDTO.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            OrderHeaderDTO orderHeaderDTO = JsonConvert.DeserializeObject<OrderHeaderDTO>(Convert.ToString(response.Result));
            if (response != null && response.IsSuccess)
            {

            }
            return View();


        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            return View(orderId);
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.
                Where(temp => temp.Type == JwtRegisteredClaimNames.Sub)?.
                FirstOrDefault()?.Value;
            ResponseDTO? responseDTO = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully!";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            ResponseDTO? responseDTO = await _cartService.ApplyCouponAsync(cartDTO);
            if (responseDTO != null && responseDTO.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully!";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();

        }

		[HttpPost]
		public async Task<IActionResult> EmailCart(CartDTO cartDTO)
		{
            CartDTO cart = await LoadCartDTOBaseOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(temp => temp.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
			ResponseDTO? responseDTO = await _cartService.EmailCart(cart);
			if (responseDTO != null && responseDTO.IsSuccess)
			{
				TempData["success"] = "Email will be processed and sent shortly.";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();

		}

		[HttpPost]
		public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
		{
            cartDTO.CartHeader.CouponCode = "";
			ResponseDTO? responseDTO = await _cartService.ApplyCouponAsync(cartDTO);
			if (responseDTO != null && responseDTO.IsSuccess)
			{
				TempData["success"] = "Coupon applied successfully!";
				return RedirectToAction(nameof(CartIndex));
			}
			return View();

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
