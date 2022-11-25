using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Rooms.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CV.Application.Commands;

public class Open : ICommand
{
    private readonly IStateService _stateService;
    private readonly IRoomsService _roomsService;
    private const string _couldNotFindDoor = "Could not find a door like that.";
    private const string _doorNotRegistered = "Although there is a door, it leads nowhere.";

    public CommandsEnum Handles => CommandsEnum.Open;

    public Open(IStateService stateService, IRoomsService roomsService)
    {
        _stateService = stateService;
        _roomsService = roomsService;
    }

    public async Task<CommandResponse> Execute(string input)
    {
        var room = await _stateService.GetRoom();

        var components = input.Split(" ");
                
        CommandResponse? roomResponse = new CommandResponse();
        CommandResponse? enterResponse = new CommandResponse();
        CommandResponse? exitResponse = new CommandResponse();

        foreach (var word in components)
        {            
            var matchingDoor = room.Doors.FirstOrDefault(r => r.LookableWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));

            if (matchingDoor != null)
            {
                (roomResponse, enterResponse, exitResponse) = await TryEnterNewRoom(matchingDoor.Title);
            }
            else
            {
                exitResponse = new CommandResponse { SlowText = _couldNotFindDoor };
            }
        }

        //The open command returns as follows. Probably scope for improvement here.
        var response = new CommandResponse
        {
            PreWriteStaticText = exitResponse?.PreWriteStaticText,
            PostWriteStaticText = enterResponse?.PostWriteStaticText,
            SlowText = $"{exitResponse?.SlowText}{Environment.NewLine}{enterResponse?.SlowText}"
        };

        return response;
    }

    private async Task<(CommandResponse?, CommandResponse?, CommandResponse?)> TryEnterNewRoom(string roomName)
    {
        var newRoom = await GetNewRoom(roomName);
        var currentRoom = await _stateService.GetRoom();

        if (newRoom == null)
        {
            return (new CommandResponse { SlowText = _doorNotRegistered }, null, null);
        }

        var exitCommand = await currentRoom.Exit();
        var enterCommand = await newRoom.Enter();

        return (null, enterCommand, exitCommand);
    }

    private async Task<IRoom?> GetNewRoom(string room)
    {        
        Enum.TryParse(room, true, out Room roomEnum);

        if (room != null)
        {
            return await _roomsService.GetRoom(roomEnum);
        }

        return default;
    }
}
