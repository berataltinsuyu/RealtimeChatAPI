using Microsoft.EntityFrameworkCore;
using RealtimeChatAPI.Domain.Entities;
using RealtimeChatAPI.Domain.Interfaces;
using RealtimeChatAPI.Infrastructure.Data;

namespace RealtimeChatAPI.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
       _context = context;
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
       return await _context.Rooms
            .Include(r => r.CreatedBy)
            .FirstOrDefaultAsync(r => r.Id ==id);
    }

    public async Task<List<Room>> GetAllAsync()
    {
       return await _context.Rooms
            .Include(r => r.CreatedBy)
            .Include(r => r.RoomMembers)
            .ToListAsync();
    }

    public async Task<List<RoomMember>> GetMembersAsync(int roomId)
    {
        return await _context.RoomMembers
            .Include( rm => rm.User)
            .Where(rm => rm.RoomId == roomId)
            .ToListAsync();
    }

    public async Task<bool> IsUserMemberAsync(int roomId, int userId)
    {
        return await _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == userId);
    }

    public async Task<Room> CreateAsync(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room;
    }

    public async Task AddMemberAsync(RoomMember member)
    {
        _context.RoomMembers.Add(member);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(int roomId, int userId)
    {
        var member = await _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId);

        if (member != null)
        {
            _context.RoomMembers.Remove(member);
            await _context.SaveChangesAsync();
        }
    }




}