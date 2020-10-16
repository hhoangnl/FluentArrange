// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.NSubstitute.Tests
{
    public class FluentTests
    {
        [Fact]
        public void For_ConcreteType_ShouldBuild()
        {
            // Act
            var result = Arrange.For<AccountRepository>();

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
            var result = Arrange.CreateMock(type);

            // Assert
            result.GetType().FullName.Should().Be("Castle.Proxies.ObjectProxy");
        }

        [Fact]
        public void Sut_Action_ShouldInvokeAction()
        {
            // Arrange
            var invoked = false;

            // Act
            _ = Arrange.Sut<AccountService>(service => invoked = true);

            // Assert
            invoked.Should().BeTrue();
        }
    }
}
