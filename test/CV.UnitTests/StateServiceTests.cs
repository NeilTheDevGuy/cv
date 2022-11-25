using CV.Application.Rooms;
using CV.Application.Services;
using CV.Domain.Enums;
using CV.Infrastructure.Services;
using FluentAssertions;

namespace CV.UnitTests;

public class StateServiceTests
{

    [Fact]
    public async Task GetRoom_ReturnsCorrectRoom()
    {
        var stateService = new StateService();
        await stateService.SetRoom(new Lobby(new TextDownloaderService(new HttpClient()), stateService));

        var receivedRoom = await stateService.GetRoom();
        receivedRoom.Should().BeOfType<Lobby>();
    }

    [Theory]
    [InlineData(State.WaitingForCommand)]
    [InlineData(State.WaitingForAnswer)]
    public async Task GetState_ReturnsCorrectState(Domain.Enums.State expectedState)
    {
        var stateService = new StateService();
        await stateService.SetInputState(expectedState);

        var receivedState = await stateService.GetInputState();
        receivedState.Should().Be(expectedState);
    }
}
