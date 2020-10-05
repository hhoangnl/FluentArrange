// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FluentArrange
{
    public class FluentArrangeObject<T>
        where T : class
    {
        internal readonly IReadOnlyDictionary<Type, object> Dependencies;

        internal FluentArrangeObject(IDictionary<Type, object> dependencies)
        {
            Dependencies = new ReadOnlyDictionary<Type, object>(dependencies);
        }

        public FluentArrangeObject<T> WithDependency<T2>(Action<T2> configureDependency) where T2 : class
        {
            if (Dependencies.TryGetValue(typeof(T2), out var value) && value is T2 dependency)
            {
                configureDependency.Invoke(dependency);
            }

            return this;
        }

        public T BuildSut()
        {
            return (T) Activator
                .CreateInstance(typeof(T), Dependencies.Select(e => e.Value)
                .ToArray());
        }
    }
}
