using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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

namespace Restaurants.Application.Restaurants.Commands.UpdateRestuarant
{
    public class UpdateRestuarantCommandHandler(ILogger<UpdateRestuarantCommandHandler> logger,
        IMapper mapper, IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestuarantCommand>
    {
        public async Task Handle(UpdateRestuarantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restuarant with id: {RestaurantId} with {@UpdateRestaurant}", request.Id, request);
            var restuarant = await restaurantsRepository.GetById( request.Id );
            if (restuarant == null)
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(restuarant, ResourceOperations.Update))
                throw new ForbidException();

            mapper.Map(request, restuarant);
            //restuarant.Name = request.Name;
            //restuarant.Description = request.Description;
            //restuarant.HasDelivery = request.HasDelivery;
            await restaurantsRepository.SaveChanges();
        }
    }
}
