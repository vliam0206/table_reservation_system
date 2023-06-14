using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.AuthModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Handle login logic
            // ...

            // Return a successful response with token or user information
            return Ok(new { Token = "your_token", UserId = "user_id" });
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
