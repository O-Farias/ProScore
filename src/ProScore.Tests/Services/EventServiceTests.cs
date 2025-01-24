using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;

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
            var events = new List<Event>();
            var mockEvents = new Mock<DbSet<Event>>();
            mockEvents.SetupDbSet(events);
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
            var mockMatches = new Mock<DbSet<ProScore.Api.Models.Match>>();
            var matches = new List<ProScore.Api.Models.Match>();
            mockMatches.SetupDbSet(matches);
            mockMatches.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns((ProScore.Api.Models.Match)null);
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
            var match = new ProScore.Api.Models.Match { Id = 1 };
            var player = new Player { Id = 1 };
            var gameEvent = new Event { MatchId = 1, PlayerId = 1 };

            // Setup match mock
            var mockMatches = new Mock<DbSet<ProScore.Api.Models.Match>>();
            var matches = new List<ProScore.Api.Models.Match> { match };
            mockMatches.SetupDbSet(matches);
            mockMatches.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(match);
            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);

            // Setup player mock
            var mockPlayers = new Mock<DbSet<Player>>();
            var players = new List<Player> { player };
            mockPlayers.SetupDbSet(players);
            mockPlayers.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(player);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Setup events mock
            var mockEvents = new Mock<DbSet<Event>>();
            var events = new List<Event>();
            mockEvents.SetupDbSet(events);
            _mockContext.Setup(c => c.Events).Returns(mockEvents.Object);

            // Act
            var result = _eventService.CreateEvent(gameEvent);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(gameEvent);
            _mockContext.Verify(c => c.Events.Add(It.IsAny<Event>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}