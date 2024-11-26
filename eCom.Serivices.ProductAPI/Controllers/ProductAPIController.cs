using AutoMapper;
using eCom.Services.ProductAPI.Data;
using eCom.Services.ProductAPI.Models;
using eCom.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCom.Services.ProductAPI.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{

		private readonly AppDbContext _db;
		private ResponseDTO _response;
		private IMapper _mapper;
		public ProductAPIController(AppDbContext db, IMapper mapper)
		{
			_db = db;
			_response = new ResponseDTO();
			_mapper = mapper;
		}

		[HttpGet]
		public ResponseDTO Get()
		{
			try
			{
				IEnumerable<Product> list = _db.Products.ToList();
				_response.Result = _mapper.Map<IEnumerable<ProductDTO>>(list);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpGet]
		[Route("{id:int}")]
		public ResponseDTO Get(int id)
		{
			try
			{
				Product product = _db.Products.First(temp => temp.ProductId == id);
				_response.Result = _mapper.Map<ProductDTO>(product);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		//[HttpGet]
		//[Route("GetByCode/{code}")]
		//public ResponseDTO GetByCode(string code)
		//{
		//	try
		//	{
		//		Product product = _db.Products.First(temp => temp..ToLower() == code.ToLower());
		//		_response.Result = _mapper.Map<ProductDTO>(product);
		//	}
		//	catch (Exception ex)
		//	{
		//		_response.IsSuccess = false;
		//		_response.Message = ex.Message;
		//	}
		//	return _response;
		//}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Post(ProductDTO productDTO)
		{
			try
			{
				Product product = _mapper.Map<Product>(productDTO);
				_db.Products.Add(product);
				_db.SaveChanges();

				if (productDTO.Image != null)
				{

					string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
					string filePath = @"wwwroot\ProductImages\" + fileName;

					//I have added the if condition to remove the any image with same name if that exist in the folder by any change
					var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					FileInfo file = new FileInfo(directoryLocation);
					if (file.Exists)
					{
						file.Delete();
					}

					var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
					using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
					{
						productDTO.Image.CopyTo(fileStream);
					}
					var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
					product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
					product.ImageLocalPath = filePath;
				}
				else
				{
					product.ImageUrl = "https://placehold.co/600x400";
				}
				_db.Products.Update(product);
				_db.SaveChanges();
				_response.Result = _mapper.Map<ProductDTO>(product);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpPut]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Put(ProductDTO productDTO)
		{
			try
			{
				Product product = _mapper.Map<Product>(productDTO);
                if (productDTO.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                _db.Products.Update(product);
				_db.SaveChanges();

				_response.Result = product;
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpDelete]
		[Route("{id:int}")]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Delete(int id)
		{
			try
			{
				Product product = _db.Products.First(temp => temp.ProductId == id);
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _db.Products.Remove(product);
				_db.SaveChanges();

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
