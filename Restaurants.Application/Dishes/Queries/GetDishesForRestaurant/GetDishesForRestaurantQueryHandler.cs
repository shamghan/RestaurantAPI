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

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant
{
    public class GetDishesForRestaurantQueryHandler(
        ILogger<GetDishesForRestaurantQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository
        ) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching dishes for rastaurant with id : {RestaurantId}",request.restaurantId);
            var restaurant = await restaurantsRepository.GetById(request.restaurantId) ??
                throw new NotFoundException(nameof(Restaurant),request.restaurantId.ToString());

            var result = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
            return result;
        }
    }
}
