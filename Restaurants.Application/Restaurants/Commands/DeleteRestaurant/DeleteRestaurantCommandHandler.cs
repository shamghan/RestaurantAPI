using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper, IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant with id: {RestaurantId}",request.Id);
            var restaurant = await restaurantsRepository.GetById(request.Id);
            if (restaurant == null)
                throw new NotFoundException(nameof(Restaurant),request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperations.Delete))
                throw new ForbidException(); 

            await restaurantsRepository.Delete(restaurant);
            
        }
    }
}
