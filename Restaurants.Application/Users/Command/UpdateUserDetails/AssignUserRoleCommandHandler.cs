using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Command.UpdateUserDetails
{
    public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
        UserManager<Domain.Entities.User> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IRequestHandler<AssignUserRoleCommand>
    {
        public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Assigning user roles: {@Request}", request);
            var dbUser = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

            var dbRole = await roleManager.FindByNameAsync(request.UserRole)
                 ?? throw new NotFoundException(nameof(User), request.UserEmail);

            await userManager.AddToRoleAsync(dbUser, dbRole.Name!);
            //By writing dbRole.Name!, you’re telling the compiler:
            //I guarantee dbRole.Name is not null here, so don’t warn me.
        }
    }
}
