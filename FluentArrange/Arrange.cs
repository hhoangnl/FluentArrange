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
        /// Construct a class of type {T} (using the first public constructor) and inject it with mocked dependencies.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createMockType"></param>
        /// <returns></returns>
        public static FluentArrangeContext<T> For<T>(Func<Type, object> createMockType)
            where T : class
        {
            return For<T>(createMockType, constructors => constructors.FirstOrDefault(c => c.IsPublic));
        }

        /// <summary>
        /// Construct a class of type {T} and inject it with mocked dependencies.
        /// </summary>
        /// <typeparam name="T">Type of the concrete class</typeparam>
        /// <param name="createMockType">The method to create mocked dependencies</param>
        /// <param name="constructorSelector">Pick a specific constructor for instantiation</param>
        /// <returns>An instance of {T}</returns>
        public static FluentArrangeContext<T> For<T>(Func<Type, object> createMockType, Func<IEnumerable<ConstructorInfo>, ConstructorInfo?> constructorSelector)
            where T : class
        {
            var constructor = constructorSelector.Invoke(typeof(T).GetTypeInfo().DeclaredConstructors);
            if (constructor == null)
            {
                throw new InvalidOperationException("No matching constructor found.");
            }
            
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