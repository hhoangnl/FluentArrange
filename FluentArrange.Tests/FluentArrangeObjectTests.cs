// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.Tests
{
    public class FluentArrangeObjectTests
    {
        [Fact]
        public void WithDependency_DependencyTypeNotFound_ShouldNotCallConfigureDependency()
        {
            // Arrange
            var called = false;
            var sut = new FluentArrangeObject<object>(new Dictionary<Type, object>());

            // Act
            sut.WithDependency<IAccountRepository>(e => called = true);

            // Assert
            called.Should().BeFalse();
        }

        [Fact]
        public void WithDependency_DependencyTypeFound_ShouldInvokeConfigureDependency()
        {
            // Arrange
            var called = false;
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };
            var sut = new FluentArrangeObject<AccountService>(dependencies);

            // Act
            sut.WithDependency<IAccountRepository>(e => called = true);

            // Assert
            called.Should().BeTrue();
        }

        [Fact]
        public void WithDependency_ShouldReturnReferenceToSut()
        {
            // Arrange
            var sut = new FluentArrangeObject<AccountService>(new Dictionary<Type, object>());

            // Act
            var result = sut.WithDependency<IAccountRepository>(e => { });

            // Assert
            result.Should().Be(sut);
        }

        [Fact]
        public void BuildSut_DependencyContainsInstance_BuiltSutShouldContainDependencyInstance()
        {
            // Arrange
            var dependency = new AccountRepository();
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), dependency}
            };

            var sut = new FluentArrangeObject<AccountService>(dependencies)
                .WithDependency<IAccountRepository>(e => { });

            // Act
            var result = sut.BuildSut();

            // Assert
            result.AccountRepository.Should().Be(dependency);
        }
    }
}
