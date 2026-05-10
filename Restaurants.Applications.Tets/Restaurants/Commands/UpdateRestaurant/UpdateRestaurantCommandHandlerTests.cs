using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestuarant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Security.AccessControl;

namespace Restaurants.Applications.Tests.Restaurants.Commands.UpdateRestaurant
{


    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestuarantCommandHandler>> _loggerMock;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
        private readonly Mock<IMapper> _mapperLock;
        private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

        private readonly UpdateRestuarantCommandHandler _handler;
        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestuarantCommandHandler>>();
            _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            _mapperLock = new Mock<IMapper>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
            _handler = new UpdateRestuarantCommandHandler(
                _loggerMock.Object,
                _mapperLock.Object,
                _restaurantsRepositoryMock.Object,
                _restaurantAuthorizationServiceMock.Object);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
        {
            var restaurantId = 1;
            var command = new UpdateRestuarantCommand
            {
                Id = restaurantId,

                Name = "Updated Restaurant",
                Description = "Updated Description",
                HasDelivery = true,

            };
            var restaurant = new Restaurant()
            {
                Id = restaurantId,
                Name = "Old Restaurant",
                Description = "Old Description",
                HasDelivery = false,
            };
            _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId))
                .ReturnsAsync(restaurant);
            _restaurantAuthorizationServiceMock.Setup(x => x.Authorize(restaurant, ResourceOperations.Update))
                .Returns(true);
            //act
            await _handler.Handle(command, CancellationToken.None);

            //assert
            _restaurantsRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
            _mapperLock.Verify(m => m.Map(command, restaurant), Times.Once);
        }
        [Fact]
        public async Task Handle_WithValidRequest_ShouldThrowNoyFoundException()
        {
            var restaurantId = 2;
            var command = new UpdateRestuarantCommand
            {
                Id = restaurantId,
            };
            _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId))
                .ReturnsAsync((Restaurant?)null);

            //act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            //assert
            await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(Restaurant)} with id: {restaurantId} doesn't exist");
        }
        [Fact]
        public async Task Handle_WithValidRequest_ShouldThrowForbiddenAction()
        {
            var restaurantId = 3;
            var command = new UpdateRestuarantCommand
            {
                Id = restaurantId,

            };
            var existingRestaurant = new Restaurant()
            {
                Id = restaurantId,
              
            };
            _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId))
                .ReturnsAsync((existingRestaurant));

          _restaurantAuthorizationServiceMock.Setup(x => x.Authorize(existingRestaurant, ResourceOperations.Update))
                .Returns(false);
            //act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            //assert
            await act.Should().ThrowAsync<ForbidException>();

        }
    }
}
