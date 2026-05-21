using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Command.CreateRestaurant
{
    public class CreateRestaurantCommand :IRequest<int>
    {
        public CreateRestaurantDto RestaurantDto { get; set; }
        public CreateRestaurantCommand()
        {
            RestaurantDto = new CreateRestaurantDto();
        }
    }
}
