using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Services
{
    public class MatchServiceTests
    {
        private readonly Mock<ProScoreContext> _mockContext;
        private readonly MatchService _matchService;

        public MatchServiceTests()
        {
            _mockContext = new Mock<ProScoreContext>(new DbContextOptions<ProScoreContext>());
            _matchService = new MatchService(_mockContext.Object);
        }

        [Fact]
        public void GetAllMatches_ShouldReturnEmptyList_WhenNoMatchesExist()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<ProScore.Api.Models.Match>>().ReturnsDbSet(new List<ProScore.Api.Models.Match>());
            _mockContext.Setup(c => c.Matches).Returns(mockDbSet.Object);

            // Act
            var result = _matchService.GetAllMatches();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateMatch_ShouldThrowException_WhenTeamsDoNotExist()
        {
            // Arrange
            var match = new ProScore.Api.Models.Match { HomeTeamId = 1, AwayTeamId = 2 };

            _mockContext.Setup(c => c.Teams.Find(1)).Returns((Team?)null);
            _mockContext.Setup(c => c.Teams.Find(2)).Returns((Team?)null);

            // Act
            var action = () => _matchService.CreateMatch(match);

            // Assert
            action.Should().Throw<Exception>().WithMessage("Um ou ambos os times não foram encontrados.");
        }

        [Fact]
        public void CreateMatch_ShouldAddMatchToDatabase()
        {
            // Arrange
            var match = new ProScore.Api.Models.Match { HomeTeamId = 1, AwayTeamId = 2 };
            _mockContext.Setup(c => c.Teams.Find(1)).Returns(new Team { Id = 1, Name = "Time A" });
            _mockContext.Setup(c => c.Teams.Find(2)).Returns(new Team { Id = 2, Name = "Time B" });

            // Act
            var result = _matchService.CreateMatch(match);

            // Assert
            _mockContext.Verify(c => c.Matches.Add(It.IsAny<ProScore.Api.Models.Match>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            result.Should().Be(match);
        }
    }
}
