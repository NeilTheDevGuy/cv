using CV.Application.Interfaces;
using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;
using CV.Domain.Rooms.Interfaces;

namespace CV.Application.Rooms;

public class Contact : IRoom
{
    private readonly ITextDownloaderService _textDownloaderService;
    private readonly IStateService _stateService;
    private readonly IRoomsService _roomsService;
    private readonly List<ILookableObject> _objects = new();
    private readonly List<ILookableObject> _doors = new();
    private bool _riddleAnswered = false;
    private bool _givenUp = false;
    private readonly string[] _riddleAnswers = new[] { "7", "seven" };
    private readonly string _giveUp = "GIVE UP";

    public Room Room => Room.Contact;
    public List<ILookableObject> Objects => _objects;

    public List<ILookableObject> Doors => _doors;

    public Contact(ITextDownloaderService textService, IStateService stateService, IRoomsService roomsService)
    {
        _textDownloaderService = textService;
        _stateService = stateService;
        _roomsService = roomsService;
    }

    public async Task InitializeRoom()
    {
        _objects.Add(new GeneralObject(
            "Business Card", 
            await _textDownloaderService.GetText("objects/contact_businesscard_viz.txt"),
            ObjectTextType.StaticPost,
            new List<string> {"Business Card", "Card" }));

        _doors.Add(new Door(
            Room.Lobby.ToString(),
            await _textDownloaderService.GetText("objects/door_profile.txt"),
            ObjectTextType.Slow,
            new List<string> { "Lobby Door", "Lobby" }));     
    }

    public async Task<CommandResponse> Look()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/contact_look.txt") };
    }

    public async Task<CommandResponse> Enter()
    {
        await _stateService.SetRoom(this);
        var response = new CommandResponse();

        if (_givenUp)
        {
            await _stateService.SetInputState(State.WaitingForAnswer);
            response.SlowText = await _textDownloaderService.GetText("rooms/contact_enteraftergivenup.txt");
            return response;
        }

        if (_riddleAnswered)
        {
            response.SlowText = await _textDownloaderService.GetText("rooms/contact_enterafterriddle.txt");
            return response;
        }

        await _stateService.SetInputState(State.WaitingForAnswer);
        response.SlowText = await _textDownloaderService.GetText("rooms/contact_enterbeforeriddle.txt");
        return response;
    }

    public async Task<CommandResponse> Exit()
    {
        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/contact_exit.txt") };
    }

    public async Task<CommandResponse> AnswerRiddle(string answer)
    {
        if (answer.Equals(_giveUp, StringComparison.OrdinalIgnoreCase))
        {
            _givenUp = true;

            await _stateService.SetInputState(State.WaitingForCommand);
            var lobby = await _roomsService.GetRoom(Room.Lobby);

            await _stateService.SetRoom(lobby);

            return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/contact_giveup.txt") };
        }

        if (_riddleAnswers.Contains(answer)) 
        {
            _riddleAnswered = true;
            _givenUp = false;

            await _stateService.SetInputState(State.WaitingForCommand);

            var correctText = await _textDownloaderService.GetText("rooms/contact_correctanswer.txt");  
            
            return new CommandResponse { SlowText = $"{correctText}" };
        }

        return new CommandResponse { SlowText = await _textDownloaderService.GetText("rooms/contact_wronganswer.txt") };
    }
}
