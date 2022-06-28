using System.Collections.Generic;

namespace UseCases.Common;

public class CompareResult<T>
{
    public IEnumerable<T> Added { get; init; }

    public IEnumerable<T> Removed { get; init; }

    public IEnumerable<T> Updated { get; init; }
}