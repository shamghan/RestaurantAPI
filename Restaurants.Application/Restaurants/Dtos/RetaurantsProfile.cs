using AutoMapper;
using Restaurants.Application.Restaurants.Commands.UpdateRestuarant;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class RetaurantsProfile : Profile
    {
        public RetaurantsProfile()
        {
            CreateMap<UpdateRestuarantCommand, Restaurant>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(d=>d.Address, opt=>opt.MapFrom(
                    src=> new Address
                    {
                        City= src.City,
                        PostalCode= src.PostalCode,
                        Street=src.Street
                    }));


            CreateMap<Restaurant, RestaurantsDto>()
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(d => d.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
                .ForMember(d => d.PostalCode, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
                .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes));

        }
    }
}
