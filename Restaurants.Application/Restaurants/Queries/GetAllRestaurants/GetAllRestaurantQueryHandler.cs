using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
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
        IMapper mapper, IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantQuery, PagedResult<RestaurantsDto>>
    {
        public async Task<PagedResult<RestaurantsDto>> Handle(GetAllRestaurantQuery request, CancellationToken cancellationToken)
        {
    
            logger.LogInformation("Getting all restaurants");
            var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase,
                request.PageSize, request.PageNumber);
            var restaurantDto = mapper.Map<IEnumerable<RestaurantsDto>>(restaurants);
            var result = new PagedResult<RestaurantsDto>(restaurantDto, totalCount, request.PageSize, request.PageNumber);
            return result; 
        }
    }
}
