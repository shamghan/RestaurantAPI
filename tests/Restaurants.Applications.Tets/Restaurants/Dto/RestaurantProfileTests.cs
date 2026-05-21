using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestuarant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Applications.Tests.Restaurants.Dto
{
    public class RestaurantsProfileTests
    {
        private IMapper _mapper;
        public RestaurantsProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RetaurantsProfile>();
            });
            _mapper = config.CreateMapper();
        }
        [Fact()]
        public void CreateMap_ForRestaurantDto_MapsCorrectly()
        {

            var restaurant = new Restaurant()
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "A test restaurant",
                Category = "Test Category",
                HasDelivery = true,
                ContactEmail = "test@mail.com",
                ContactNumber = "123456789",
                Address = new Address
                {
                    City = "Test City",
                    Street = "Test Street",
                    PostalCode = "12-345"
                },

            };

            var restaurantDto = _mapper.Map<RestaurantsDto>(restaurant);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
            restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
            restaurantDto.City.Should().Be(restaurant.Address.City);
            restaurantDto.Street.Should().Be(restaurant.Address.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);


        }
        [Fact()]
        public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            var command = new CreateRestaurantCommand()
            {
                RestaurantDto = new CreateRestaurantDto
                {
                    Id = 1,
                    Name = "Test Restaurant",
                    Description = "A test restaurant",
                    Category = "Test Category",
                    HasDelivery = true,
                    ContactEmail = "test@mail.com",
                    ContactNumber = "123456789",
                    City = "Test City",
                    PostalCode = "12-345",
                    Street = "Test Street"
                },
            };

            var restaurant = _mapper.Map<Restaurant>(command.RestaurantDto);
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(command.RestaurantDto.Id);
            restaurant.Name.Should().Be(command.RestaurantDto.Name);
            restaurant.Description.Should().Be(command.RestaurantDto.Description);
            restaurant.Category.Should().Be(command.RestaurantDto.Category);
            restaurant.HasDelivery.Should().Be(command.RestaurantDto.HasDelivery);
            restaurant.Address.City.Should().Be(command.RestaurantDto.City);
            restaurant.Address.Street.Should().Be(command.RestaurantDto.Street);
            restaurant.Address.PostalCode.Should().Be(command.RestaurantDto.PostalCode);

        }
        [Fact()]
        public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            var command = new UpdateRestuarantCommand()
            {
                Id = 1,
                Name = "Test Restaurant",
                Description = "A test restaurant",
                HasDelivery = true,
            };
            var restaurant = _mapper.Map<Restaurant>(command);
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(command.Id);
            restaurant.Name.Should().Be(command.Name);
            restaurant.Description.Should().Be(command.Description);
            restaurant.HasDelivery.Should().Be(command.HasDelivery);
        }
    }
}
