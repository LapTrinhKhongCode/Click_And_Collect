using eCom.Services.AuthAPI.Data;
using eCom.Services.AuthAPI.Models;
using eCom.Services.AuthAPI.Models.DTO;
using eCom.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

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

        public async Task<RoleDTO> AssignRole(RoleDTO model)
        {
            
            ApplicationUser user = await _userManager.FindByIdAsync(model.User.ID);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var oldUserRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to remove old roles");
            }

            result = await _userManager.AddToRolesAsync(user, model.RolesList
                .Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                throw new Exception("Failed to assign new roles");
            }

            return model;
        }

        public async Task<string> DeleteUser(string id)
        {
            ApplicationUser user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return "User not found";
            }

            _db.ApplicationUsers.Remove(user);
            await _db.SaveChangesAsync();
            return "User deleted successfully";

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

        public async Task<RoleDTO> GetRoleByUserId(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null; // Handle user not found case
            }
            List<string> exsitingUserRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var model = new RoleDTO()
            {
                User = new UserDTO
                {
                    ID = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,

                },
                RolesList = new List<RoleSelection>()
            };

            foreach (var role in _roleManager.Roles)
            {
                var roleSelection = new RoleSelection()
                {
                    RoleName = role.Name ?? string.Empty,
                    IsSelected = exsitingUserRoles.Any(c => c == role.Name)
                };
                model.RolesList.Add(roleSelection);
            }
            return model;
        }
    }
}
