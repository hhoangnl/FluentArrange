// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.Tests
{
    public class ArrangeTests
    {
        [Fact]
        public void Arrange_MultipleCtorsFound_ShouldThrowInvalidOperationException()
        {
            // Act
            Action act = () => Arrange.For<MultipleCtor>(type => new object());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Arrange_ParameterlessCtor_ShouldNotHaveAnyDependencies()
        {
            // Act
            var result = Arrange.For<AccountRepository>(type => new object());

            // Assert
            result.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public void Arrange_CtorWithParameters_ShouldReturnFluentArrangeObjectWithDependency()
        {
            // Arrange
            var dependency = new AccountRepository();

            // Act
            var result = Arrange.For<AccountService>(type => dependency);

            // Assert
            result.Dependencies.Values.Should().SatisfyRespectively(first => first.Should().Be(dependency));
        }
    }
}
