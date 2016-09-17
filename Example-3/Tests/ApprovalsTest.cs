using Shouldly;
using Xunit;
using Pipeline.Testing.UseCases;
using System;
using System.Reflection;
using System.Text;
using Pipeline.Testing.Decorators;

namespace Pipeline.Testing.Tests
{
    public class ApprovalsTest
    {

        [Fact]
        public void AllRequestsHaveTheCorrectPermissionOnThem()
        {
            Console.WriteLine("Looking for permission attributes");
            var actual = new StringBuilder();
            foreach(var type in typeof(MyUseCase.MyRequest).GetTypeInfo().Assembly.FindRequestTypes())
            {
                var permissionAttribute = (RequiresPermissionAttribute)type.GetTypeInfo().GetCustomAttribute(typeof(RequiresPermissionAttribute));
                if (permissionAttribute != null) 
                {
                    actual.AppendLine(type.Name + " -> " + permissionAttribute.Permission);
                }
            }
            actual.ToString().Trim().ShouldBe(Approved);
        }

        private const string Approved = @"MyOtherRequest -> MyOtherPermission
MyRequest -> MyRequestPermission";
    }
}
