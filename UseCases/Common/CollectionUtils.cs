using System;
using System.Collections.Generic;
using System.Linq;

namespace UseCases.Common;

public static class CollectionUtils
{
    public static CompareResult<T> Difference<T>(IEnumerable<T> source, IEnumerable<T> target,
        Func<T, T, bool> compareLogic)
    {
        var removed = new List<T>();
        var updated = new List<T>();

        foreach (var sourceItem in source)
        {
            var sameItemFromTarget = target.FirstOrDefault(targetItem => compareLogic(sourceItem, targetItem));
            if (sameItemFromTarget is null)
            {
                removed.Add(sourceItem);
            }
            else
            {
                updated.Add(sameItemFromTarget);
            }
        }

        var added = source.Except(updated);

        return new CompareResult<T>
        {
            Added = added,
            Removed = removed,
            Updated = updated
        };
    }
}