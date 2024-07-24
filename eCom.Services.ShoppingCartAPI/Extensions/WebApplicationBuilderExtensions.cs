using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eCom.Services.ShoppingCartAPI.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
		{

			var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
			var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
			var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

			var key = Encoding.ASCII.GetBytes(secret);

			builder.Services.AddAuthentication(temp =>
			{
				temp.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				temp.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(temp =>
			{
				temp.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = issuer,
					ValidAudience = audience,
					ValidateAudience = true
				};
			});
			return builder;
		}
	}
}
