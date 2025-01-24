using Microsoft.AspNetCore.Mvc;
using Moq;
using ProScore.Api.Controllers;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;
using FluentAssertions;
using Match = ProScore.Api.Models.Match;

namespace ProScore.Tests.Controllers
{
    public class MatchControllerTests
    {
        private readonly Mock<IMatchService> _mockMatchService;
        private readonly MatchController _controller;

        public MatchControllerTests()
        {
            _mockMatchService = new Mock<IMatchService>();
            _controller = new MatchController(_mockMatchService.Object);
        }

        [Fact]
        public void GetAllMatches_ShouldReturnOkResult_WhenMatchesExist()
        {
            // Arrange
            var matches = new List<Match>
            {
                new Match { Id = 1, Location = "Estádio A" },
                new Match { Id = 2, Location = "Estádio B" }
            };
            _mockMatchService.Setup(s => s.GetAllMatches()).Returns(matches);

            // Act
            var result = _controller.GetAllMatches();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedMatches = okResult.Value.Should().BeAssignableTo<IEnumerable<Match>>().Subject;
            returnedMatches.Should().BeEquivalentTo(matches);
        }

        [Fact]
        public void GetMatchById_ShouldReturnNotFound_WhenMatchDoesNotExist()
        {
            // Arrange
            _mockMatchService.Setup(s => s.GetMatchById(It.IsAny<int>())).Returns((Match?)null);

            // Act
            var result = _controller.GetMatchById(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void CreateMatch_ShouldReturnCreatedResult_WhenMatchIsValid()
        {
            // Arrange
            var match = new Match
            {
                Id = 1,
                Location = "Estádio A",
                HomeTeamId = 1,
                AwayTeamId = 2,
                Date = DateTime.Now
            };
            _mockMatchService.Setup(s => s.CreateMatch(It.IsAny<Match>())).Returns(match);

            // Act
            var result = _controller.CreateMatch(match);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(match);
        }

        [Fact]
        public void UpdateMatch_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var match = new Match { Id = 1, Location = "Estádio A" };
            _mockMatchService.Setup(s => s.UpdateMatch(1, match)).Returns(true);

            // Act
            var result = _controller.UpdateMatch(1, match);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteMatch_ShouldReturnNotFound_WhenMatchDoesNotExist()
        {
            // Arrange
            _mockMatchService.Setup(s => s.DeleteMatch(It.IsAny<int>())).Returns(false);

            // Act
            var result = _controller.DeleteMatch(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}