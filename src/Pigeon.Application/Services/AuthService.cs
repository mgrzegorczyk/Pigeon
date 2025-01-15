using Pigeon.Application.DTOs;
using Pigeon.Application.Helpers;
using Pigeon.Domain;
using Pigeon.Domain.Interfaces.Repositories;

namespace Pigeon.Application.Services;

public interface IAuthService
{
    public Task RegisterUser(RegisterUserDto registerUserDto);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task RegisterUser(RegisterUserDto registerUserDto)
    {
        if (String.IsNullOrEmpty(registerUserDto.Username) || String.IsNullOrEmpty(registerUserDto.Username))
            throw new ArgumentException($"Username can't be null or empty!", nameof(registerUserDto.Username));

        if (String.IsNullOrEmpty(registerUserDto.Password) || String.IsNullOrEmpty(registerUserDto.Password))
            throw new ArgumentException($"Password can't be null or empty!", nameof(registerUserDto.Password));

        var existingUser = await _userRepository.GetByUsernameAsync(registerUserDto.Username);

        if (existingUser is not null)
            throw new InvalidOperationException($"User with username {registerUserDto.Username} exist!");

        var hashPasswordResult = PasswordHasher.HashPassword(registerUserDto.Password);

        var newUser = new User(registerUserDto.Username, hashPasswordResult.HashedPassword, hashPasswordResult.Salt);

        await _userRepository.AddAsync(newUser);
    }
}