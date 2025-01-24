using Microsoft.AspNetCore.Mvc;
using Moq;
using ProScore.Api.Controllers;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Controllers
{
    public class TeamControllerTests
    {
        private readonly Mock<ITeamService> _mockTeamService;
        private readonly TeamController _controller;

        public TeamControllerTests()
        {
            _mockTeamService = new Mock<ITeamService>();
            _controller = new TeamController(_mockTeamService.Object);
        }

        [Fact]
        public void GetAllTeams_ShouldReturnOkResult_WhenTeamsExist()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Time A" },
                new Team { Id = 2, Name = "Time B" }
            };
            _mockTeamService.Setup(s => s.GetAllTeams()).Returns(teams);

            // Act
            var result = _controller.GetAllTeams();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedTeams = okResult.Value.Should().BeAssignableTo<IEnumerable<Team>>().Subject;
            returnedTeams.Should().BeEquivalentTo(teams);
        }

        [Fact]
        public void GetTeamById_ShouldReturnNotFound_WhenTeamDoesNotExist()
        {
            // Arrange
            _mockTeamService.Setup(s => s.GetTeamById(It.IsAny<int>())).Returns((Team)null);

            // Act
            var result = _controller.GetTeamById(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void CreateTeam_ShouldReturnCreatedResult_WhenTeamIsValid()
        {
            // Arrange
            var team = new Team { Name = "Novo Time" };
            _mockTeamService.Setup(s => s.CreateTeam(It.IsAny<Team>())).Returns(team);

            // Act
            var result = _controller.CreateTeam(team);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(team);
        }
    }
}