using CV.Application.Factories;
using CV.Domain.Commands;
using CV.Domain.Enums;

namespace CV.Application.Services;

public interface ICommandParserService
{
    public Task<CommandResponse> GetCommandResponse(string input);
}

public class CommandParserService : ICommandParserService
{
    private readonly IStateService _stateService;
    private readonly ICommandHandlerFactory _commandHandlerFactory;

    public CommandParserService(IStateService stateService, ICommandHandlerFactory commandHandlerFactory)
    {
        _stateService = stateService;
        _commandHandlerFactory = commandHandlerFactory;
    }

    public async Task<CommandResponse> GetCommandResponse(string input)
    {
        if (await _stateService.GetInputState() == State.WaitingForAnswer)
        {
            var room = await _stateService.GetRoom();
            return await room.AnswerRiddle(input);
        }

        var components = input.Split(" ");

        var commandExists = Enum.TryParse(components[0], true, out CommandsEnum commandEnum);

        if (!commandExists)
        {
            return new CommandResponse { SlowText = "Unknown Command. Try HELP" };
        }

        var command = _commandHandlerFactory.GetCommand(commandEnum);

        if (command == null)
        {
            return new CommandResponse { SlowText = "Hmm. I know that command, but don't know what to do with it." };
        }

        return await command.Execute(input);
    }
}
