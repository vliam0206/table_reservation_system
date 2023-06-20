using Application.Commons;
using Application.IServices;
using Application.Utils;
using Infrastructures.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
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
        private IClaimService _claimService;

        public AuthController(IUnitOfWork unitOfWork, 
                                    AppConfiguration configuration,
                                    IClaimService claimService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _claimService = claimService;
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
                    var token = account.GenerateJsonWebToken(
                                                        _configuration.JwtConfiguration.SecretKey,
                                                        DateTime.Now);
                    response.Success = true;
                    response.Data = token;
                }
            } catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("current-id")]
        [Authorize]
        public IActionResult GetCurrentId()
        {
            var response = new ApiResponse()
            {
                Success = true,
                Data = _claimService.GetCurrentUserId
            };
            return Ok(response);
        }

        [HttpGet("current-username")]
        [Authorize]
        public IActionResult GetCurrentUserName()
        {
            var response = new ApiResponse()
            {
                Success = true,
                Data = _claimService.GetCurrentUserName
            };
            return Ok(response);
        }

        //[HttpPost("logout")]
        //[Authorize]
        //public IActionResult Logout()
        //{
        //    return Ok(new ApiResponse()
        //    {
        //        Success = false,
        //        ErrorMessage = "This Log Out api is just used to log out in Postman. Log out must be implemented in client-side."
        //    });
        //}

        //[HttpPost("logout")]
        //[Authorize]
        //public IActionResult Logout()
        //{
        //    var response = new ApiResponse();
        //    // Get the token from the request (from header)
        //    string token = GetTokenFromRequest();
        //    // Revoke the token by adding it to the blacklist
        //    _jwtBlacklistService.RevokeToken(token);

        //    return Ok(new ApiResponse() { Success=true });
        //}

        //private string GetTokenFromRequest()
        //{
        //    string authorizationHeader = Request.Headers["Authorization"];
        //    if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return authorizationHeader.Substring("Bearer ".Length).Trim();
        //    }

        //    return null;
        //}

    }
}
