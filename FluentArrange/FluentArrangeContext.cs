// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentArrange
{
    public class FluentArrangeContext<T>
        where T : class
    {
        internal readonly Dictionary<Type, object> Dependencies;

        internal FluentArrangeContext(Dictionary<Type, object> dependencies)
        {
            Dependencies = new Dictionary<Type, object>(dependencies);
        }

        private T? _sut;

        public T Sut => _sut ??= BuildSut();

        public FluentArrangeContext<T> WithDependency<T2>(Action<T2> configureDependency)
            where T2 : class
        {
            if (Dependencies.TryGetValue(typeof(T2), out var value) && value is T2 dependency)
            {
                configureDependency.Invoke(dependency);
            }

            return this;
        }

        public FluentArrangeContext<T> WithDependency<T2>(Action<FluentArrangeContext<T>, T2> configureDependency)
	        where T2 : class
        {
	        return WithDependency<T2>(dependency =>
	        {
		        configureDependency.Invoke(this, dependency);
	        });
        }

        public FluentArrangeContext<T> WithDependency<T2>(T2 instance)
            where T2 : class
        {
            _ = Dependency<T2>();

            Dependencies[typeof(T2)] = instance;

            return this;
        }

        public FluentArrangeContext<T> WithDependency<T2>(T2 instance, Action<T2> configureInstance)
            where T2 : class
        {
            _ = Dependency<T2>();

            Dependencies[typeof(T2)] = instance;
            configureInstance(instance);

            return this;
        }

        public FluentArrangeContext<T> WithDependency<T2, T3>(T3 instance, Action<T3> configureInstance)
            where T2 : class
            where T3 : T2
        {
            _ = Dependency<T2>();

            Dependencies[typeof(T2)] = instance;
            configureInstance(instance);

            return this;
        }

        public FluentArrangeContext<T> WithDependency<T2, T3>(T3 instance, Action<FluentArrangeContext<T>, T3> configureInstance)
	        where T2 : class
	        where T3 : T2
        {
	        _ = Dependency<T2>();

	        Dependencies[typeof(T2)] = instance;
	        configureInstance(this, instance);

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

        public T3 Dependency<T2, T3>()
            where T2 : class
            where T3 : T2
        {
            var dependency = Dependency<T2>();
            if (dependency.GetType() == typeof(T3))
            {
                return (T3) dependency;
            }

            throw new InvalidOperationException($"The found dependency is of type '{dependency.GetType()}' but type '{typeof(T3)}' was expected");
        }

        public T BuildSut()
        {
            return (T) Activator
                .CreateInstance(typeof(T), Dependencies.Select(e => e.Value)
                .ToArray());
        }
    }
}
