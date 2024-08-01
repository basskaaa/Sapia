using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomExtensions
{
    public static T RandomElement<T>(this IEnumerable<T> enumerable)
    {
        var elements = enumerable.ToArray();

        if (elements.Any())
        {
            return elements[Random.Range(0, elements.Length)];
        }

        return default;
    }
}