using CV.Application.Interfaces;
using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Rooms;

public class WorkHistory : IRoom
{
    private readonly ITextDownloaderService _textDownloaderService;
    private readonly IStateService _stateService;
    private readonly List<ILookableObject> _objects = new List<ILookableObject>();
    private readonly List<ILookableObject> _doors = new List<ILookableObject>();

    public Room Room => Room.WorkHistory;
    public List<ILookableObject> Objects => _objects;
    public List<ILookableObject> Doors => _doors;

    public WorkHistory(ITextDownloaderService textService, IStateService stateService)
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
            "Global Relay", 
            await _textDownloaderService.GetText("objects/experience_globalrelay.txt"),
            ObjectTextType.Slow,
            new List<string> { "Global Relay", "Global", "Relay" }));

        _objects.Add(new GeneralObject(
            "Plum Guide", 
            await _textDownloaderService.GetText("objects/experience_plumguide.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Plum Guide", "Plum" }));

        _objects.Add(new GeneralObject(
            "Travel Republic", 
            await _textDownloaderService.GetText("objects/experience_travelrepublic.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Travel Republic", "TR", "Travel" }));

        _objects.Add(new GeneralObject(
            "Vivid Travel",
            await _textDownloaderService.GetText("objects/experience_vividtravel.txt"),
            ObjectTextType.Slow,
            new List<string> { "Vivid Travel", "Vivid " }));

        _objects.Add(new GeneralObject(
            "Cybit", 
            await _textDownloaderService.GetText("objects/experience_cybit.txt"), 
            ObjectTextType.Slow,
            new List<string> { "Cybit", "Thales", "Thales Telematics", "Masternaut" }));
    }

    public async Task<CommandResponse> Look()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/experience_look.txt") };
    }

    public async Task<CommandResponse> Enter()
    {
        await _stateService.SetRoom(this);
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/experience_enter.txt") };
    }

    public async Task<CommandResponse> Exit()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/experience_exit.txt") };
    }

    public async Task<CommandResponse> AnswerRiddle(string answer)
    {
        return new CommandResponse { SlowText = "Nobody asked you anything." };
    }
}
