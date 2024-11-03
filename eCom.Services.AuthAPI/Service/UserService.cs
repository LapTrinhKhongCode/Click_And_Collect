using eCom.Services.AuthAPI.Data;
using eCom.Services.AuthAPI.Models;
using eCom.Services.AuthAPI.Models.DTO;
using eCom.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace eCom.Services.AuthAPI.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;      
        public UserService(UserManager<ApplicationUser> userManager, AppDbContext applicationDbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _db = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<UserDTO>> GetAllUser()
        {
            var userList = _db.ApplicationUsers.ToList();
            List<UserDTO> userDTOList = new();
            foreach (var user in userList)
            {
                var user_role = await _userManager.GetRolesAsync(user) as List<string>;
                user.Role = String.Join(", ", user_role);
                UserDTO userDTO = new()
                {
                    Email = user.Email,
                    ID = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                };
                userDTOList.Add(userDTO);
            }
            return userDTOList;    
        }
    }
}
