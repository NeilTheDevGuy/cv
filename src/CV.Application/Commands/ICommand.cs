using CV.Domain.Commands;
using CV.Domain.Enums;

namespace CV.Application.Commands;

public interface ICommand
{
    public CommandsEnum Handles { get; }
    public Task<CommandResponse> Execute(string input);
}
