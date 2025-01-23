using Moq;
using ProScore.Api.Data;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ProScore.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ProScoreContext> _mockContext;
        private readonly TeamService _teamService;

        public TeamServiceTests()
        {
            // Configura um mock do DbContext
            _mockContext = new Mock<ProScoreContext>(new DbContextOptions<ProScoreContext>());
            _teamService = new TeamService(_mockContext.Object);
        }

        [Fact]
        public void GetAllTeams_ShouldReturnEmptyList_WhenNoTeamsExist()
        {
            // Arrange
            _mockContext.Setup(c => c.Teams).ReturnsDbSet(new List<Team>());

            // Act
            var result = _teamService.GetAllTeams();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateTeam_ShouldAddTeamToDatabase()
        {
            // Arrange
            var newTeam = new Team { Name = "Flamengo", Country = "Brasil", Founded = new DateTime(1895, 11, 17) };

            // Act
            var result = _teamService.CreateTeam(newTeam);

            // Assert
            _mockContext.Verify(c => c.Teams.Add(It.IsAny<Team>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            result.Should().Be(newTeam);
        }
    }
}
