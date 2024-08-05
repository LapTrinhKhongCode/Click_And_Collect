using eCom.MessageBus;
using eCom.Services.AuthAPI.Models.DTO;
using eCom.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eCom.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
		private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
		protected ResponseDTO _response;
        public AuthAPIController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
        {
            _configuration = configuration; 
            _messageBus = messageBus;
            _authService = authService;
            _response = new ResponseDTO();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var errorMessage = await _authService.Register(registrationRequestDTO);
            if (!string.IsNullOrEmpty(errorMessage)) {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
			await _messageBus.PublishMessage(registrationRequestDTO.Email,
                _configuration.GetValue<string>("TopicAndQueueNames:RegisterQueue"));
			return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            LoginResponseDTO loginResponse = await _authService.Login(loginRequestDTO);
            if(loginResponse.User == null) { 
                _response.IsSuccess=false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO RegistrationRequestDTO)
        {
            var assignRoleSuccessfully = await _authService.AssignRole(RegistrationRequestDTO.Email,RegistrationRequestDTO.Role.ToUpper());
            if (!assignRoleSuccessfully)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
