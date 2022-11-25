using CV.Application.Interfaces;
using CV.Application.Rooms;
using CV.Application.Services;
using FluentAssertions;
using Moq;

namespace CV.UnitTests;

public class RoomServiceTests
{
    private readonly Mock<ITextDownloaderService> _textDownloaderServiceMock = new Mock<ITextDownloaderService>();
    private readonly Mock<IStateService> _stateServiceMock = new Mock<IStateService>();
    private readonly Mock<IRoomsService> _roomServiceMock = new Mock<IRoomsService>();

[Theory]
    [InlineData(Domain.Enums.Room.Lobby)]
    [InlineData(Domain.Enums.Room.Contact)]
    [InlineData(Domain.Enums.Room.WorkHistory)]
    [InlineData(Domain.Enums.Room.Profile)]
    public async Task CanRegisterAndRetrieveRoom(Domain.Enums.Room room)
    {
        var roomsService = new RoomsService();
        await roomsService.RegisterRoom(new Lobby(_textDownloaderServiceMock.Object, _stateServiceMock.Object));
        await roomsService.RegisterRoom(new Contact(_textDownloaderServiceMock.Object, _stateServiceMock.Object, _roomServiceMock.Object));
        await roomsService.RegisterRoom(new WorkHistory(_textDownloaderServiceMock.Object, _stateServiceMock.Object));
        await roomsService.RegisterRoom(new Profile(_textDownloaderServiceMock.Object, _stateServiceMock.Object));

        var retrievedRoom = await roomsService.GetRoom(room);
        retrievedRoom.Room.Should().Be(room);
    }
}
