using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<ProScoreContext> _mockContext;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockContext = new Mock<ProScoreContext>(new DbContextOptions<ProScoreContext>());
            _playerService = new PlayerService(_mockContext.Object);
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnEmptyList_WhenNoPlayersExist()
        {
            // Arrange
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player>());
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.GetAllPlayers();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetPlayerById_ShouldReturnPlayer_WhenPlayerExists()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Gabriel Barbosa", TeamId = 1 };
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player> { player });
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.GetPlayerById(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Gabriel Barbosa");
        }

        [Fact]
        public void CreatePlayer_ShouldAddPlayerToDatabase()
        {
            // Arrange
            var player = new Player { Name = "Gabriel Barbosa", TeamId = 1 };
            var mockTeams = new Mock<DbSet<Team>>();
            mockTeams.SetupDbSet(new List<Team> { new Team { Id = 1, Name = "Flamengo" } });
            _mockContext.Setup(c => c.Teams).Returns(mockTeams.Object);

            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player>());
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.CreatePlayer(player);

            // Assert
            _mockContext.Verify(c => c.Players.Add(It.IsAny<Player>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            result.Should().Be(player);
        }

        [Fact]
        public void UpdatePlayer_ShouldUpdatePlayerDetails()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Gabriel Barbosa", TeamId = 1 };
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player> { player });
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            var updatedPlayer = new Player { Name = "Gabi Gol", TeamId = 2 };

            // Act
            var result = _playerService.UpdatePlayer(1, updatedPlayer);

            // Assert
            result.Should().BeTrue();
            player.Name.Should().Be("Gabi Gol");
            player.TeamId.Should().Be(2);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeletePlayer_ShouldRemovePlayerFromDatabase()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Gabriel Barbosa", TeamId = 1 };
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player> { player });
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.DeletePlayer(1);

            // Assert
            result.Should().BeTrue();
            _mockContext.Verify(c => c.Players.Remove(player), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
