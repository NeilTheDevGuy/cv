using CV.Domain.Enums;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Services;

public interface IRoomsService
{
    Task RegisterRoom(IRoom room);
    Task<IRoom?> GetRoom(Room room);
}

public class RoomsService : IRoomsService
{
    private readonly HashSet<IRoom> _rooms = new HashSet<IRoom>();

    public async Task<IRoom?> GetRoom(Room room)
    {
        return _rooms.FirstOrDefault(r => r.Room == room);
    }

    public async Task RegisterRoom(IRoom room)
    {
        _rooms.Add(room);
    }
}
