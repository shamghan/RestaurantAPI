using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Command.RemoveRole
{
    public class RemoveUserRoleCommandHandler(
        ILogger<RemoveUserRoleCommandHandler> logger,
        UserManager<Domain.Entities.User> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IRequestHandler<RemoveUserRoleCommand>
    {
        public async Task Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning user roles: {@Request}", request);
            var dbUser = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var dbRole = await roleManager.FindByNameAsync(request.UserRole)
                 ?? throw new NotFoundException(nameof(User), request.UserEmail);

            await userManager.RemoveFromRoleAsync(dbUser, dbRole.Name!);
        }
    }
}
