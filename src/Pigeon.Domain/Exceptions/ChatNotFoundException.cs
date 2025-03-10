namespace Pigeon.Domain.Exceptions;

public class ChatNotFoundException(Guid id) : Exception($"Chat with ID {id} not found!");