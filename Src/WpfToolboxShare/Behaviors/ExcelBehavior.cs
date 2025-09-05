using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Xaml.Behaviors;

namespace WpfToolbox.Behaviors;

public class ExcelBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty ExportToExcelFileProperty =
        DependencyProperty.Register("ExportToExcelFile", typeof(string), typeof(ExcelBehavior), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(
          (d, e) => ((ExcelBehavior)d).OnExportToExcelFileChanged((string)e.NewValue))));

    public string ExportToExcelFile
    {
        get => (string)GetValue(ExportToExcelFileProperty);
        set => SetValue(ExportToExcelFileProperty, value);
    }

    public static readonly DependencyProperty ExportHiddenColumnsProperty =
        DependencyProperty.Register("ExportHiddenColumns", typeof(bool), typeof(ExcelBehavior), new FrameworkPropertyMetadata(false));

    public bool ExportHiddenColumns
    {
        get => (bool)GetValue(ExportHiddenColumnsProperty);
        set => SetValue(ExportHiddenColumnsProperty, value);
    }

    public static readonly DependencyProperty SheetNameProperty =
        DependencyProperty.Register("SheetName", typeof(string), typeof(ExcelBehavior), new FrameworkPropertyMetadata("Sheet"));

    public string SheetName
    {
        get => (string)GetValue(SheetNameProperty);
        set => SetValue(SheetNameProperty, value);
    }

    private void OnExportToExcelFileChanged(string filepath)
    {

        using var spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);
        //SheetData sheetData = CreateSheet(spreadsheetDocument, "Server");


        // Replace a WorkbookPart to the document.
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Replace a WorksheetPart to the WorkbookPart.
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new();
        Worksheet worksheet = new(sheetData);
        worksheetPart.Worksheet = worksheet;

        // Replace Sheets to the Workbook.
        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        // Append a new worksheet and associate it with the workbook.
        Sheet sheet = new() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = SheetName };
        sheets.Append(sheet);

        // Title
        uint colIndex = 1;
        foreach (var col in AssociatedObject.Columns)
        {
            string title = col.Header as string ?? "Error";
            InserCell(sheetData, colIndex++, 1, title);
        }

        // Cells
        for (var r = 0; r < AssociatedObject.Items.Count; r++)
        {
            //Debug.WriteLine($"Row {r}");

            var item = AssociatedObject.Items[r];
            colIndex = 1;
            foreach (var col in AssociatedObject.Columns)
            {
                var x1 = col.OnCopyingCellClipboardContent(item);
                InserCell(sheetData, colIndex++, (uint)(r + 2), x1?.ToString() ?? "");
            }
        }

        // AutoFiler


        //char letter = (char)('@' + this.Columns.Count);
        string letter = ConvertNumberToLetters(AssociatedObject.Columns.Count);
        int num = AssociatedObject.Items.Count + 1;
        string reference = $"A1:{letter}{num}";
        var autoFilter = new AutoFilter() { Reference = reference };
        worksheet.Append(autoFilter);

    }

    public static string ConvertNumberToLetters(int number)
    {
        string result = string.Empty;
        while (number > 0)
        {
            number--; // Adjust for 0-based index
            result = (char)('A' + (number % 26)) + result;
            number /= 26;
        }

        return result;
    }


    ////public SheetData CreateSheet(SpreadsheetDocument spreadsheetDocument, string sheetName)
    ////{
    ////    // Replace a WorkbookPart to the document.
    ////    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
    ////    workbookPart.Workbook = new Workbook();

    ////    // Replace a WorksheetPart to the WorkbookPart.
    ////    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
    ////    SheetData sheetData = new SheetData();
    ////    worksheetPart.Worksheet = new Worksheet(sheetData);

    ////    // Replace Sheets to the Workbook.
    ////    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

    ////    // Append a new worksheet and associate it with the workbook.
    ////    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheetName" };
    ////    sheets.Append(sheet);

    ////    return sheetData;
    ////}

    //public static Sheet CreateSheet(SpreadsheetDocument spreadsheetDocument, string sheetName)
    //{
    //    // Replace a WorkbookPart to the document.
    //    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
    //    workbookPart.Workbook = new Workbook();

    //    // Replace a WorksheetPart to the WorkbookPart.
    //    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
    //    var sheetData = new SheetData();
    //    worksheetPart.Worksheet = new Worksheet(sheetData);

    //    // Replace Sheets to the Workbook.
    //    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

    //    // Append a new worksheet and associate it with the workbook.
    //    var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
    //    sheets.Append(sheet);

    //    return sheet;
    //}

    //public static SheetData CreateSheetData(SpreadsheetDocument spreadsheetDocument, string sheetName)
    //{
    //    // Replace a WorkbookPart to the document.
    //    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
    //    workbookPart.Workbook = new Workbook();

    //    // Replace a WorksheetPart to the WorkbookPart.
    //    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
    //    var sheetData = new SheetData();
    //    worksheetPart.Worksheet = new Worksheet(sheetData);

    //    // Replace Sheets to the Workbook.
    //    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

    //    // Append a new worksheet and associate it with the workbook.
    //    var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };
    //    sheets.Append(sheet);

    //    return sheetData;
    //}

    public static void InserCell(SheetData sheetData, uint colIndex, uint rowIndex, string val)
    {
        string columnName = ((char)('@' + (char)(colIndex))).ToString();
        string cellReference = columnName + rowIndex;

        //Debug.WriteLine($"{cellReference}: {val}");

        Row row;
        if (sheetData?.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData!.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        var cell = new Cell
        {
            CellReference = cellReference,
            CellValue = new CellValue(val),
            DataType = new EnumValue<CellValues>(CellValues.String)
        };
        row.Append(cell);
    }
}
