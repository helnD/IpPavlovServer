using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Infrastructure.Abstractions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WebApplication.Services
{
    /// <summary>
    /// NPOI excel reader.
    /// </summary>
    public class NpoiExcelReader : IExcelReader
    {
        private readonly ISheet _sheet;
        private int _currentRow = 0;

        public NpoiExcelReader(string filename, string sheet)
        {
            using var file = File.Open(filename, FileMode.Open);
            var workbook = new XSSFWorkbook(file);
            _sheet = workbook.GetSheetAt(0);
        }

        /// <inheritdoc/>
        public IList<string> NextRow()
        {
            if (_currentRow == _sheet.LastRowNum)
            {
                throw new InvalidOperationException("Current row is last.");
            }

            var row = _sheet.GetRow(_currentRow++);
            return row.Select(cell => cell.CellType switch
            {
                CellType.String => cell.StringCellValue,
                CellType.Numeric => ((decimal)cell.NumericCellValue).ToString(CultureInfo.InvariantCulture),
                _ => cell.ToString()
            }).ToList();
        }

        /// <inheritdoc/>
        public bool IsEnd => _currentRow + 1 == _sheet.LastRowNum;
    }
}