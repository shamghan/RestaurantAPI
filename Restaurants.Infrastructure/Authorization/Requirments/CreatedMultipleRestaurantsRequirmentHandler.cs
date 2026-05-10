using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.User;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Requirments
{
    internal class CreatedMultipleRestaurantsRequirmentHandler(IRestaurantsRepository restaurantsRepository,
        IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirment>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirment requirement)
        {
            var currentUser = userContext.GetCurrentUser();
           var restaurants = await restaurantsRepository.GetAllAsync();
            var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);
            if(userRestaurantsCreated>= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
