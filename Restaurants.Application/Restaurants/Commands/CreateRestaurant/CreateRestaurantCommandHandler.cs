using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.User;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Command.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger, 
        IMapper mapper, IRestaurantsRepository restaurantsRepository,
        IUserContext userContext) : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            logger.LogInformation("{UserName} [{UserId}] a new restaurant {@Restaurant}", currentUser.Email, currentUser.Id, request);
            var restaurant = mapper.Map<Restaurant>(request.RestaurantDto);
            restaurant.OwnerId = currentUser.Id;
            await restaurantsRepository.Create(restaurant);
            return restaurant.Id;
        }
    }
}
