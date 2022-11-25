using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;

namespace CV.Domain.Rooms.Interfaces;

public interface IRoom
{
    public Room Room { get; }
    public List<ILookableObject> Objects { get; } //Objects are things you can look at
    public List<ILookableObject> Doors { get; } //Doors are things you can go through to another room.
    public Task InitializeRoom();
    public Task<CommandResponse> Look();
    public Task<CommandResponse> Enter();
    public Task<CommandResponse> Exit();
    public Task<CommandResponse> AnswerRiddle(string answer);    
}
