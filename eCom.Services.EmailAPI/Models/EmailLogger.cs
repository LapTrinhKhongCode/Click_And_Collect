namespace eCom.Services.EmailAPI.Models
{
	public class EmailLogger
	{
		public int id { get; set; }	
		public string Message { get; set; }
		public string Email { get; set; }
		public DateTime? EmailSent { get; set; }

	}
}
