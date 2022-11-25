using CV.Application.Commands;
using CV.Domain.Enums;

namespace CV.Application.Factories;

public interface ICommandHandlerFactory
{
    ICommand? GetCommand(CommandsEnum command);
}

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IEnumerable<ICommand> _commands;

    public CommandHandlerFactory(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public ICommand? GetCommand(CommandsEnum command)
    {
        return _commands.FirstOrDefault(c => c.Handles == command);
    }
}
