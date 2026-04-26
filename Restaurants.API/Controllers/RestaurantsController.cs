using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Command.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestuarant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;
using System.Reflection;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        //k[Authorize(Policy = PolicyName.CreatedAtLeast2Restaurants)]
        public async Task<ActionResult<IEnumerable<RestaurantsDto>>> GetAll([FromQuery] GetAllRestaurantQuery query)
        {
            var restaurant = await mediator.Send(query);
            return Ok(restaurant);
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = PolicyName.HasNationality)]
        public async Task<ActionResult<RestaurantsDto>>  GetById([FromRoute]int id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            return Ok(restaurant);
        }
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateById([FromRoute] int id, UpdateRestuarantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles=UserRoless.Owner)]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            var command = new CreateRestaurantCommand
            {
                RestaurantDto = createRestaurantDto
            };
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id}, null);

        }
    }
}
