using Microsoft.AspNetCore.Mvc;
using Moq;
using ProScore.Api.Controllers;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Controllers
{
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _mockPlayerService;
        private readonly PlayerController _controller;

        public PlayerControllerTests()
        {
            _mockPlayerService = new Mock<IPlayerService>();
            _controller = new PlayerController(_mockPlayerService.Object);
        }

        [Fact]
        public void GetAllPlayers_ShouldReturnOkResult_WhenPlayersExist()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { Id = 1, Name = "Jogador A", Number = 10 },
                new Player { Id = 2, Name = "Jogador B", Number = 9 }
            };
            _mockPlayerService.Setup(s => s.GetAllPlayers()).Returns(players);

            // Act
            var result = _controller.GetAllPlayers();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedPlayers = okResult.Value.Should().BeAssignableTo<IEnumerable<Player>>().Subject;
            returnedPlayers.Should().BeEquivalentTo(players);
        }

        [Fact]
        public void GetPlayerById_ShouldReturnNotFound_WhenPlayerDoesNotExist()
        {
            // Arrange
            _mockPlayerService.Setup(s => s.GetPlayerById(It.IsAny<int>())).Returns((Player)null);

            // Act
            var result = _controller.GetPlayerById(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void CreatePlayer_ShouldReturnCreatedResult_WhenPlayerIsValid()
        {
            // Arrange
            var player = new Player { Name = "Novo Jogador", Number = 7 };
            _mockPlayerService.Setup(s => s.CreatePlayer(It.IsAny<Player>())).Returns(player);

            // Act
            var result = _controller.CreatePlayer(player);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(player);
        }

        [Fact]
        public void CreatePlayer_ShouldReturnBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var player = new Player { Name = "Novo Jogador", Number = 7 };
            _mockPlayerService
                .Setup(s => s.CreatePlayer(It.IsAny<Player>()))
                .Throws(new Exception("Time não encontrado para associar o jogador."));

            // Act
            var result = _controller.CreatePlayer(player);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Time não encontrado para associar o jogador.");
        }
    }
}