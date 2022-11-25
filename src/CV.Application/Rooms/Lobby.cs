using CV.Application.Interfaces;
using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Rooms;

public class Lobby : IRoom
{
    private readonly ITextDownloaderService _textDownloaderService;
    private readonly IStateService _stateService;
    private readonly List<ILookableObject> _objects = new List<ILookableObject>();
    private readonly List<ILookableObject> _doors = new List<ILookableObject>();    

    public Room Room => Room.Lobby;
    public List<ILookableObject> Objects => _objects;
    public List<ILookableObject> Doors => _doors;

    public Lobby(ITextDownloaderService textService, IStateService stateService)
    {
        _textDownloaderService = textService;
        _stateService = stateService;
    }

    public async Task InitializeRoom()
    {
        _doors.Add(new Door(
            Room.Profile.ToString(), 
            await _textDownloaderService.GetText("objects/door_profile.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Profile", "Profile Door" }));

        _doors.Add(new Door(
            Room.Contact.ToString(),
            await _textDownloaderService.GetText("objects/door_contact.txt"),
            ObjectTextType.Slow,
            new List<string> { "Contact", "Contact Door" }));

        _doors.Add(new Door(
            Room.WorkHistory.ToString(), 
            await _textDownloaderService.GetText("objects/door_experience.txt"),
            ObjectTextType.Slow,
            new List<string> { "Work History", "Work History Door", "Work", "Work Door", "History", "History Door" }));
    }

    public async Task<CommandResponse> Look()
    {
        return new CommandResponse { 
            SlowText = await _textDownloaderService.GetText("rooms/lobby_look.txt"),
            PostWriteStaticText = await _textDownloaderService.GetText("rooms/lobby_viz.txt")
        };        
    }

    public async Task<CommandResponse> Enter()
    {
        await _stateService.SetRoom(this);
        return new CommandResponse { 
            SlowText = await _textDownloaderService.GetText("rooms/lobby_enter.txt"),
            PostWriteStaticText = await _textDownloaderService.GetText("rooms/lobby_viz.txt")
        };
    }

    public async Task<CommandResponse> Exit()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/lobby_exit.txt") };
    }

    public async Task<CommandResponse> AnswerRiddle(string answer)
    {
        return new CommandResponse { SlowText = "Nobody asked you anything." };
    }
}
