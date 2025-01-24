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
            var players = new List<Player>();
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(players);
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
            var player = new Player { Id = 1, Name = "Test Player" };
            var players = new List<Player> { player };
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(players);
            mockPlayers.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(player);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.GetPlayerById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(player);
        }

        [Fact]
        public void CreatePlayer_ShouldAddPlayerToDatabase()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Flamengo" };
            var player = new Player { Name = "Gabriel Barbosa", TeamId = 1 };

            var mockTeams = new Mock<DbSet<Team>>();
            mockTeams.SetupDbSet(new List<Team> { team });
            mockTeams.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(team);
            _mockContext.Setup(c => c.Teams).Returns(mockTeams.Object);

            var players = new List<Player>();
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(players);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.CreatePlayer(player);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(player);
            _mockContext.Verify(c => c.Players.Add(It.IsAny<Player>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdatePlayer_ShouldUpdatePlayerDetails()
        {
            // Arrange
            var existingPlayer = new Player { Id = 1, Name = "Gabriel Barbosa" };
            var updatedPlayer = new Player { Id = 1, Name = "Gabi Gol", TeamId = 1 };

            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(new List<Player> { existingPlayer });
            mockPlayers.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(existingPlayer);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.UpdatePlayer(1, updatedPlayer);

            // Assert
            result.Should().BeTrue();
            existingPlayer.Name.Should().Be(updatedPlayer.Name);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeletePlayer_ShouldRemovePlayerFromDatabase()
        {
            // Arrange
            var player = new Player { Id = 1, Name = "Gabriel Barbosa", TeamId = 1 };
            var players = new List<Player> { player };
            var mockPlayers = new Mock<DbSet<Player>>();
            mockPlayers.SetupDbSet(players);
            mockPlayers.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(player);
            _mockContext.Setup(c => c.Players).Returns(mockPlayers.Object);

            // Act
            var result = _playerService.DeletePlayer(1);

            // Assert
            result.Should().BeTrue();
            _mockContext.Verify(c => c.Players.Remove(It.Is<Player>(p => p.Id == 1)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}