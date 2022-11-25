using CV.Application.Commands;
using CV.Application.Factories;
using CV.Application.Interfaces;
using CV.Application.Services;
using CV.Domain.Enums;
using FluentAssertions;
using Moq;
using ICommand = CV.Application.Commands.ICommand;

namespace CV.UnitTests;

public class CommandParserFactoryTests
{
    private readonly Mock<ITextDownloaderService> _textDownloaderServiceMock = new Mock<ITextDownloaderService>();
    private readonly Mock<IStateService> _stateServiceMock = new Mock<IStateService>();
    private readonly Mock<IRoomsService> _roomsServiceMock = new Mock<IRoomsService>();
    private readonly List<ICommand> _commands = new List<ICommand>();

    public CommandParserFactoryTests()
    {        
        _commands.Add(new About(_textDownloaderServiceMock.Object));
        _commands.Add(new Help(_textDownloaderServiceMock.Object));
        _commands.Add(new Look(_stateServiceMock.Object));
        _commands.Add(new Open(_stateServiceMock.Object, _roomsServiceMock.Object));
    }

    [Theory]
    [InlineData(CommandsEnum.About, typeof(About))]
    [InlineData(CommandsEnum.Help, typeof(Help))]
    [InlineData(CommandsEnum.Look, typeof(Look))]
    [InlineData(CommandsEnum.Open, typeof(Open))]
    public void GetCommand_ReturnsCorrectCommand(CommandsEnum commandEnum, Type expectedCommand)
    {
        var factory = new CommandHandlerFactory(_commands);
        var command = factory.GetCommand(commandEnum);

        command.Should().BeOfType(expectedCommand);
    }
}