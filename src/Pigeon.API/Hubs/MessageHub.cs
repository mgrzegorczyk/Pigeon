using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Pigeon.Application.DTOs;
using Pigeon.Application.Services;
using Pigeon.Infrastructure.Options;

namespace Pigeon.API.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private readonly ILogger<MessageHub> _logger;
    private readonly IUserConnectionService _userConnectionService;
    private readonly IChatService _chatService;
    private readonly IOptions<KafkaOptions> _kafkaOptions;
    private readonly IOptions<MessageHubOptions> _messageHubOptions;
    private string _userId => _userConnectionService.GetClaimValue(Context.User, ClaimTypes.NameIdentifier);

    public MessageHub(ILogger<MessageHub> logger,
        IUserConnectionService userConnectionService,
        IChatService chatService,
        IOptions<KafkaOptions> kafkaOptions,
        IOptions<MessageHubOptions> messageHubOptions)
    {
        _logger = logger;
        _userConnectionService = userConnectionService;
        _chatService = chatService;
        _kafkaOptions = kafkaOptions;
        _messageHubOptions = messageHubOptions;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"[{nameof(MessageHub)}] Client connected: {Context.ConnectionId}");
        _userConnectionService.AddConnection(_userId, Context.ConnectionId);
        await JoinToChat(_messageHubOptions.Value.DefaultChatId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation(
            $"[{nameof(MessageHub)}] Client disconnected: {Context.ConnectionId}. Reason: {exception?.Message ?? "None"}");
        _userConnectionService.RemoveConnection(_userId);
        await LeaveChat(_messageHubOptions.Value.DefaultChatId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToChat(string user, string message, string chatId)
    {
        var messageDto = CreateMessage(chatId, _userId, message);

        _logger.LogInformation(
            $"[{nameof(MessageHub)}] Recived chat {chatId} message: {message} from: {user}");
        await Clients.Group(chatId).SendAsync(_messageHubOptions.Value.ReceiveMessage, messageDto);

        await _chatService.SaveMesageAsync(messageDto);
    }

    private MessageDto CreateMessage(string chatId, string userId, string message)
    {
        return new MessageDto(Guid.NewGuid(),
            Guid.Parse(chatId),
            Guid.Parse(userId),
            message,
            DateTime.UtcNow);
    }

    private async Task JoinToChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    private async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}