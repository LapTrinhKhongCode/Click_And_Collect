using eCom.Web.Models;
using eCom.Web.Service.IService;
using eCom.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace eCom.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new();
            return View(loginRequestDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
			ResponseDTO responseDto = await _authService.LoginAsync(loginRequestDTO);

			if (responseDto != null && responseDto.IsSuccess)
			{
				LoginResponseDTO loginResponseDto =
					JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDto.Result));

				await SignInUser(loginResponseDto);
				_tokenProvider.SetToken(loginResponseDto.Token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				TempData["error"] = responseDto.Message;
				return View(loginRequestDTO);
			}
		}

        [HttpGet]
        public IActionResult Register()
        {
			var roleList = new List<SelectListItem>()
			{
				new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
				new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
			};

			ViewBag.RoleList = roleList;
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
		{
			ResponseDTO result = await _authService.RegisterAsync(registrationRequestDTO);
			ResponseDTO assingRole;

			if (result != null && result.IsSuccess)
			{
				if (string.IsNullOrEmpty(registrationRequestDTO.Role))
				{
					registrationRequestDTO.Role = SD.RoleCustomer;
				}
				assingRole = await _authService.AssignRoleAsync(registrationRequestDTO);
				if (assingRole != null && assingRole.IsSuccess)
				{
					TempData["success"] = "Registration Successful";
					return RedirectToAction(nameof(Login));
				}
			}
			else
			{
				TempData["error"] = result.Message;
			}

			var roleList = new List<SelectListItem>()
			{
				new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
				new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
			};

			ViewBag.RoleList = roleList;
			return View(registrationRequestDTO);
		}

		[HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            //TempData["success"] = "Logout Successful";
            return RedirectToAction("Index","Home");
        }

        private async Task SignInUser(LoginResponseDTO loginResponseDTO)
		{
			var handler = new JwtSecurityTokenHandler();

			var jwt = handler.ReadJwtToken(loginResponseDTO.Token);

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


			identity.AddClaim(new Claim(ClaimTypes.Name,
				jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
			identity.AddClaim(new Claim(ClaimTypes.Role,
				jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



			var principal = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		}


	}
}
