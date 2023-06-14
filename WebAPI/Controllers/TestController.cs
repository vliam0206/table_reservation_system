using Application.Commons;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {        
        private readonly ILogger<TestController> _logger;
        private readonly AppConfiguration _appConfiguration;

        public TestController(ILogger<TestController> logger,
            AppConfiguration appConfiguration)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
        }

        [HttpGet("db", Name = "GetJsonDB")]
        public string GetDB()
        {
            string dbContext = _appConfiguration.ConnectionStrings.DefaultDB;
            return dbContext;
        }
        [HttpGet("secret-key", Name = "GetJsonSecretKey")]
        public string GetSecretKey()
        {
            string key = _appConfiguration.JwtConfiguration.SecretKey;
            return key;
        }
        [HttpGet("time-list")]
        public List<string> GetTimeList()
        {
            var timelist = ReservationTime.TimeList;
            return timelist;
        }
        [HttpGet("test-date-time")]
        public string GetDateTime()
        {
            var date = DateTime.Parse(DateTime.Now.AddDays(1).ToString("M/d/yyyy") + " " + "09:00:00");            
            return date.ToString("f");
        }
        [HttpGet("test-date")]
        public string GetDateFormat()
        {
            var date = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            return date;
        }
        [HttpGet("test-time")]
        public string GetTimeFormat()
        {
            var date = DateTime.Now;
            var time = date.ToString("HH:mm:ss");
            return time;
        }
    }
}