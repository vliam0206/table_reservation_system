using Application.IServices;
using System.Security.Claims;

namespace WebAPI.Services;

public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }    

    public Guid GetCurrentUserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor
                                .HttpContext?
                                .User?
                                .FindFirstValue(ClaimTypes.SerialNumber);

            return string.IsNullOrEmpty(userIdClaim) ? 
                        Guid.Empty : Guid.Parse(userIdClaim);
        }
    }
}
