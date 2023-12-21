using System.Linq.Expressions;
namespace Ndknitor.System;
public static class ArrayExtensions
{
    /// <summary>
    /// Map a property of elements in one collection to another collection of values with the same length. This can be helpful when you need to update or synchronize properties between two collections.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="targetArray"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="sourceArray"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<TSource> MapProperty<TSource, TResult>(
        this IEnumerable<TSource> targetArray,
        Expression<Func<TSource, TResult>> propertyExpression,
        IEnumerable<TResult> sourceArray)
    {
        if (targetArray == null)
        {
            throw new ArgumentNullException(nameof(targetArray));
        }

        if (propertyExpression == null)
        {
            throw new ArgumentNullException(nameof(propertyExpression));
        }

        if (sourceArray == null)
        {
            throw new ArgumentNullException(nameof(sourceArray));
        }

        if (targetArray.Count() != sourceArray.Count())
        {
            throw new ArgumentException("Source and target arrays must have the same length.");
        }

        var memberExpression = (MemberExpression)propertyExpression.Body;
        var propertyName = memberExpression.Member.Name;

        for (int i = 0; i < targetArray.Count(); i++)
        {
            typeof(TSource).GetProperty(propertyName)?.SetValue(targetArray.ElementAt(i), sourceArray.ElementAt(i));
        }
        return targetArray;
    }

    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, sbyte> expression, sbyte offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, byte> expression, byte offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, short> expression, short offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, int> expression, int offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, uint> expression, uint offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, long> expression, long offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, ulong> expression, ulong offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, Int128> expression, Int128 offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
    public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, UInt128> expression, UInt128 offset)
    {
        offset++;
        foreach (var item in array)
        {
            expression(item, offset);
            offset++;
        }
        return array;
    }
}