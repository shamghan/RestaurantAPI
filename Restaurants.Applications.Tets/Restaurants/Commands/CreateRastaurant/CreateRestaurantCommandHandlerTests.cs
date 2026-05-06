using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.User;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Applications.Tests.Restaurants.Commands.CreateRastaurant
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
        {
            // Arrange

            // 1. Logger
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

            // 2. Mapper
            var mapperMock = new Mock<IMapper>();
            var command = new CreateRestaurantCommand();

            // Set the properties on the DTO
            command.RestaurantDto.Name = "Test Restaurant";
   
            var restaurant = new Restaurant();
            mapperMock
                .Setup(m => m.Map<Restaurant>(It.IsAny<CreateRestaurantDto>()))
                .Returns((CreateRestaurantDto dto) =>
                {
                    // apply mapped properties to the instance used in assertions
                    restaurant.Name = dto.Name;
                    return restaurant;
                });

            // 3. Repository
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Restaurant>()))
                .ReturnsAsync(1); // return new restaurant ID

            // 4. User context
            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser(
                "owner-id",
                "admin@gmail.com",
                new[] { "Admin" },
                null,
                null
            );
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            // 5. Command handler
            var commandHandler = new CreateRestaurantCommandHandler(
                loggerMock.Object,
                mapperMock.Object,
                restaurantRepositoryMock.Object,
                userContextMock.Object
            );

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert

            // a) Correct restaurant ID returned
            result.Should().Be(1);

            // b) OwnerId set correctly
            restaurant.OwnerId.Should().Be("owner-id");

            // c) Repository Create called exactly once
            restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);

            // d) Optional: check that mapped properties were applied
            restaurant.Name.Should().Be("Test Restaurant");

        }
    }
}
