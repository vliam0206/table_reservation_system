using Application.IServices;
namespace WebAPI.Services;

public class JwtBlacklistService : IJwtBlacklistService
{
    private readonly ITokenStore _tokenStore;

    public JwtBlacklistService(ITokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    public void RevokeToken(string token)
    {
        _tokenStore.AddToBlacklist(token);
    }

    public bool IsTokenRevoked(string token)
    {
        return _tokenStore.IsTokenInBlacklist(token);
    }
}
