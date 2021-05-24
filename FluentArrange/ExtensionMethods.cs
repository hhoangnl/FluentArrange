using System;
using System.Threading.Tasks;

namespace FluentArrange
{
    public static class ExtensionMethods
    {
        [Obsolete("Please use the extension method With instead")]
        public static void Returns<T>(this Task<T> @object, Action<T> configure)
        {
            configure.Invoke(@object.GetAwaiter().GetResult());
        }

        [Obsolete("Please use the extension method With instead")]
        public static void Returns<T>(this T @object, Action<T> configure)
        {
            configure.Invoke(@object);
        }

        public static Task<T> With<T>(this Task<T> @object, Action<T> configure)
        {
            configure.Invoke(@object.GetAwaiter().GetResult());
            return @object;
        }

        public static T With<T>(this T @object, Action<T> configure)
        {
            configure.Invoke(@object);
            return @object;
        }
    }
}
