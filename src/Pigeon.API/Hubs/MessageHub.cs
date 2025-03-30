using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pigeon.Application.Services;

namespace Pigeon.API.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private readonly ILogger<MessageHub> _logger;
    private readonly IUserConnectionService _userConnectionService;
    private const string _mainGroupName = "main";
    private string _username => _userConnectionService.GetClaimValue(Context.User, ClaimTypes.NameIdentifier);

    public MessageHub(ILogger<MessageHub> logger,
        IUserConnectionService userConnectionService)
    {
        _logger = logger;
        _userConnectionService = userConnectionService;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"[{nameof(MessageHub)}] Client connected: {Context.ConnectionId}");
        _userConnectionService.AddConnection(_username, Context.ConnectionId);
        await JoinToGroup(_mainGroupName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation(
            $"[{nameof(MessageHub)}] Client disconnected: {Context.ConnectionId}. Reason: {exception?.Message ?? "None"}");
        _userConnectionService.RemoveConnection(_username);
        await LeaveGroup(_mainGroupName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        _logger.LogInformation(
            $"[{nameof(MessageHub)}] Recived message: {message} from: {user}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendMessageToGroup(string user, string message, string groupName)
    {
        _logger.LogInformation(
            $"[{nameof(MessageHub)}] Recived group {groupName} message: {message} from: {user}");
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    private async Task JoinToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    private async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}