// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FluentArrange.Tests.TestClasses;
using FluentAssertions;
using Xunit;

namespace FluentArrange.Tests
{
    public class FluentArrangeContextTests
    {
        [Fact]
        public void WithDependency_TypeNotFound_ShouldNotInvokeAction()
        {
            // Arrange
            var invoked = false;
            var sut = new FluentArrangeContext<object>(new Dictionary<Type, object>());

            // Act
            sut.WithDependency<IAccountRepository>(e => invoked = true);

            // Assert
            invoked.Should().BeFalse();
        }

        [Fact]
        public void WithDependency_TypeFound_ShouldInvokeAction()
        {
            // Arrange
            var invoked = false;
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };
            var sut = new FluentArrangeContext<AccountService>(dependencies);

            // Act
            sut.WithDependency<IAccountRepository>(e => invoked = true);

            // Assert
            invoked.Should().BeTrue();
        }

        [Fact]
        public void WithDependency_Type_ShouldReturnReferenceToSut()
        {
            // Arrange
            var sut = new FluentArrangeContext<AccountService>(new Dictionary<Type, object>());

            // Act
            var result = sut.WithDependency<IAccountRepository>(e => { });

            // Assert
            result.Should().Be(sut);
        }

        [Fact]
        public void WithDependency_T_InstanceTypeFound_DependenciesShouldContainInstance()
        {
            // Arrange
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };

            var sut = new FluentArrangeContext<AccountService>(dependencies);

            var newInstance = new AccountRepository();

            // Act
            sut.WithDependency<IAccountRepository>(newInstance);

            // Assert
            sut.Dependencies[typeof(IAccountRepository)].Should().Be(newInstance);
        }

        [Fact]
        public void WithDependency_T_InstanceTypeNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };

            var sut = new FluentArrangeContext<AccountService>(dependencies);

            // Act
            Action act = () => sut.WithDependency<IFoo>(new Foo());

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("No dependency found of type FluentArrange.Tests.TestClasses.IFoo");
        }

        [Fact]
        public void WithDependency_T2_T3_InstanceTypeFound_ShouldInvokeAction()
        {
            // Arrange
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };

            var sut = new FluentArrangeContext<AccountService>(dependencies);

            var invoked = false;

            // Act
            sut.WithDependency<IAccountRepository, AccountRepository>(new AccountRepository(), repository => invoked = true);

            // Assert
            invoked.Should().BeTrue();
        }

        [Fact]
        public void WithDependency_T2_T3_InstanceTypeFound_DependenciesShouldContainInstance()
        {
            // Arrange
            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };

            var sut = new FluentArrangeContext<AccountService>(dependencies);

            var instance = new AccountRepository();

            // Act
            sut.WithDependency<IAccountRepository, AccountRepository>(instance, repository => { });

            // Assert
            sut.Dependencies[typeof(IAccountRepository)].Should().Be(instance);
        }

        [Fact]
        public void Dependency_T2_DependencyTypeNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var sut = new FluentArrangeContext<AccountRepository>(new Dictionary<Type, object>());

            // Act
            Action act = () => sut.Dependency<IFoo>();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("No dependency found of type FluentArrange.Tests.TestClasses.IFoo");
        }

        [Fact]
        public void Dependency_T2_T3_DifferentTypeExpected_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var expectedErrorMessage = $"The found dependency is of type '{typeof(AccountRepository)}' but type '{typeof(YetAnotherAccountRepository)}' was expected";

            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), new AccountRepository()}
            };

            var sut = new FluentArrangeContext<AccountRepository>(dependencies);

            // Act
            Action act = () => sut.Dependency<IAccountRepository, YetAnotherAccountRepository>();

            // Assert
            act.Should().Throw<InvalidOperationException>().And.Message.Should().Be(expectedErrorMessage);
        }

        [Fact]
        public void Dependency_T2_T3_TypeFound_ShouldReturnT3()
        {
            // Arrange
            var dependency = new AccountRepository();

            var dependencies = new Dictionary<Type, object>
            {
                {typeof(IAccountRepository), dependency}
            };

            var sut = new FluentArrangeContext<AccountRepository>(dependencies);

            // Act
            var result = sut.Dependency<IAccountRepository, AccountRepository>();

            // Assert
            result.Should().Be(dependency);
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

            var sut = new FluentArrangeContext<AccountService>(dependencies)
                .WithDependency<IAccountRepository>(e => { });

            // Act
            var result = sut.BuildSut();

            // Assert
            result.AccountRepository.Should().Be(dependency);
        }
    }
}