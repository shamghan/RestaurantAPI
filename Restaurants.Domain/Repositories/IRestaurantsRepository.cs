using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<int> Create(Restaurant entity);
        Task Delete(Restaurant entity);
        Task<IEnumerable<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetById(int id);
        Task Update(Restaurant entity);
    }
}
