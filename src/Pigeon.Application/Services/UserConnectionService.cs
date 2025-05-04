using System.Collections.Concurrent;
using System.Security.Claims;

namespace Pigeon.Application.Services;

public interface IUserConnectionService
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId);
    string GetClaimValue(ClaimsPrincipal? principal, string claimType);
}

public class UserConnectionService : IUserConnectionService
{
    private readonly ConcurrentDictionary<string, string> _userConnections = new();

    public void AddConnection(string userId, string connectionId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            _userConnections.TryAdd(userId, connectionId);
        }
    }

    public void RemoveConnection(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            _userConnections.TryRemove(userId, out _);
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