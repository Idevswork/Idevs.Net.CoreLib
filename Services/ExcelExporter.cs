using System.Collections;
using ClosedXML.Excel;
using ClosedXML.Report;
using FastMember;
using Serenity.Data;
using Serenity.Reporting;

namespace Idevs.Services;

public interface IIdevsExcelExporter
{
    byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns, IEnumerable<string>? headers = null);
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string>? headers = null);
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns, IEnumerable<string>? headers = null);
    byte[] Generate(IReadOnlyList<ReportColumn> columns, IList rows, IEnumerable<string>? headers = null,
        string sheetName = "Page1", string tableName = "Table1");
    byte[] ExportReport(string templatePath, params object[] data);
}

public class IdevsExcelExporter : IIdevsExcelExporter
{
    private readonly IServiceProvider _serviceProvider;

    public IdevsExcelExporter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns, IEnumerable<string>? headers = null)
    {
        var report = new TabularDataReport(data, columns);
        return Render(report, headers);
    }

    public byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string>? headers = null)
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, headers);
    }

    public byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns, IEnumerable<string>? headers = null)
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, headers);
    }

    private byte[] Render(IDataOnlyReport report, IEnumerable<string>? headers = null)
    {
        var columns = report.GetColumnList();

        var input = report.GetData();
        var list = (input as IEnumerable) ?? new List<object> { input };
        var data = list.Cast<object?>().ToList();

        return Generate(columns, data, headers);
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
            return format;

        if (format.Contains('f', StringComparison.Ordinal) &&
            Array.IndexOf(DateTimeTypes, dataType) >= 0)
            return format.Replace('f', '0');

        if (!format.StartsWith("n", StringComparison.OrdinalIgnoreCase) ||
            Array.IndexOf(NumberTypes, dataType) < 0) return format;

        if (int.TryParse(format.ToLower().Replace("n", string.Empty), out var n) == false)
        {
            n = 0;
        }

        return n == 0 ? "#,##0" : "#,##0.".PadRight(n + 6, '0');
    }

    public byte[] Generate(IReadOnlyList<ReportColumn> columns, IList rows, IEnumerable<string>? headers = null,
        string sheetName = "Sheet1", string tableName = "Table1")
    {
        Field[]? fields = null;
        TypeAccessor? accessor = null;
        bool[]? invalidProperty = null;

        var colCount = columns.Count;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);
        var startRow = 0;
        if (headers is not null)
        {
            startRow = 1;
            foreach (var header in headers)
            {
                worksheet.Cell(startRow++, 1).Value = header;
            }
        }
        var endRow = rows.Count + startRow + 1;
        startRow++;

        for (var col = 0; col < columns.Count; col++)
        {
            worksheet.Cell(startRow, col + 1).Value = columns[col].Title ?? columns[col].Name;
        }

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
                if (obj is IDictionary || obj is IDictionary<string, object>)
                {
                }
                else if (accessor == null)
                {
                    accessor = TypeAccessor.Create(obj.GetType());
                    invalidProperty = new bool[colCount];
                    for (var c = 0; c < colCount; c++)
                        try
                        {
                            if (accessor[obj, columns[c].Name] != null)
                            {
                            }
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
                            if (objects.TryGetValue(n, out object? v))
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

        // Apply column format if available
        for (var i = 1; i <= colCount; i++)
        {
            var column = columns[i - 1];
            if (!string.IsNullOrEmpty(column.Format))
            {
                worksheet.Column(i).Style.NumberFormat.Format = FixFormatSpecifier(column.Format, column.DataType);
            }
        }

        if (rows.Count > 0)
        {
            worksheet.Cell(startRow + 1, 1).InsertData(dataList);
            var range = worksheet.Range(startRow, 1, endRow, colCount);

            // create the actual table
            var table = range.CreateTable();

            // apply style
            table.Theme = XLTableTheme.TableStyleMedium2;
        }

        // Apply column format if available
        for (var i = 1; i <= colCount; i++)
        {
            var column = columns[i - 1];
            if (!string.IsNullOrEmpty(column.Format))
            {
                worksheet.Column(i).Style.NumberFormat.Format = FixFormatSpecifier(column.Format, column.DataType);
            }
        }

        worksheet.Columns().AdjustToContents();

        var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return ms.ToArray();
    }

    public byte[] ExportReport(string templatePath, params object[] data)
    {
        var template = new XLTemplate(templatePath);
        foreach (var o in data)
        {
            template.AddVariable(o);
        }
        template.Generate();

        var ms = new MemoryStream();
        template.SaveAs(ms);

        return ms.ToArray();
    }
}
