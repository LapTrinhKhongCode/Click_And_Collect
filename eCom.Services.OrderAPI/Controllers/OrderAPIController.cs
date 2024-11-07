using AutoMapper;
using eCom.MessageBus;
using eCom.Services.OrderAPI.Data;
using eCom.Services.OrderAPI.Models;
using eCom.Services.OrderAPI.Models.DTO;
using eCom.Services.OrderAPI.Service.IService;
using eCom.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace eCom.Services.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private IProductService _productService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        public OrderAPIController( IMapper mapper, AppDbContext appDbContext, IProductService productService, IConfiguration configuration, IMessageBus messageBus)
        {
            _response = new ResponseDTO();
            _mapper = mapper;
            _appDbContext = appDbContext;
            _productService = productService;
            _configuration = configuration;
            _messageBus = messageBus;
        }

        //[Authorize]
        [HttpGet("GetAllOrder")]
        public ResponseDTO? Get(string? userId = "")
        {
            try
            {
                IEnumerable<OrderHeader> objList;
                if (User.IsInRole(SD.RoleAdmin))
                {
                    objList = _appDbContext.OrderHeaders.Include(x => x.OrderDetails).
                        OrderByDescending(temp=>temp.OrderHeaderId).ToList();
                }
                else
                {
                    objList = _appDbContext.OrderHeaders.Include(x => x.OrderDetails).
                        Where(temp => temp.UserId==userId).
                        OrderByDescending(temp => temp.OrderHeaderId).ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDTO>>(objList);
            }
            catch(Exception ex) {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpGet("GetOrders/{id:int}")]
        public ResponseDTO? Get(int id)
        {
            try
            {
                OrderHeader orderHeader = _appDbContext.OrderHeaders.Include(temp => temp.OrderDetails).
                    First(temp => temp.OrderHeaderId==id);  
                _response.Result = _mapper.Map<OrderHeaderDTO>(orderHeader);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetOrdersByUserId/{id}")]
        public ResponseDTO? GetOrdersByUserId(string id)
        {
            try
            {
                var orderHeaders = _appDbContext.OrderHeaders.Include(temp => temp.OrderDetails)
                    .Where(temp => temp.UserId == id).ToList();
                _response.Result = _mapper.Map<IEnumerable<OrderHeaderDTO>>(orderHeaders);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDTO> CreateOrder([FromBody] CartDTO cartDTO)
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(cartDTO.CartHeader); 
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.Status = SD.Status_Pending;
                orderHeaderDTO.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.CartDetails);
                
                OrderHeader orderCreated = _appDbContext.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDTO)).Entity;
                await _appDbContext.SaveChangesAsync();

                orderHeaderDTO.OrderHeaderId = orderCreated.OrderHeaderId;    
                _response.Result = orderHeaderDTO;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("CreateStripeSession")]
        public async Task<ResponseDTO> CreateStripeSession([FromBody] StripeRequestDTO stripeRequestDTO)
        {
            try
            {
               
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = stripeRequestDTO.ApprovedUrl,
                    CancelUrl = stripeRequestDTO.CancelUrl,
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                    
                };

                var DiscountsObj = new List<SessionDiscountOptions>()
                {
                    new SessionDiscountOptions()
                    {
                        Coupon = stripeRequestDTO.OrderHeader.CouponCode
                    }
                };
                foreach(var item in stripeRequestDTO.OrderHeader.OrderDetails)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductName
                            }
                        },
                        Quantity = item.Count
                    };


                    options.LineItems.Add(sessionLineItem);
                }

                if (stripeRequestDTO.OrderHeader.Discount > 0)
                {
                    options.Discounts = DiscountsObj;
                }

                var service = new Stripe.Checkout.SessionService();
                Session session = service.Create(options);
                stripeRequestDTO.StripeSessionUrl = session.Url;
                OrderHeader orderHeader = _appDbContext.OrderHeaders.First(temp => temp.OrderHeaderId == stripeRequestDTO.OrderHeader.OrderHeaderId);
                orderHeader.StripeSessionId = session.Id;
                _appDbContext.SaveChanges();
                _response.Result = stripeRequestDTO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDTO> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            try
            {
                OrderHeader orderHeader = _appDbContext.OrderHeaders.First(temp => temp.OrderHeaderId == orderHeaderId);

                var service = new SessionService();
                Session session = service.Get(orderHeader.StripeSessionId);

                var paymentIntentService = new PaymentIntentService();
                PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

                if (paymentIntent.Status =="succeeded")
                {
                    orderHeader.PaymentIntentId = paymentIntent.Id;
                    orderHeader.Status = SD.Status_Approved;
                    _appDbContext.SaveChanges();
                    RewardsDTO rewardsDTO = new()
                    {
                        OrderId = orderHeader.OrderHeaderId,
                        RewardActivity = Convert.ToInt32(orderHeader.OrderTotal),
                        UserId = orderHeader.UserId,
                        Email = orderHeader.Email
                    };
                    string topicName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
                    await _messageBus.PublishMessage(rewardsDTO, topicName);
                    _response.Result = _mapper.Map<OrderHeaderDTO>(orderHeader);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDTO> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            try
            {
                OrderHeader orderHeader = _appDbContext.OrderHeaders.First(temp => temp.OrderHeaderId == orderId);
                if(orderHeader != null)
                {
                    if (newStatus == SD.Status_Cancelled)
                    {
                        var options = new RefundCreateOptions
                        {
                            Reason = RefundReasons.RequestedByCustomer,
                            PaymentIntent = orderHeader.PaymentIntentId
                        };
                        var service = new RefundService();
                        Refund refund = service.Create(options);
                    }
                        orderHeader.Status = newStatus;
                        _appDbContext.SaveChanges();
                    
                }
            }
            catch(Exception ex) {
                _response.Message = ex.Message;
                _response.IsSuccess = false;          
            }
            return _response;

            
        }


    }
}
