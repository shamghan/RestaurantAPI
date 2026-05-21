using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestuarant
{
    public class UpdateRestuarantValidator : AbstractValidator<UpdateRestuarantCommand>
    {

        public UpdateRestuarantValidator() {

            RuleFor(dto => dto.Name)
                .Length(3, 100);

            RuleFor(dto => dto.Description)
                .NotEmpty()
                .WithMessage("Description is required.");
        }
    }
}
