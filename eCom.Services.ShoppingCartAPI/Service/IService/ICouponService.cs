using eCom.Services.ShoppingCartAPI.Models.DTO;

namespace eCom.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDTO> GetCoupon(string couponCode);
    }
}
