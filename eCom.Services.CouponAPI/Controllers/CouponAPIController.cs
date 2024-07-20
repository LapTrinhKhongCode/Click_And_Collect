using AutoMapper;
using eCom.Services.CouponAPI.Data;
using eCom.Services.CouponAPI.Models;
using eCom.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCom.Services.CouponAPI.Controllers
{
	[Route("api/coupon")]
	[ApiController]
	[Authorize]
	public class CouponAPIController : ControllerBase
	{
		private readonly AppDbContext _db; 
		private ResponseDTO _response;
		private IMapper _mapper;
		public CouponAPIController(AppDbContext db, IMapper mapper)
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
				IEnumerable<Coupon> list = _db.Coupons.ToList();
				_response.Result = _mapper.Map<IEnumerable<CouponDTO>>(list);
			}
			catch(Exception ex)
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
				Coupon coupon = _db.Coupons.First(temp => temp.CouponId == id);
				_response.Result = _mapper.Map<CouponDTO>(coupon);
			}
			catch (Exception ex)
			{	
				_response.IsSuccess = false;	
				_response.Message = ex.Message;	
			}
			return _response;
		}

		[HttpGet]
		[Route("GetByCode/{code}")]
		public ResponseDTO GetByCode(string code)
		{
			try
			{
				Coupon coupon = _db.Coupons.First(temp => temp.CouponCode.ToLower() == code.ToLower());	
				_response.Result = _mapper.Map<CouponDTO>(coupon);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public ResponseDTO Post([FromBody]CouponDTO couponDTO)
		{
			try
			{
				Coupon coupon = _mapper.Map<Coupon>(couponDTO);	
				_db.Coupons.Add(coupon);
				_db.SaveChanges();

				_response.Result = coupon;
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
		public ResponseDTO Put([FromBody] CouponDTO couponDTO)
		{
			try
			{
				Coupon coupon = _mapper.Map<Coupon>(couponDTO);
				_db.Coupons.Update(coupon);
				_db.SaveChanges();

				_response.Result = coupon;
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
				Coupon coupon = _db.Coupons.First(temp => temp.CouponId == id);
				_db.Coupons.Remove(coupon);
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
