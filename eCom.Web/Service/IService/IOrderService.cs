﻿
using eCom.Web.Models;

namespace eCom.Web.Service.IService
{
	public interface IOrderService
	{
		Task<ResponseDTO?> CreateOrder(CartDTO cartDTO);	
		Task<ResponseDTO?> CreateStripeSession(StripeRequestDTO stripeRequestDTO);	
		Task<ResponseDTO?> ValidateStripeSession(int orderHeaderId);	
		Task<ResponseDTO?> GetAllOrder(string? userId);	
		Task<ResponseDTO?> GetOrder(int orderId);	
		//Task<ResponseDTO?> GetOrderByUserId(string userId);	
        Task<ResponseDTO?> UpdateOrderStatus(int orderId,string newStatus);	
		
	}
}
