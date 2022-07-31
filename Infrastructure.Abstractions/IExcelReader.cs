using System.Collections.Generic;

namespace Infrastructure.Abstractions;

/// <summary>
/// Interface for reading excel file.
/// </summary>
public interface IExcelReader
{
    /// <summary>
    /// Get next row.
    /// </summary>
    /// <returns>List with cells values.</returns>
    IList<string> NextRow();

    /// <summary>
    /// Current row is last.
    /// </summary>
    bool IsEnd { get; }
}