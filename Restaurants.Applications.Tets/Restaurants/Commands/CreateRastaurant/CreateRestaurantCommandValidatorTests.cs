using FluentValidation;
using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Applications.Tets.Restaurants.Commands.CreateRastaurant
{
    public class CreateRestaurantCommandValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            //arrange
            var command = new CreateRestaurantCommand()
            {
                RestaurantDto = new CreateRestaurantDto
                {
                    Name = "Test Restaurant",
                    Description="Test Description",
                    Category ="Italian",
                    ContactEmail = "test@gmail.com",
                    PostalCode="56-554",
                }
            };
            var validator = new CreateRestaurantDtoValidator();
            //act

            var result = validator.TestValidate(command.RestaurantDto);
            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Fact()]
        public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
        {
            //arrange
            var command = new CreateRestaurantCommand()
            {
                RestaurantDto = new CreateRestaurantDto
                {
                    Name = "Te",
                    Description = "Test Description",
                    Category = "Ita",
                    ContactEmail = "@gmail.com",
                    PostalCode = "56554",
                }
            };
            var validator = new CreateRestaurantDtoValidator();
            //act

            var result = validator.TestValidate(command.RestaurantDto);
            //assert
            result.ShouldHaveValidationErrorFor(x=>x.Name);
            result.ShouldHaveValidationErrorFor(x=>x.Category);
            result.ShouldHaveValidationErrorFor(x=>x.ContactEmail);
            result.ShouldHaveValidationErrorFor(x=>x.PostalCode);
        }
        [Theory()]
        [InlineData("Italian")]
        [InlineData("American")]
        [InlineData("Indian")]
        [InlineData("Japan")]
        public void Validator_ForCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
        {
            //arrange
            
            var validator = new CreateRestaurantDtoValidator();
            var command = new CreateRestaurantCommand()
            {
                RestaurantDto = new CreateRestaurantDto
                {
                    Category = category,
                }
            };
            //act

            var result = validator.TestValidate(command.RestaurantDto);
            //assert
            
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
            
        }
        [Theory()]
        [InlineData("1252")]
        [InlineData("10-54454")]
        [InlineData("52-488")]
        [InlineData("52-5 78")]
        public void Validator_ForPostalCode_ShouldNotHaveValidationErrorsForPostalCodeProperty(string postalCode)
        {
            //arrange

            var validator = new CreateRestaurantDtoValidator();
            var command = new CreateRestaurantCommand()
            {
                RestaurantDto = new CreateRestaurantDto
                {
                    PostalCode = postalCode,
                }
            };
            //act

            var result = validator.TestValidate(command.RestaurantDto);
            //assert

            result.ShouldNotHaveValidationErrorFor(x => x.PostalCode);

        }
    }
}
