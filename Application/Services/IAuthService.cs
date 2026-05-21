using RealtimeChatAPI.Application.DTOs;

namespace RealtimeChatAPI.Application.Services;

public interface IAuthService
{
     Task<AuthResponse> RegisterAsync (RegisterRequest request);
     Task<AuthResponse> LoginAsync (LoginRequest request);
}