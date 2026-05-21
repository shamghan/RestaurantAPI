using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.User;
using Restaurants.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Applications.Tets.User
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {

            //arrange
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier,"1"),
                new (ClaimTypes.Email,"test@gmail.com"),
                new (ClaimTypes.Role, UserRoless.Admin),
                new (ClaimTypes.Role, UserRoless.User),
                new ("Nationality", "Indian"),
                new ("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });
            

            var userContext = new UserContext(httpContextAccessorMock.Object);
            
            //act
            var currentUser = userContext.GetCurrentUser();

            //asset
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@gmail.com");
            currentUser.Roles.Should().ContainInOrder(UserRoless.Admin, UserRoless.User);
            currentUser.Nationality.Should().Be("Indian");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
        }
        [Fact]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            //arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);
            var userContext = new UserContext(httpContextAccessorMock.Object);
            //act
            Action act = () => userContext.GetCurrentUser();
            //assert
            act.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
        }
    }
}
