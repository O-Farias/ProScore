using Microsoft.AspNetCore.Mvc;
using Moq;
using ProScore.Api.Controllers;
using ProScore.Api.Models;
using ProScore.Api.Services;
using Xunit;
using FluentAssertions;

namespace ProScore.Tests.Controllers
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _controller = new EventController(_mockEventService.Object);
        }

        [Fact]
        public void GetEventsByMatch_ShouldReturnOkResult_WhenEventsExist()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, MatchId = 1, Type = "Gol", Minute = 10 },
                new Event { Id = 2, MatchId = 1, Type = "Cartão Amarelo", Minute = 15 }
            };
            _mockEventService.Setup(s => s.GetEventsByMatch(1)).Returns(events);

            // Act
            var result = _controller.GetEventsByMatch(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedEvents = okResult.Value.Should().BeAssignableTo<IEnumerable<Event>>().Subject;
            returnedEvents.Should().BeEquivalentTo(events);
        }

        [Fact]
        public void GetEventsByMatch_ShouldReturnNotFound_WhenNoEventsExist()
        {
            // Arrange
            _mockEventService.Setup(s => s.GetEventsByMatch(1)).Returns(new List<Event>());

            // Act
            var result = _controller.GetEventsByMatch(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void CreateEvent_ShouldReturnCreatedResult_WhenEventIsValid()
        {
            // Arrange
            var gameEvent = new Event
            {
                Id = 1,
                MatchId = 1,
                Type = "Gol",
                PlayerId = 1,
                Minute = 10
            };
            _mockEventService.Setup(s => s.CreateEvent(It.IsAny<Event>())).Returns(gameEvent);

            // Act
            var result = _controller.CreateEvent(gameEvent);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(gameEvent);
        }

        [Fact]
        public void UpdateEvent_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var gameEvent = new Event { Id = 1, Type = "Gol", Minute = 10 };
            _mockEventService.Setup(s => s.UpdateEvent(1, gameEvent)).Returns(true);

            // Act
            var result = _controller.UpdateEvent(1, gameEvent);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            _mockEventService.Setup(s => s.DeleteEvent(It.IsAny<int>())).Returns(false);

            // Act
            var result = _controller.DeleteEvent(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}