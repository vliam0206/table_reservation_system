using Application.IServices;

namespace WebAPI.Services;

// Using an in-memory HashSet by using singleton lifecycle DI
public class InMemoryTokenStore : ITokenStore
{
    private readonly HashSet<string> _blacklist;

    public InMemoryTokenStore()
    {
        _blacklist = new HashSet<string>();
    }

    public void AddToBlacklist(string token)
    {
        _blacklist.Add(token);
    }

    public bool IsTokenInBlacklist(string token)
    {
        return _blacklist.Contains(token);
    }
}
