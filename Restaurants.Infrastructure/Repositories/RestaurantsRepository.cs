using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> Create(Restaurant entity)
        {
            await dbContext.Restaurants.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
        public async Task Delete(Restaurant entity)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync(); 
        }
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants
                .Include(r=>r.Dishes)
                .ToListAsync();
            return restaurants;
        }
        public async Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase)
        {
            var seachPhraseLower = searchPhrase?.ToLower();
            var restaurants = await dbContext.Restaurants
                .Where(r => seachPhraseLower == null || ( r.Name.ToLower().Contains(seachPhraseLower)
                         || r.Description.ToLower().Contains(seachPhraseLower)))
                .ToListAsync();
            return restaurants;
        }

        public async Task<Restaurant?> GetById(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(r=>r.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }

        public async Task Update(Restaurant entity)
        =>  await dbContext.SaveChangesAsync();
    }
}
