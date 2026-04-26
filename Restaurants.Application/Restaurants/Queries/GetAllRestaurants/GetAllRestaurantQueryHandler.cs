using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantQueryHandler(ILogger<GetAllRestaurantQueryHandler> logger,
        IMapper mapper, IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantQuery, IEnumerable<RestaurantsDto>>
    {
        public async Task<IEnumerable<RestaurantsDto>> Handle(GetAllRestaurantQuery request, CancellationToken cancellationToken)
        {
    
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase);
            var restaurantDto = mapper.Map<IEnumerable<RestaurantsDto>>(restaurants);
            return restaurantDto; 
        }
    }
}
