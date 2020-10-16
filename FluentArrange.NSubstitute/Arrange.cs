// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using NSubstitute;

namespace FluentArrange.NSubstitute
{
    public static class Arrange
    {
        public static FluentArrangeContext<T> For<T>()
            where T : class
        {
            return FluentArrange.Arrange.For<T>(CreateMock);
        }

        public static T Sut<T>()
            where T : class
        {
            return FluentArrange.Arrange.Sut<T>(CreateMock);
        }

        public static T Sut<T>(Action<T> arrangeSut)
            where T : class
        {
            return FluentArrange.Arrange.Sut<T>(CreateMock, arrangeSut);
        }

        internal static object CreateMock(Type type)
        {
            return Substitute.For(new[] {type}, new object[] { });
        }
    }
}
