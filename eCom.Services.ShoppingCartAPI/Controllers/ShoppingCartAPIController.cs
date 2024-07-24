using AutoMapper;
using eCom.Services.ShoppingCartAPI.Data;
using eCom.Services.ShoppingCartAPI.Models;
using eCom.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCom.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private ResponseDTO _response;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;    

        public ShoppingCartAPIController(ResponseDTO responseDTO, IMapper mapper, AppDbContext appDbContext)
        {
            _response = responseDTO;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeaders.
                    FirstOrDefaultAsync(temp => temp.UserId == cartDTO.CartHeader.UserId);   
                if(cartHeaderFromDb == null)
                {
                    // create header and details 
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeader);   
                    _appDbContext.CartHeaders.Add(cartHeader);
                    await _appDbContext.SaveChangesAsync(); 
                    cartDTO.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _appDbContext.Add(_mapper.Map<CartHeader>(cartDTO.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    // if header is not null
                    //check if details has same product 
                    var cartDetailsFromDb = await _appDbContext.CartDetails.
                        FirstOrDefaultAsync(temp => temp.ProductId == cartDTO.CartDetails.First().ProductId
                        && temp.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails

                    }
                    else
                    {
                        //update count in cart details
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;    
            }

        }

    }
}
