﻿namespace eCom.Services.EmailAPI.Message
{
    public class RewardsMessage
    {
        public string UserId { get; set; }
        public int RewardActivity { get; set; }
        public int OrderId { get; set; }
        public string Email { get; set; }
    }
}
