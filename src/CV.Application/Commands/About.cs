using CV.Application.Interfaces;
using CV.Domain.Commands;
using CV.Domain.Enums;

namespace CV.Application.Commands;

public class About : ICommand
{
    private readonly ITextDownloaderService _textDownloaderService;

    public CommandsEnum Handles => CommandsEnum.About;

    public About(ITextDownloaderService textService)
    {
        _textDownloaderService = textService;
    }

    public async Task<CommandResponse> Execute(string input)
    {
        return new CommandResponse
        {
            SlowText = await _textDownloaderService.GetText("about.txt"),
            PostWriteStaticText = @"https://github.com/NeilTheDevGuy/cv"
        };
    }
}
