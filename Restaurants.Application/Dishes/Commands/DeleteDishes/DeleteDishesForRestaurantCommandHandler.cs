using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteDishesForRestaurantCommandHandler(
        ILogger<DeleteDishesForRestaurantCommandHandler> logger,
        IMapper mapper,
        IDishesRepository dishesRepository,
        IRestaurantsRepository restaurantsRepository
        
        ) : IRequestHandler<DeleteDishesForRestaurantCommand>
    {
     
        public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Remove all dishes from restaurant: {@RestaurantId}", request.RestaurantId);
            var restaurant = await restaurantsRepository.GetById(request.RestaurantId) ??
                throw new NotFoundException(nameof(Restaurants), request.RestaurantId.ToString());

            await dishesRepository.Delete(restaurant.Dishes);
        }
    }
}
