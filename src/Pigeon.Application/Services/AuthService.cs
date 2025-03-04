using Pigeon.Application.DTOs;
using Pigeon.Application.Helpers;
using Pigeon.Domain;
using Pigeon.Domain.Interfaces.Repositories;

namespace Pigeon.Application.Services;

public interface IAuthService
{
    public Task RegisterUser(RegisterUserDto registerUserDto);
    Task<AuthDto> GetToken(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
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

    public async Task<AuthDto> GetToken(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null) throw new InvalidOperationException($"User with username {username} do not exist!");

        if (!PasswordValid(password, user.Password, user.Salt))
            throw new InvalidOperationException($"Invalid password for user {username}!");

        return _jwtService.GenerateJwtToken(user);
    }

    private bool PasswordValid(string enteredPassword, string storedHashedPassword, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);

        var enteredHashedPassword = PasswordHasher.HashPassword(enteredPassword, salt);

        return storedHashedPassword == enteredHashedPassword.HashedPassword;
    }
}