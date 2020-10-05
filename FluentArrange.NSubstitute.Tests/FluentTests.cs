using System;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.NSubstitute.Tests
{
    public class FluentTests
    {
        [Fact]
        public void Arrange_ConcreteType_ShouldBuild()
        {
            // Act
            var result = Fluent.Arrange<AccountRepository>();

            // Assert
            Action verify = () => result.BuildSut();
            verify.Should().NotThrow();
        }

        [Fact]
        public void CreateMock_InterfaceType_ShouldReturnObjectOfTypeObjectProxy()
        {
            // Arrange
            var type = typeof(IAccountRepository);

            // Act
            var result = Fluent.CreateMock(type);

            // Assert
            result.GetType().FullName.Should().Be("Castle.Proxies.ObjectProxy");
        }
    }
}
