using System;
using NSubstitute;

namespace FluentArrange.NSubstitute
{
    public static class Fluent
    {
        public static FluentArrangeObject<T> Arrange<T>()
            where T : class
        {
            return FluentArrange.Fluent.Arrange<T>(CreateMock);
        }

        internal static object CreateMock(Type type)
        {
            return Substitute.For(new[] {type}, new object[] { });
        }
    }
}
