using CV.Application.Services;
using CV.Domain.Commands;
using CV.Domain.Enums;
using CV.Domain.Objects;
using System.Linq;

namespace CV.Application.Commands;

public class Look : ICommand
{
    private readonly IStateService _stateService;
    private const string _noObject = "Could not find such a thing to look at.";

    public CommandsEnum Handles => CommandsEnum.Look;

    public Look(IStateService stateService)
    {
        _stateService = stateService;
    }

    public async Task<CommandResponse> Execute(string input)
    {
        var room = await _stateService.GetRoom();
        var components = input.Split(" ");

        if (components.Length == 1)
        {
            return await room.Look();
        }

        //Could be a Door or an Object
        var lookableObjects = room.Objects.Union(room.Doors).ToList();

        foreach (var word in components)
        {
            var matchingObject = lookableObjects.FirstOrDefault(r => r.LookableWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (matchingObject != null)
            {
                return GetObjectLookResponse(matchingObject);
            }
        }

        return new CommandResponse
        {
            SlowText = _noObject
        };
    }

    private CommandResponse GetObjectLookResponse(ILookableObject matchingObject)
    {
        //Maybe Objects should have individual text values rather than determining the type
        var slowText = matchingObject.TextType == ObjectTextType.Slow ? matchingObject.Description : null;
        var preStaticText = matchingObject.TextType == ObjectTextType.StaticPre ? matchingObject.Description : null;
        var postStaticText = matchingObject.TextType == ObjectTextType.StaticPost ? matchingObject.Description : null;
        return new CommandResponse
        {
            SlowText = slowText,
            PreWriteStaticText = preStaticText,
            PostWriteStaticText = postStaticText
        };
    }
}
