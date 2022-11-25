using CV.Application.Interfaces;
using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Rooms;

public class Profile : IRoom
{
    private readonly ITextDownloaderService _textDownloaderService;
    private readonly IStateService _stateService;
    private readonly List<ILookableObject> _objects = new List<ILookableObject>();
    private readonly List<ILookableObject> _doors = new List<ILookableObject>();

    public Room Room => Room.Profile;
    public List<ILookableObject> Objects => _objects;
    public List<ILookableObject> Doors => _doors;

    public Profile(ITextDownloaderService textService, IStateService stateService)
    {
        _textDownloaderService = textService;
        _stateService = stateService;
    }

    public async Task InitializeRoom()
    {
        _doors.Add(new Door(
            Room.Lobby.ToString(), 
            await _textDownloaderService.GetText("objects/door_lobby.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Lobby", "Lobby Door" }));

        _objects.Add(new GeneralObject(
            "Wallpaper", 
            await _textDownloaderService.GetText("objects/profile_wallpaper.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Wallpaper", "Wall", "Paper" }));
    }

    public async Task<CommandResponse> Look()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/profile_look.txt") };
    }

    public async Task<CommandResponse> Enter()
    {
        await _stateService.SetRoom(this);
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/profile_enter.txt") };

    }

    public async Task<CommandResponse> Exit()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/profile_exit.txt") };
    }

    public async Task<CommandResponse> AnswerRiddle(string answer)
    {
        return new CommandResponse { SlowText = "Nobody asked you anything." };
    }
}
