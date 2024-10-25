using System;
using System.Collections.Generic;
using System.Linq;

namespace BananaMan.Frameworks;

public static class EnumerableExtensions
{
    public static bool Any<TSource, TParam>(this IEnumerable<TSource> source,
                                            in TParam param,
                                            Func<TSource, TParam, bool> predicate)
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                return true;
            }
        }

        return false;
    }

    public static IEnumerable<TSource?> Where<TSource, TParam>(this IEnumerable<TSource> source,
        TParam param,
        Func<TSource, TParam, bool> predicate) where TSource : struct
    {
        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                yield return element;
            }
        }

        yield return null;
    }

    public static TSource First<TSource, TParam>(this IEnumerable<TSource> source,
                                                 in TParam? param = default,
                                                 Func<TSource, TParam?, bool>? predicate = null)
    {
        if (predicate is null)
        {
            return source.First();
        }

        foreach (var element in source)
        {
            if (predicate(element, param))
            {
                return element;
            }
        }

        throw new InvalidOperationException();
    }
}