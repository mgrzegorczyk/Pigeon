using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pigeon.Application.Requests;
using Pigeon.Application.Services;
using Pigeon.Domain.Exceptions;

namespace Pigeon.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost(nameof(GetPaginatedChat))]
    public async Task<IActionResult> GetPaginatedChat(GetPaginatedChatRequest getPaginatedChatRequest)
    {
        try
        {
            var paginedChat = await _chatService.GetPaginatedChatAsync(getPaginatedChatRequest.ChatId,
                getPaginatedChatRequest.PageNumber, getPaginatedChatRequest.PageSize);
            return Json(paginedChat);
        }
        catch (ChatNotFoundException ex)
        {
            return Conflict(ex.Message);
        }
    }
}