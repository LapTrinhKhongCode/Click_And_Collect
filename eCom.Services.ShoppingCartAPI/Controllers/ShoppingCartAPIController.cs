using AutoMapper;
using eCom.MessageBus;
using eCom.Services.ShoppingCartAPI.Data;
using eCom.Services.ShoppingCartAPI.Models;
using eCom.Services.ShoppingCartAPI.Models.DTO;
using eCom.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace eCom.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly ICouponService _couponService;
        public ShoppingCartAPIController(IMapper mapper, AppDbContext appDbContext, IProductService productService,
            ICouponService couponService, IMessageBus messageBus, IConfiguration configuration)
        {
            _response = new ResponseDTO();
            _mapper = mapper;
            _appDbContext = appDbContext;
            _productService = productService;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_appDbContext.CartHeaders.
                    First(temp => temp.UserId == userId)),

                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_appDbContext.CartDetails.
                    Where(temp => temp.CartHeaderId == cart.CartHeader.CartHeaderId));

                IEnumerable<ProductDTO> productDTOs = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDTOs.FirstOrDefault(temp => temp.ProductId == item.ProductId);
					cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
				}
				_response.Result = cart;

                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDTO coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;

                    }
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }





        [HttpPost("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _appDbContext.CartDetails.
                    First(temp => temp.CartDetailsId == cartDetailsId);
                int totalCountofCartItem = _appDbContext.CartDetails.
                    Where(temp => temp.CartHeaderId == cartDetailsId).Count(); 
                _appDbContext.CartDetails.Remove(cartDetails);
                if(totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _appDbContext.CartHeaders.
                        FirstOrDefaultAsync(temp => temp.CartHeaderId == cartDetails.CartHeaderId);
                    _appDbContext.Remove(cartHeaderToRemove);
                }
                await _appDbContext.SaveChangesAsync(); 
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDTO> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeaders.FirstAsync(temp => temp.UserId == cartDTO.CartHeader.UserId);
                cartFromDb.CouponCode = cartDTO.CartHeader.CouponCode;
                _appDbContext.CartHeaders.Update(cartFromDb);
                await _appDbContext.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<ResponseDTO> EmailCartRequest([FromBody] CartDTO cartDTO)
        {
            try
            {
                await _messageBus.PublishMessage(cartDTO,_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"));
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDTO.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);
                    _appDbContext.CartHeaders.Add(cartHeader);
                    await _appDbContext.SaveChangesAsync();
                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDTO.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        cartDTO.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDTO.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _appDbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDTO.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                }
                _response.Result = cartDTO;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
