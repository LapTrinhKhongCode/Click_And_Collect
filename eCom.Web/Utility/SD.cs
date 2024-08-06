namespace eCom.Web.Utility
{
	public class SD
	{
		public static string CouponAPIBase { set; get; }	
		public static string AuthAPIBase { set; get; }	
		public static string ProductAPIBase { set; get; }	
		public static string ShoppingCartAPIBase { set; get; }	
		public static string OrderAPIBase { set; get; }	

		public const string RoleAdmin = "ADMIN";	
		public const string RoleCustomer = "CUSTOMER";	
		public const string TokenCookies = "JWTToken";	

		public enum ApiType
		{
			GET, POST, PUT, DELETE
		}

	}
}
