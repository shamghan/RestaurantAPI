using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantQueryValidator : AbstractValidator<GetAllRestaurantQuery>
    {
        private  int[] allowedPageSize = [5,10,15,30];
        public GetAllRestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .Must(x => allowedPageSize.Contains(x))
                .WithMessage($"Page size must be one of the following: {string.Join(", ", allowedPageSize)}");
        }
    }
}
