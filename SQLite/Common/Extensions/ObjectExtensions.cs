using System;

namespace GUISandbox.DataBase.Common.Extensions;

public static class ObjectExtensions
{
    public static void ThrowArgumentExceptionIfNull<T>(this T obj, string parameter)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(parameter);
        }
    }

    public static void ThrowArgumentExceptionIfNullOrEmptyOrWhiteSpace(this string str, string parameter)
    {
        if (str is null)
        {
            throw new ArgumentNullException(parameter);
        }

        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentException(parameter);
        }

        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException(parameter);
        }
    }
}
