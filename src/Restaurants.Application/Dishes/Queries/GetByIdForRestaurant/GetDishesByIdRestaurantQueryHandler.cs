using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Queries.GetByIdForRestaurant
{
    public class GetDishesByIdRestaurantQueryHandler(
        ILogger<GetDishesByIdRestaurantQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<GetDishesByIdRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDishesByIdRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching dishes:{DishId}, for rastaurant with id : {RestaurantId} ", request.RestaurantId, request.DishId);
            var restaurant = await restaurantsRepository.GetById(request.RestaurantId) ??
                  throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == request.DishId) ??
                  throw new NotFoundException(nameof(Dish), request.DishId.ToString());
            var results = mapper.Map<DishDto>(dish);
            return results;
        }
    }
}
