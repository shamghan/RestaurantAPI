using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDishes;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Dtos
{
    public class DishesProfile : Profile
    {
        public DishesProfile() {

            CreateMap<Dish,DishDto>();
            CreateMap<CreateDishCommand,Dish>();
        }
    }
}
