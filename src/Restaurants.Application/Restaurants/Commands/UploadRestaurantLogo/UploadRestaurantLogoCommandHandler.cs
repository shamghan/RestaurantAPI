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

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo
{
    public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
    {
        public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restuarant with id: {RestaurantId} with {@UpdateRestaurant}", request.RestaurantId, request);
            var restuarant = await restaurantsRepository.GetById(request.RestaurantId);
            if (restuarant == null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorize(restuarant, ResourceOperations.Update))
                throw new ForbidException();

             var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
            restuarant.LogoUrl = logoUrl;
            await restaurantsRepository.SaveChanges();
        }
    }
}
