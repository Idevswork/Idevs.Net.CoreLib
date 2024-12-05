using System.Collections;
using ClosedXML.Excel;
using FastMember;
using Idevs.Models;
using Serenity.Data;
using Serenity.Reporting;

namespace Idevs;

public interface IIdevsExcelExporter
{
    // Very basic
    byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns);

    // Basic
    byte[] Export(IEnumerable data, Type columnsType);

    // With export columns
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns);

    // With report headers
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<ReportHeader> reportHeaders);

    // With export columns + report headers
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader>? reportHeaders
    );


    byte[] Generate(
        IList rows,
        IReadOnlyList<ReportColumn> columns,
        IEnumerable<ReportHeader>? reportHeaders,
        string sheetName = "Page1"
    );
}

public class IdevsExcelExporter(IServiceProvider serviceProvider) : IIdevsExcelExporter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    // Very basic
    public byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns)
    {
        var report = new TabularDataReport(data, columns);
        return Render(report, null);
    }

    // Basic
    public byte[] Export(IEnumerable data, Type columnsType)
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, null);
    }

    // With export columns
    public byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns)
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, null);
    }

    // With report headers
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, reportHeaders);
    }

    // With export columns + report headers
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader>? reportHeaders
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, reportHeaders);
    }

    private byte[] Render(
        IDataOnlyReport report,
        IEnumerable<ReportHeader>? headers
    )
    {
        var columns = report.GetColumnList();

        var input = report.GetData();
        var list = (input as IEnumerable) ?? new List<object> { input };
        var data = list.Cast<object?>().ToList();

        return Generate(data, columns, headers);
    }

    private static readonly Type[] DateTimeTypes = new[]
    {
        typeof(DateTime),
        typeof(DateTime?),
        typeof(TimeSpan),
        typeof(TimeSpan?)
    };

    private static readonly Type[] NumberTypes = new[]
    {
        typeof(short),
        typeof(short?),
        typeof(int),
        typeof(int?),
        typeof(long),
        typeof(long?),
        typeof(float),
        typeof(float?),
        typeof(decimal),
        typeof(decimal?)
    };

    private static string FixFormatSpecifier(string format, Type dataType)
    {
        if (string.IsNullOrEmpty(format))
        {
            return format;
        }

        if (
            format.Contains('f', StringComparison.Ordinal)
            && Array.IndexOf(DateTimeTypes, dataType) >= 0
        )
        {
            return format.Replace('f', '0');
        }

        if (
            !format.StartsWith("n", StringComparison.OrdinalIgnoreCase)
            || Array.IndexOf(NumberTypes, dataType) < 0
        )
        {
            return format;
        }

        if (
            format.Contains('%', StringComparison.OrdinalIgnoreCase)
            && Array.IndexOf(NumberTypes, dataType) >= 0
        )
        {
            return format.Replace(" ", "");
        }

        if (int.TryParse(format.ToLower().Replace("n", string.Empty), out var n) == false)
        {
            n = 0;
        }

        return n == 0 ? "#,##0" : "#,##0.".PadRight(n + 6, '0');
    }

    public byte[] Generate(
        IList rows,
        IReadOnlyList<ReportColumn> columns,
        IEnumerable<ReportHeader>? reportHeaders,
        string sheetName = "Sheet1"
    )
    {
        Field[]? fields = null;
        TypeAccessor? accessor = null;
        bool[]? invalidProperty = null;

        var colCount = columns.Count;

        using var workbook = new XLWorkbook();
        workbook.Style.Font.FontName = "Carlito";
        var worksheet = workbook.Worksheets.Add(sheetName);
        var startRow = reportHeaders is not null
            ? reportHeaders.Count(x => !string.IsNullOrEmpty(x.HeaderLine)) + 1
            : 0;
        startRow++;

        CreateTableHeader(worksheet, columns, startRow);

        var dataList = new List<object[]>();
        foreach (var obj in rows)
        {
            var data = new object[colCount];
            var row = obj as IRow;
            if (row != null)
            {
                if (fields == null)
                {
                    fields = new Field[colCount];
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var n = columns[i].Name;
                        fields[i] = row.FindFieldByPropertyName(n) ?? row.FindField(n);
                    }
                }
            }
            else if (obj != null)
            {
                if (obj is IDictionary or IDictionary<string, object>) { }
                else if (accessor == null)
                {
                    accessor = TypeAccessor.Create(obj.GetType());
                    invalidProperty = new bool[colCount];
                    for (var c = 0; c < colCount; c++)
                        try
                        {
                            if (accessor[obj, columns[c].Name] != null) { }
                        }
                        catch
                        {
                            invalidProperty[c] = true;
                        }
                }
            }

            for (var c = 0; c < colCount; c++)
            {
                if (row != null)
                {
                    var field = fields?[c];
                    if (field is not null)
                        data[c] = field.AsObject(row);
                }
                else
                {
                    string n;
                    switch (obj)
                    {
                        case IDictionary<string, object> objects:
                            n = columns[c].Name;
                            if (objects.TryGetValue(n, out var v))
                                data[c] = v;
                            break;

                        case IDictionary dict:
                            n = columns[c].Name;
                            if (dict.Contains(n))
                                data[c] = dict[n] ?? string.Empty;
                            break;

                        default:
                            if (obj != null)
                            {
                                if (invalidProperty is not null && !invalidProperty[c])
                                {
                                    if (accessor is not null)
                                        data[c] = accessor[obj, columns[c].Name];
                                }
                            }

                            break;
                    }
                }
            }

            dataList.Add(data);
        }

        if (rows.Count > 0)
        {
            CreateTable(worksheet, dataList, columns, startRow);
        }

        // Apply column format if available
        for (var i = 1; i <= colCount; i++)
        {
            var column = columns[i - 1];
            if (!string.IsNullOrEmpty(column.Format))
            {
                worksheet.Column(i).Style.NumberFormat.Format = FixFormatSpecifier(
                    column.Format,
                    column.DataType
                );
            }
        }

        worksheet.Columns().AdjustToContents();

        if (reportHeaders is not null)
        {
            startRow = 1;
            foreach (var header in reportHeaders)
            {
                if (string.IsNullOrEmpty(header.HeaderLine))
                {
                    continue;
                }
                worksheet.Cell(startRow++, 1).Value = header.HeaderLine;
            }
        }

        var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return ms.ToArray();
    }

    private static void CreateTableHeader(
        IXLWorksheet worksheet,
        IReadOnlyList<ReportColumn> columns,
        int startRow
    )
    {
        for (var col = 0; col < columns.Count; col++)
        {
            worksheet.Cell(startRow, col + 1).Value = columns[col].Title ?? columns[col].Name;
        }
    }

    private static void CreateTable(
        IXLWorksheet worksheet,
        ICollection dataList,
        IReadOnlyCollection<ReportColumn> columns,
        int startRow
    )
    {
        var endRow = startRow + dataList.Count;
        worksheet.Cell(startRow + 1, 1).InsertData(dataList);
        var range = worksheet.Range(startRow, 1, endRow, columns.Count);

        // create the actual table
        var table = range.CreateTable();

        // apply style
        var theme = TableTheme.TableStyleMedium2.ToString();
        table.Theme = XLTableTheme.FromName(theme);
    }
}