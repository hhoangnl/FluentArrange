using System;
using System.Threading.Tasks;

namespace FluentArrange
{
    public static class ExtensionMethods
    {
        public static void Returns<T>(this Task<T> @object, Action<T> configure)
        {
            configure.Invoke(@object.GetAwaiter().GetResult());
        }

        public static void Returns<T>(this T @object, Action<T> configure)
        {
            configure.Invoke(@object);
        }
    }
}
