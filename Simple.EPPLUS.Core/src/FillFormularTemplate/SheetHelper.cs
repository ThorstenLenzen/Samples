using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Toto.Simple.EPPLUS.Core
{
    internal class SheetHelper
    {
        private readonly ExcelWorksheet _worksheet;

        private SheetHelper(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;         
        }

        public static SheetHelper SelectSheet(ExcelWorksheet sheet)
        {
            return new SheetHelper(sheet);
        }

        public SheetHelper SetStringWithDefaultStyle(string cellPosition, string value)
        {
            var cell = _worksheet.Cells[cellPosition];
            SetDefaultStyle(cell);
            cell.Value = "  " + value;

            return this;
        }

        public SheetHelper SetDateTimeOnTwoLinesCentered(string cellPosition, DateTime value)
        {
            var time = value.Date.ToString("dd.MM.yyyy");
            var date = value.ToString("HH:mm:ss");

            var cell = _worksheet.Cells[cellPosition];
            SetDefaultStyle(cell);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Value = date + "\n" + time;

            return this;
        }

        public SheetHelper SetBoolWithDefaultStyle(string cellPosition, bool value)
        {
            var cell = _worksheet.Cells[cellPosition];
            SetDefaultStyle(cell);
            cell.Value = value ? "  Ja" : "  Nein";

            return this;
        }

        private static void SetDefaultStyle(ExcelRange cell)
        {
            cell.Style.Font.Size = 11;
            cell.Style.Font.Name = "Calibri";
            cell.Style.WrapText = true;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }
    }
}
