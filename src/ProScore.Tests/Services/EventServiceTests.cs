using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<ProScoreContext> _mockContext;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockContext = new Mock<ProScoreContext>(new DbContextOptions<ProScoreContext>());
            _eventService = new EventService(_mockContext.Object);
        }

        [Fact]
        public void GetEventsByMatch_ShouldReturnEmptyList_WhenNoEventsExist()
        {
            // Arrange
            var mockEvents = new Mock<DbSet<Event>>();
            mockEvents.SetupDbSet(new List<Event>());
            _mockContext.Setup(c => c.Events).Returns(mockEvents.Object);

            // Act
            var result = _eventService.GetEventsByMatch(1);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateEvent_ShouldThrowException_WhenMatchDoesNotExist()
        {
            // Arrange
            var gameEvent = new Event { MatchId = 1, PlayerId = 1 };
            var mockMatches = new Mock<DbSet<Match>>();
            mockMatches.SetupDbSet(new List<Match>());

            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);

            // Act
            var action = () => _eventService.CreateEvent(gameEvent);

            // Assert
            action.Should().Throw<Exception>().WithMessage("Partida não encontrada.");
        }

        [Fact]
        public void CreateEvent_ShouldAddEventToDatabase()
        {
            // Arrange
            var gameEvent = new Event { MatchId = 1, PlayerId = 1 };
            var mockMatches = new Mock<DbSet<Match>>();
            mockMatches.SetupDbSet(new List<Match> { new Match { Id = 1 } });

            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player> { new Player { Id = 1 } });

            var mockEvents = new Mock<DbSet<Event>>();
            mockEvents.SetupDbSet(new List<Event>());

            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);
            _mockContext.Setup(c => c.Events).Returns(mockEvents.Object);

            // Act
            var result = _eventService.CreateEvent(gameEvent);

            // Assert
            _mockContext.Verify(c => c.Events.Add(It.IsAny<Event>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            result.Should().Be(gameEvent);
        }
    }
}
