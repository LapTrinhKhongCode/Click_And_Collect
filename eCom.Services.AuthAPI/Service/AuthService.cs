using eCom.Services.AuthAPI.Data;
using eCom.Services.AuthAPI.Models;
using eCom.Services.AuthAPI.Models.DTO;
using eCom.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace eCom.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext appDbContext, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _appDbContext.ApplicationUsers.FirstOrDefault(temp => temp.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();  
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;    

            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _appDbContext.Users.FirstOrDefault(temp => temp.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password); 
            if(user==null && !isValid)
            {
                return new LoginResponseDTO { User = null, Token = "" };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDTO userDTO = new() {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO
            {
                User = userDTO,
                Token = token
            };
            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name, 
                PhoneNumber = registrationRequestDTO.PhoneNumber
                
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);  
                if(result.Succeeded)
                {
                    var userToReturn = _appDbContext.ApplicationUsers.First(temp => temp.UserName == user.Email);

                    UserDTO userDTO = new()
                    {
                        Email = userToReturn.Email, 
                        ID = userToReturn.Id, 
                        Name = userToReturn.Name, 
                        PhoneNumber = userToReturn.PhoneNumber, 

                    };
                    return "";
                }else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
               
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
