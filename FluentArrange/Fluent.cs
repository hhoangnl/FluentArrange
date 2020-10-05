// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentArrange
{
    public static class Fluent
    {
        /// <summary>
        /// Construct a class of type {T} and inject it with mocked dependencies.
        /// </summary>
        /// <typeparam name="T">Type of the concrete class</typeparam>
        /// <param name="createMockType">The method to create mocked dependencies</param>
        /// <returns></returns>
        public static FluentArrangeObject<T> Arrange<T>(Func<Type, object> createMockType)
            where T : class
        {
            var constructor = typeof(T).GetConstructors().Single();

            var listOfDependencies = new Dictionary<Type, object>();

            foreach (var p in constructor.GetParameters())
            {
                var dependency = createMockType(p.ParameterType);
                listOfDependencies.Add(p.ParameterType, dependency);
            }

            return new FluentArrangeObject<T>(listOfDependencies);
        }
    }
}