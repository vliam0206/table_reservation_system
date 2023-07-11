using Application.Commons;
using Application.IServices;
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
        private readonly IClaimService _claimService;

        public TestController(ILogger<TestController> logger,
            AppConfiguration appConfiguration,
            IClaimService claimService)
        {
            _logger = logger;
            _appConfiguration = appConfiguration;
            _claimService = claimService;
        }

        [HttpGet("db", Name = "GetJsonDB")]
        public string GetDB()
        {
            string dbContext = _appConfiguration.ConnectionStrings.DefaultDB;
            return dbContext;
        }       

        [HttpGet("time-list")]
        public List<string> GetTimeList()
        {
            var timelist = ReservationTime.TimeList;
            return timelist;
        }

        [HttpGet("get-date-time")]
        public string GetDateTime()
        {
            var date = DateTime.Parse(DateTime.Now.AddDays(1).ToString("d/M/yyyy") + " " + "09:00:00");            
            return date.ToString("f");
        }

        [HttpGet("date-format")]
        public string GetDateFormat()
        {
            var date = DateTime.Now.AddDays(1).ToString("d/M/yyyy");
            return date;
        }

        [HttpGet("time-format")]
        public string GetTimeFormat()
        {
            var date = DateTime.Now;
            var time = date.ToString("HH:mm:ss");
            return time;
        }

        [HttpGet("current-id")]
        public Guid GetCurrentId()
        {
            return _claimService.GetCurrentUserId;
        }
    }
}