// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentArrange
{
    public static class Arrange
    {
        /// <summary>
        /// Construct a class of type {T} and inject it with mocked dependencies.
        /// </summary>
        /// <typeparam name="T">Type of the concrete class</typeparam>
        /// <param name="createMockType">The method to create mocked dependencies</param>
        /// <returns>An instance of {T}</returns>
        public static FluentArrangeContext<T> For<T>(Func<Type, object> createMockType)
            where T : class
        {
            var constructor = typeof(T).GetTypeInfo().DeclaredConstructors.Single();

            var listOfDependencies = new Dictionary<Type, object>();

            foreach (var p in constructor.GetParameters())
            {
                var dependency = createMockType(p.ParameterType);
                listOfDependencies.Add(p.ParameterType, dependency);
            }

            return new FluentArrangeContext<T>(listOfDependencies);
        }

        public static T Sut<T>(Func<Type, object> createMockType)
            where T : class
        {
            return Sut<T>(createMockType, null);
        }

        public static T Sut<T>(Func<Type, object> createMockType, Action<T>? arrangeSut)
            where T : class
        {
            var result = For<T>(createMockType).BuildSut();

            arrangeSut?.Invoke(result);

            return result;
        }
    }
}