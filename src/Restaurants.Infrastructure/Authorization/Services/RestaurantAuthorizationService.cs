using Microsoft.Extensions.Logging;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperations resourceOperations)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
                user.Email,
                resourceOperations,
                restaurant.Name
                );
            if (resourceOperations == ResourceOperations.Read || resourceOperations == ResourceOperations.Create)
            {
                logger.LogInformation("Create/read operation - successfully authorization");
                return true;
            }
            if (resourceOperations == ResourceOperations.Delete && user.IsInRole(UserRoless.Admin))
            {
                logger.LogInformation("Admin user, delete operation - successfully authorization");
                return true;
            }
            if (resourceOperations == ResourceOperations.Delete || resourceOperations == ResourceOperations.Update && user.Id == restaurant.OwnerId)
            {
                logger.LogInformation("Restaurant Owner - successfully authorization");
                return true;
            }
            return false;
        }

    }
}
