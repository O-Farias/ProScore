using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;

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
            var mockMatches = new Mock<DbSet<ProScore.Api.Models.Match>>();
            mockMatches.SetupDbSet(new List<ProScore.Api.Models.Match>());
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
            var match = new ProScore.Api.Models.Match { HomeTeamId = 1, AwayTeamId = 2 };
            var mockTeams = new Mock<DbSet<Team>>();
            mockTeams.SetupDbSet(new List<Team>());
            mockTeams.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns((Team)null);
            mockTeams.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 2))).Returns((Team)null);
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
            var homeTeam = new Team { Id = 1, Name = "Time A" };
            var awayTeam = new Team { Id = 2, Name = "Time B" };
            var match = new ProScore.Api.Models.Match { HomeTeamId = 1, AwayTeamId = 2 };

            var mockTeams = new Mock<DbSet<Team>>();
            var teams = new List<Team> { homeTeam, awayTeam };
            mockTeams.SetupDbSet(teams);
            mockTeams.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 1))).Returns(homeTeam);
            mockTeams.Setup(d => d.Find(It.Is<object[]>(o => (int)o[0] == 2))).Returns(awayTeam);

            var mockMatches = new Mock<DbSet<ProScore.Api.Models.Match>>();
            mockMatches.SetupDbSet(new List<ProScore.Api.Models.Match>());

            _mockContext.Setup(c => c.Teams).Returns(mockTeams.Object);
            _mockContext.Setup(c => c.Matches).Returns(mockMatches.Object);

            // Act
            var result = _matchService.CreateMatch(match);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(match);
            _mockContext.Verify(c => c.Matches.Add(It.IsAny<ProScore.Api.Models.Match>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}