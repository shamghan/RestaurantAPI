using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.API.Tests.Controllers
{
    public class RestaurantControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;

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
