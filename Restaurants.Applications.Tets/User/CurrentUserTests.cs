
using Restaurants.Domain.Constants;
using Restaurants.Application.User;
using FluentAssertions;
namespace Restaurants.Applications.Tets.User
{
    public class CurrrentUserTests
    {
        //TestMethod_Scenario_ExpectResult
        //[Fact()]
        [Theory()]
        [InlineData(UserRoless.Admin)]
        [InlineData(UserRoless.User)]
        public void IsInRoleTest_WithMatchingRole_ShouldReturnTrue(string roleName)
        {
            //arrange - set up the current user object with some roles that in act
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoless.Admin, UserRoless.User], null, null);

            //act - get the value from the method
            var isInRole = currentUser.IsInRole(roleName);
            
            //asert - whether it matches the expected result
            isInRole.Should().BeTrue();
        }
        [Fact()]
        public void IsInRoleTest_WithMatchingRole_ShouldReturnFalse()
        {
            //arrange - set up the current user object with some roles that in act
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoless.Owner, UserRoless.User], null, null);

            //act - get the value from the method
            var isInRole = currentUser.IsInRole(UserRoless.Admin);

            //asert - whether it matches the expected result
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRoleTest_WithMatchingRoleCase_ShouldReturnFalse()
        {
            //arrange - set up the current user object with some roles that in act
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoless.Owner, UserRoless.User], null, null);

            //act - get the value from the method
            var isInRole = currentUser.IsInRole(UserRoless.Admin.ToLower());

            //asert - whether it matches the expected result
            isInRole.Should().BeFalse();
        }
    }
}