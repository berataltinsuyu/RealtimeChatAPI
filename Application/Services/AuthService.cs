using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealtimeChatAPI.Application.DTOs;
using RealtimeChatAPI.Domain.Entities;
using RealtimeChatAPI.Domain.Interfaces;

namespace RealtimeChatAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new Exception("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new Exception("Password is required.");
        }

        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);

        if (existingUser is not null)
        {
            throw new Exception("Username is already taken.");
        }

        var user = new User
        {
            Username = request.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.CreateAsync(user);
        var token = GenerateJwtToken(createdUser);

        return new AuthResponse
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            Token = token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            throw new Exception("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new Exception("Password is required.");
        }

        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new Exception("Invalid username or password.");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);

        return new AuthResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = _configuration["Jwt:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new Exception("JWT secret key is missing.");
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expirationMinutes = Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}