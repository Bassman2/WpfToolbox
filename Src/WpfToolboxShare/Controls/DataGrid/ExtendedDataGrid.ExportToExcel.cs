using DocumentFormat.OpenXml.Spreadsheet;

namespace WpfToolbox.Controls;


// https://learn.microsoft.com/en-us/office/open-xml/spreadsheet/how-to-insert-text-into-a-cell-in-a-spreadsheet?tabs=cs-1%2Ccs-2%2Ccs-3%2Ccs-4%2Ccs


public partial class ExtendedDataGrid : DataGrid
{

    public static readonly DependencyProperty ExportToExcelFileProperty =
        DependencyProperty.Register("ExportToExcelFile", typeof(string), typeof(ExtendedDataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(
                (d, e) => ((ExtendedDataGrid)d).OnExportToExcelFileChanged((string)e.NewValue))));

    public string ExportToExcelFile
    {
        get => (string)GetValue(ExportToExcelFileProperty);
        set => SetValue(ExportToExcelFileProperty, value);
    }

    private void OnExportToExcelFileChanged(string filepath)
    {

        using var spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);
        //SheetData sheetData = CreateSheet(spreadsheetDocument, "Server");


        // Add a WorkbookPart to the document.
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Add a WorksheetPart to the WorkbookPart.
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new();
        Worksheet worksheet = new(sheetData);
        worksheetPart.Worksheet = worksheet;

        // Add Sheets to the Workbook.
        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        // Append a new worksheet and associate it with the workbook.
        Sheet sheet = new () { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Server" };
        sheets.Append(sheet);

        // Title
        uint colIndex = 1;
        foreach (var col in this.Columns)
        {
            string title = col.Header as string ?? "Error";
            InserCell(sheetData, colIndex++, 1, title);
        }

        // Cells
        for (var r = 0; r < this.Items.Count; r++)
        {
            //Debug.WriteLine($"Row {r}");

            var item = this.Items[r];
            colIndex = 1;
            foreach (var col in this.Columns)
            {
                var x1 = col.OnCopyingCellClipboardContent(item);
                InserCell(sheetData, colIndex++, (uint)(r + 2), x1?.ToString() ?? "");
            }
        }

        // AutoFiler


        //char letter = (char)('@' + this.Columns.Count);
        string letter = ConvertNumberToLetters(this.Columns.Count);
        int num = this.Items.Count + 1;
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


    //public SheetData CreateSheet(SpreadsheetDocument spreadsheetDocument, string sheetName)
    //{
    //    // Add a WorkbookPart to the document.
    //    WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
    //    workbookPart.Workbook = new Workbook();

    //    // Add a WorksheetPart to the WorkbookPart.
    //    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
    //    SheetData sheetData = new SheetData();
    //    worksheetPart.Worksheet = new Worksheet(sheetData);

    //    // Add Sheets to the Workbook.
    //    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

    //    // Append a new worksheet and associate it with the workbook.
    //    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheetName" };
    //    sheets.Append(sheet);

    //    return sheetData;
    //}

    public Sheet CreateSheet(SpreadsheetDocument spreadsheetDocument, string sheetName)
    {
        // Add a WorkbookPart to the document.
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Add a WorksheetPart to the WorkbookPart.
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        // Add Sheets to the Workbook.
        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        // Append a new worksheet and associate it with the workbook.
        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheetName" };
        sheets.Append(sheet);

        return sheet;
    }

    public SheetData CreateSheetData(SpreadsheetDocument spreadsheetDocument, string sheetName)
    {
        // Add a WorkbookPart to the document.
        WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        // Add a WorksheetPart to the WorkbookPart.
        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        SheetData sheetData = new SheetData();
        worksheetPart.Worksheet = new Worksheet(sheetData);

        // Add Sheets to the Workbook.
        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        // Append a new worksheet and associate it with the workbook.
        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "sheetName" };
        sheets.Append(sheet);

        return sheetData;
    }

    public void InserCell(SheetData sheetData, uint colIndex, uint rowIndex, string val)
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

        var cell = new Cell() { CellReference = cellReference };

        cell.CellValue = new CellValue(val);
        cell.DataType = new EnumValue<CellValues>(CellValues.String);
        row.Append(cell);
    }

}

