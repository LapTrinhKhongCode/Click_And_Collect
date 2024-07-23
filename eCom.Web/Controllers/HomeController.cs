using eCom.Services.ProductAPI.Models.DTO;
using eCom.Web.Models;
using eCom.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace eCom.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IProductService _productService;
		public HomeController(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IActionResult> Index()
		{
			List<ProductDTO?> list = new();
			ResponseDTO? response = await _productService.GetAllProductAsync();
			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(list);
		}

		public async Task<IActionResult> ProductDetails(int productId)
		{
			ProductDTO? product = new();
			ResponseDTO? response = await _productService.GetProductByIdAsync(productId);
			if (response != null && response.IsSuccess)
			{
				product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(product);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
