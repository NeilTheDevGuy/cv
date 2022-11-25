using CV.Application.Interfaces;
using CV.Domain.Commands;
using CV.Domain.Enums;

namespace CV.Application.Commands;

public class Help : ICommand
{
    private readonly ITextDownloaderService _textDownloaderService;

    public CommandsEnum Handles => CommandsEnum.Help;

    public Help(ITextDownloaderService textService)
    {
        _textDownloaderService = textService;
    }

    public async Task<CommandResponse> Execute(string input)
    {
        return new CommandResponse
        {
            SlowText = await _textDownloaderService.GetText("help.txt")
        };
    }
}
