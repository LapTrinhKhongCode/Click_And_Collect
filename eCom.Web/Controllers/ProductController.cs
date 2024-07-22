﻿
using eCom.Services.ProductAPI.Models.DTO;
using eCom.Web.Models;
using eCom.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eCom.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		
		public async Task<IActionResult> ProductIndex()
		{
			List<ProductDTO>? list = new List<ProductDTO>();
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

		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductCreate(ProductDTO productDTO)
		{
			if (ModelState.IsValid)
			{
				ResponseDTO? response = await _productService.CreateProductAsync(productDTO);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product created successfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
			}
			return View(productDTO);
		}
		public async Task<IActionResult> ProductEdit()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDTO productDTO)
		{
			return View();
		}
		public async Task<IActionResult> ProductDelete(int productId)
		{
			ResponseDTO? response = await _productService.GetProductByIdAsync(productId);
			if (response != null && response.IsSuccess)
			{
				ProductDTO? productDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

				return View(productDTO);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}


		[HttpPost]
		public async Task<IActionResult> ProductDelete(ProductDTO productDTO)
		{
			ResponseDTO? response = await _productService.DeleteProductAsync(productDTO.ProductId);
			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product deleted successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}


			return View(productDTO);
		}



	}
}
