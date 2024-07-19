using eCom.Web.Models;
using eCom.Web.Service.IService;
using eCom.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
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
            ResponseDTO responseDTO = await _authService.LoginAsync(loginRequestDTO);

            if (responseDTO != null && responseDTO.IsSuccess)
            {

                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDTO.Result));
                _tokenProvider.SetToken(loginResponseDTO.Token);

                await SignInUser(loginResponseDTO);
                TempData["success"] = "Login Successful";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDTO.Message);
                return View(loginRequestDTO);
            }

        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ResponseDTO result = await _authService.RegisterAsync(registrationRequestDTO);
            ResponseDTO assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDTO.Role))
                {
                    registrationRequestDTO.Role = SD.RoleCustomer;

                }
                assignRole = await _authService.AssignRoleAsync(registrationRequestDTO);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }


            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{Text = SD.RoleCustomer, Value = SD.RoleCustomer}
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
            var handle = new JwtSecurityTokenHandler();
            var jwt = handle.ReadJwtToken(loginResponseDTO.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(temp => temp.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(temp => temp.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(temp => temp.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(temp => temp.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }

    }
}
