using Shouldly;
using Xunit;
using Pipeline.Testing.UseCases;
using System.Reflection;
using Pipeline.Testing.Decorators;

namespace Pipeline.Testing.Tests
{
    public class ConventionTest
    {

        [Fact]
        public void AllRequestsNeedAPermissionAttribute()
        {
            foreach(var type in typeof(MyUseCase.MyRequest).GetTypeInfo().Assembly.FindRequestTypes())
            {
                var permissionAttribute = type.GetTypeInfo()
                    .GetCustomAttribute(
                            typeof(RequiresPermissionAttribute));
                            
                permissionAttribute.ShouldNotBeNull();
            }
        }
    }
}
