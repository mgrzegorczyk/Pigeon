using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pigeon.Application.DTOs;
using Pigeon.Domain.Models;
using Pigeon.Infrastructure.Options;

namespace Pigeon.Application.Services;

public interface IJwtService
{
    AuthDto GenerateJwtToken(User user);
}

public class JwtService : IJwtService
{
    private readonly JwtSettingsOptions _jwtSettingOptions;

    public JwtService(IOptions<JwtSettingsOptions> jwtSettingOptions)
    {
        _jwtSettingOptions = jwtSettingOptions.Value;
    }

    public AuthDto GenerateJwtToken(User user)
    {
        var claims = GetClaims(user);
        var expiryDate = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettingOptions.ExpiryInMinutes));
        var credentials = GetCredentials();

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: credentials,
            expires: expiryDate
        );

        return new AuthDto()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            ExpiryDate = expiryDate
        };
    }

    private Claim[] GetClaims(User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ];
    }

    private SigningCredentials GetCredentials()
    {
        var byteSecretKey = Encoding.ASCII.GetBytes(_jwtSettingOptions.SecretKey);
        var key = new SymmetricSecurityKey(byteSecretKey);

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }
}