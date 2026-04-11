using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.CreateDishes
{
    public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
       IRestaurantsRepository restaurantsRepository, IDishesRepository dishesRespository,
       IMapper mapper) : IRequestHandler<CreateDishCommand, int>
    {
        public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new restaurant {@Dishes}", request);
            var restaurant = await restaurantsRepository.GetById(request.RestaurantId) ??
                throw new NotFoundException(nameof(Restaurants), request.RestaurantId.ToString());
            var dish = mapper.Map<Dish>(request);
            return await dishesRespository.Create(dish);
        }
    }
}
