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
        public void For_MultipleCtorsFound_ShouldThrowInvalidOperationException()
        {
            // Act
            Action act = () => Arrange.For<MultipleCtor>(type => new object());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void For_ParameterlessCtor_ShouldNotHaveAnyDependencies()
        {
            // Act
            var result = Arrange.For<AccountRepository>(type => new object());

            // Assert
            result.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public void For_CtorWithParameters_ShouldReturnFluentArrangeObjectWithDependency()
        {
            // Arrange
            var dependency = new AccountRepository();

            // Act
            var result = Arrange.For<AccountService>(type => dependency);

            // Assert
            result.Dependencies.Values.Should().SatisfyRespectively(first => first.Should().Be(dependency));
        }

        [Fact]
        public void Sut_Func_ShouldReturnDependency()
        {
            // Arrange
            var dependency = new AccountRepository();

            // Act
            var result = Arrange.Sut<AccountService>(type => dependency);

            // Assert
            result.AccountRepository.Should().Be(dependency);
        }

        [Fact]
        public void Sut_Func_Action_ShouldReturnDependency()
        {
            // Arrange
            var dependency = new AccountRepository();

            // Act
            var result = Arrange.Sut<AccountService>(type => dependency, action => { });

            // Assert
            result.AccountRepository.Should().Be(dependency);
        }

        [Fact]
        public void Sut_Func_Action_ShouldInvokeAction()
        {
            // Arrange
            var dependency = new AccountRepository();
            var invoked = false;

            // Act
            _ = Arrange.Sut<AccountService>(type => dependency, action => invoked = true);

            // Assert
            invoked.Should().BeTrue();
        }
    }
}
