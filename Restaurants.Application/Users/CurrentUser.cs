using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.User
{
    public record CurrentUser (string Id, string Email, IEnumerable<string> Roles)
    {
        public bool IsRole(string role)=> Roles.Contains(role);
    }
}
