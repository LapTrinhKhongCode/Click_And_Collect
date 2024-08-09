
using static eCom.Web.Utility.SD;

namespace eCom.Web.Models
{
	public class RequestDTO
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public string Url { get; set; } = string.Empty;
		public object Data { get; set; }	
		public string AccessToken { get; set; }

		public ContentType ContentType {  get; set; } = ContentType.Json;
	}
}
