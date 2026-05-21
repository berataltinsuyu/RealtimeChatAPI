using RealtimeChatAPI.Domain.Entities;

namespace RealtimeChatAPI.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> CreateAsync (User user);
}