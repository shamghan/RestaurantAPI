using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.API.Tests.Controllers
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        private readonly Mock<IRestaurantsSeeder> _restaurantsSeederMock = new();
        private readonly WebApplicationFactory<Program> _factory;

        // Constructor where we can use the injected factory parameter
        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
            // Customize the factory with mocks
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace policy evaluator for testing auth/authorization
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    // Replace RestaurantsRepository with mock
                    services.Replace(ServiceDescriptor.Scoped(
                        typeof(IRestaurantsRepository),
                        _ => _restaurantsRepositoryMock.Object));

                    services.Replace(ServiceDescriptor.Scoped(
                        typeof(IRestaurantsSeeder),
                        _ => _restaurantsSeederMock.Object));
                });
            });
        }


        [Fact()]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // Arrange
            var id = 1123;
            _restaurantsRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync((Restaurant?)null);
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync($"/api/restaurants/{id}");
            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound); // Status Code 200-299
        }
        [Fact()]
        public async Task GetById_ForExistingId_ShouldReturn200oK()
        {
            // Arrange
            var id = 99;
            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Description",
            };



            _restaurantsRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync((restaurant));
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync($"/api/restaurants/{id}");
            var restaurantDto = await result.Content.ReadFromJsonAsync<RestaurantsDto>();
            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be("Test");
            restaurantDto.Description.Should().Be("Description");
        }

        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");
            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); // Status Code 200-299
        }
        [Fact()]
        public async Task GetAll_ForInValidRequest_Returns400BadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var result = await client.GetAsync("/api/restaurants");
            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest); // Status Code 200-299
        }
    }
}
