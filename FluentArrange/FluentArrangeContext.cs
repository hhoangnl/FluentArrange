// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FluentArrange
{
    public class FluentArrangeContext<T>
        where T : class
    {
        internal readonly IReadOnlyDictionary<Type, object> Dependencies;

        internal FluentArrangeContext(IDictionary<Type, object> dependencies)
        {
            Dependencies = new ReadOnlyDictionary<Type, object>(dependencies);
        }

        private T? _sut;

        public T Sut => _sut ??= BuildSut();

        public FluentArrangeContext<T> WithDependency<T2>(Action<T2> configureDependency) where T2 : class
        {
            if (Dependencies.TryGetValue(typeof(T2), out var value) && value is T2 dependency)
            {
                configureDependency.Invoke(dependency);
            }

            return this;
        }

        public T2 Dependency<T2>() where T2 : class
        {
            if (Dependencies.TryGetValue(typeof(T2), out var value) && value is T2 dependency)
            {
                return dependency;
            }

            throw new InvalidOperationException($"No dependency found of type {typeof(T2)}");
        }

        public T BuildSut()
        {
            return (T) Activator
                .CreateInstance(typeof(T), Dependencies.Select(e => e.Value)
                .ToArray());
        }
    }
}
