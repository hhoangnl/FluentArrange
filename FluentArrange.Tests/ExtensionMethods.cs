using System.Threading.Tasks;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.Tests
{
    public class ExtensionMethods
    {
        [Fact]
        public void Returns_ActionT_ShouldInvokeAction()
        {
            // Arrange
            Foo foo = new();

            // Act
            foo.Returns(e => e.Id = 1);

            // Assert
            foo.Id.Should().Be(1);
        }

        [Fact]
        public void Returns_ActionTaskT_ShouldInvokeAction()
        {
            // Arrange
            Task<Foo> foo = Task.FromResult(new Foo());

            // Act
            foo.Returns(e => e.Id = 2);

            // Assert
            foo.Result.Id.Should().Be(2);
        }
    }
}
