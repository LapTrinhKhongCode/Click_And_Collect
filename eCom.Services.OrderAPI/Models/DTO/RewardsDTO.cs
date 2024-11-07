namespace eCom.Services.OrderAPI.Models.DTO
{
    public class RewardsDTO
    {
        public string UserId { get; set; }
        public int RewardActivity { get; set; }   
        public int OrderId {  get; set; }
        public string Email { get; set; }   

    }
}
