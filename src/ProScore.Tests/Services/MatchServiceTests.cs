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
            var mockMatches = new Mock<DbSet<Match>>();
            mockMatches.SetupDbSet(new List<Match>());
            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);

            // Act
            var result = _matchService.GetAllMatches();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateMatch_ShouldThrowException_WhenTeamsDoNotExist()
        {
            // Arrange
            var match = new Match { HomeTeamId = 1, AwayTeamId = 2 };

            var mockTeams = new Mock<DbSet<Team>>();
            mockTeams.SetupDbSet(new List<Team>());
            _mockContext.Setup(c => c.Teams).Returns(mockTeams.Object);

            // Act
            var action = () => _matchService.CreateMatch(match);

            // Assert
            action.Should().Throw<Exception>().WithMessage("Um ou ambos os times não foram encontrados.");
        }

        [Fact]
        public void CreateMatch_ShouldAddMatchToDatabase()
        {
            // Arrange
            var match = new Match { HomeTeamId = 1, AwayTeamId = 2 };
            var mockTeams = new Mock<DbSet<Team>>();
            mockTeams.SetupDbSet(new List<Team>
            {
                new Team { Id = 1, Name = "Time A" },
                new Team { Id = 2, Name = "Time B" }
            });

            var mockMatches = new Mock<DbSet<Match>>();
            mockMatches.SetupDbSet(new List<Match>());

            _mockContext.Setup(c => c.Teams).Returns(mockTeams.Object);
            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);

            // Act
            var result = _matchService.CreateMatch(match);

            // Assert
            _mockContext.Verify(c => c.Matches.Add(It.IsAny<Match>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            result.Should().Be(match);
        }
    }
}
