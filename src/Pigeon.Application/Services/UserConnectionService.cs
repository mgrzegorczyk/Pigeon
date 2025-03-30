using System.Collections.Concurrent;
using System.Security.Claims;

namespace Pigeon.Application.Services;

public interface IUserConnectionService
{
    void AddConnection(string username, string connectionId);
    void RemoveConnection(string username);
    string GetClaimValue(ClaimsPrincipal? principal, string claimType);
}

public class UserConnectionService : IUserConnectionService
{
    private readonly ConcurrentDictionary<string, string> _userConnections = new();

    public void AddConnection(string username, string connectionId)
    {
        if (!string.IsNullOrEmpty(username))
        {
            _userConnections.TryAdd(username, connectionId);
        }
    }

    public void RemoveConnection(string username)
    {
        if (!string.IsNullOrEmpty(username))
        {
            _userConnections.TryRemove(username, out _);
        }
    }

    public string GetClaimValue(ClaimsPrincipal? principal, string claimType)
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal), $"Principal cannot be null");

        var claimValue = principal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;

        if (claimValue is null) throw new NullReferenceException($"Empty claim found for type {claimType}");

        return claimValue;
    }
}