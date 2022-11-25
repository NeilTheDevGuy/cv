using CV.Domain.Enums;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Services;

public interface IStateService
{
    public Task SetRoom(IRoom newRoom);
    public Task<IRoom> GetRoom();
    public Task SetInputState(State state);
    public Task<State> GetInputState();
}

public class StateService : IStateService
{
    private IRoom _currentRoom;
    private State _currentState;
    
    public async Task<IRoom> GetRoom() => _currentRoom;

    public async Task SetRoom(IRoom newRoom) => _currentRoom = newRoom;

    public async Task<State> GetInputState() => _currentState;

    public async Task SetInputState(State state) => _currentState = state;
}
