using Application.Commons;
using Application.Utils;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.AuthModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AppConfiguration _configuration;
        private IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork, AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var response = new ApiResponse();
            try
            {
                var account = _unitOfWork.AccountRepository.GetAccount(model.UserName);
                if (account == null || !model.Password.Verify(account.Password))
                {
                    response.Success = false;
                    response.ErrorMessage = "Wrong username/password!";
                } else
                {
                    var tokenModel = new TokenModel();
                    tokenModel.Token = account.GenerateJsonWebToken(
                                                        _configuration.JwtConfiguration.SecretKey,
                                                        DateTime.Now);
                    response.Success = true;
                    response.Data = tokenModel;
                }
            } catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Perform logout logic
            // ...

            // Return a successful response or a specific message indicating successful logout
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
