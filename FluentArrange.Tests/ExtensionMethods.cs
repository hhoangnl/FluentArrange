// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.Tests
{
    public class ExtensionMethods
    {
        [Fact]
        public void With_ActionT_ShouldInvokeAction()
        {
            // Arrange
            Foo foo = new();

            // Act
            foo.With(e => e.Id = 1);

            // Assert
            foo.Id.Should().Be(1);
        }

        [Fact]
        public void With_ActionTaskT_ShouldInvokeAction()
        {
            // Arrange
            Task<Foo> foo = Task.FromResult(new Foo());

            // Act
            foo.With(e => e.Id = 2);

            // Assert
            foo.Result.Id.Should().Be(2);
        }

        [Fact]
        public void With_ShouldAlways_ReturnItself()
        {
            // Arrange
            var foo = new Foo();

            // Act
            var result = foo.With(e => e.Id = 3);

            // Assert
            result.Should().Be(foo);
        }
    }
}
