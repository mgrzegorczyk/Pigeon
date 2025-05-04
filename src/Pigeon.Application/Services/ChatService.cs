using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Pigeon.Application.DTOs;
using Pigeon.Application.Extensions;
using Pigeon.Domain.Exceptions;
using Pigeon.Domain.Interfaces.Repositories;
using Pigeon.Infrastructure.Options;
using Pigeon.Infrastructure.Producers;

namespace Pigeon.Application.Services;

public interface IChatService
{
    Task<ChatDto> GetPaginatedChatAsync(Guid chatId, int pageNumber, int pageSize);
    Task SaveMesageAsync(MessageDto messageDto);
}

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IOptions<KafkaOptions> _kafkaOptions;

    public ChatService(IChatRepository chatRepository, IKafkaProducer kafkaProducer,
        IOptions<KafkaOptions> kafkaOptions)
    {
        _chatRepository = chatRepository;
        _kafkaProducer = kafkaProducer;
        _kafkaOptions = kafkaOptions;
    }

    public async Task<ChatDto> GetPaginatedChatAsync(Guid chatId, int pageNumber, int pageSize)
    {
        var paginedChat = await _chatRepository.GetByIdAsync(chatId, pageNumber, pageSize);

        if (paginedChat == null)
        {
            throw new ChatNotFoundException(chatId);
        }

        return paginedChat.ToChatDto();
    }

    public async Task SaveMesageAsync(MessageDto messageDto)
    {
        await _kafkaProducer.ProduceAsync(_kafkaOptions.Value.Producer.ChatMessageReceiveTopic,
            new Message<string, string>()
            {
                Key = messageDto.Id.ToString(),
                Value = JsonSerializer.Serialize(messageDto)
            });
    }
}