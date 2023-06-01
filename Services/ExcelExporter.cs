using System.Collections;
using ClosedXML.Excel;
using ClosedXML.Report;
using FastMember;
using Idevs.Models;
using Serenity.Data;
using Serenity.Reporting;

namespace Idevs.Services;

public interface IIdevsExcelExporter
{
    // Very basic
    byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns);
    byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns, TableTheme tableTheme);

    // Basic
    byte[] Export(IEnumerable data, Type columnsType);
    byte[] Export(IEnumerable data, Type columnsType, TableTheme tableTheme);

    // With export columns
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns);
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        TableTheme tableTheme
    );

    // With report headers
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<ReportHeader> reportHeaders);
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        TableTheme tableTheme
    );

    // With aggregate columns
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<AggregateColumn> aggregateColumns
    );
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    );

    // With export columns + report headers
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders
    );
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        TableTheme tableTheme
    );

    // With export column + aggregate columns
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<AggregateColumn> aggregateColumns
    );
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    );

    // With report headers + aggregate columns
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns
    );
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    );

    // With export columns + report headers + aggregate columns
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns
    );
    byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    );

    byte[] Generate(
        IList rows,
        IReadOnlyList<ReportColumn> columns,
        IEnumerable<ReportHeader>? reportHeaders,
        IEnumerable<AggregateColumn>? aggregateColumns,
        TableTheme? tableTheme,
        string sheetName = "Page1"
    );
    byte[] ExportReport(string templatePath, params object[] data);
}

public class IdevsExcelExporter : IIdevsExcelExporter
{
    private readonly IServiceProvider _serviceProvider;

    public IdevsExcelExporter(IServiceProvider serviceProvider)
    {
        _serviceProvider =
            serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    // Very basic
    public byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns)
    {
        var report = new TabularDataReport(data, columns);
        return Render(report, null, null, null);
    }

    public byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns, TableTheme tableTheme)
    {
        var report = new TabularDataReport(data, columns);
        return Render(report, null, null, tableTheme);
    }

    // Basic
    public byte[] Export(IEnumerable data, Type columnsType)
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, null, null, null);
    }

    public byte[] Export(IEnumerable data, Type columnsType, TableTheme tableTheme)
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, null, null, tableTheme);
    }

    // With export columns
    public byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns)
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, null, null, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, null, null, tableTheme);
    }

    // With report headers
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, reportHeaders, null, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, reportHeaders, null, tableTheme);
    }

    // With aggregate columns
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<AggregateColumn> aggregateColumns
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, null, aggregateColumns, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, null, aggregateColumns, tableTheme);
    }

    // With export columns + report headers
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, reportHeaders, null, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, reportHeaders, null, tableTheme);
    }

    // With export column + aggregate columns
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<AggregateColumn> aggregateColumns
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, null, aggregateColumns, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, null, aggregateColumns, tableTheme);
    }

    // With report headers + aggregate columns
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, reportHeaders, aggregateColumns, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, _serviceProvider);
        return Render(report, reportHeaders, aggregateColumns, tableTheme);
    }

    // With export columns + report headers + aggregate columns
    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, reportHeaders, aggregateColumns, null);
    }

    public byte[] Export(
        IEnumerable data,
        Type columnsType,
        IEnumerable<string> exportColumns,
        IEnumerable<ReportHeader> reportHeaders,
        IEnumerable<AggregateColumn> aggregateColumns,
        TableTheme tableTheme
    )
    {
        var report = new TabularDataReport(data, columnsType, exportColumns, _serviceProvider);
        return Render(report, reportHeaders, aggregateColumns, tableTheme);
    }

    private byte[] Render(
        IDataOnlyReport report,
        IEnumerable<ReportHeader>? headers,
        IEnumerable<AggregateColumn>? aggregateColumns,
        TableTheme? tableTheme
    )
    {
        var columns = report.GetColumnList();

        var input = report.GetData();
        var list = (input as IEnumerable) ?? new List<object> { input };
        var data = list.Cast<object?>().ToList();

        tableTheme = tableTheme ?? TableTheme.TableStyleMedium2;

        return Generate(data, columns, headers, aggregateColumns, tableTheme);
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

        if (
            format.Contains('f', StringComparison.Ordinal)
            && Array.IndexOf(DateTimeTypes, dataType) >= 0
        )
            return format.Replace('f', '0');

        if (
            !format.StartsWith("n", StringComparison.OrdinalIgnoreCase)
            || Array.IndexOf(NumberTypes, dataType) < 0
        )
            return format;

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
        IEnumerable<AggregateColumn>? aggregateColumns,
        TableTheme? tableTheme,
        string sheetName = "Sheet1"
    )
    {
        Field[]? fields = null;
        TypeAccessor? accessor = null;
        bool[]? invalidProperty = null;

        var colCount = columns.Count;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);
        var startRow = reportHeaders is not null ? reportHeaders.Count() + 1 : 0;
        startRow++;

        CreateTableHeader(worksheet, columns, startRow);

        var groupColumn = aggregateColumns
            ?.FirstOrDefault(x => x.AggregateType == AggregateType.GROUP)
            .ColumnName;
        var startGroup = startRow;
        var endGroup = startRow;
        var groupData = string.Empty;
        var dataList = new List<object[]>();
        foreach (var obj in rows)
        {
            var columnData = string.Empty;
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
                if (obj is IDictionary || obj is IDictionary<string, object>) { }
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

                if (columns[c].Name == groupColumn)
                {
                    columnData = data[c].ToString();
                }
            }

            if (!string.IsNullOrEmpty(groupColumn))
            {
                if (columnData != groupData)
                {
                    if (string.IsNullOrEmpty(groupData))
                    {
                        startGroup = startRow;
                    }
                    else
                    {
                        CreateTable(
                            worksheet,
                            dataList,
                            columns,
                            startGroup,
                            aggregateColumns,
                            tableTheme
                        );

                        endGroup = startGroup + dataList.Count;
                        startGroup = endGroup + 3;

                        // Clear dataList
                        dataList = new List<object[]>();

                        CreateTableHeader(worksheet, columns, startGroup);
                    }
                    groupData = columnData;
                }
            }

            dataList.Add(data);
        }

        if (rows.Count > 0)
        {
            if (startGroup > 0)
            {
                startRow = startGroup;
            }

            CreateTable(worksheet, dataList, columns, startRow, aggregateColumns, tableTheme);
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
                worksheet.Cell(startRow++, 1).Value = header.HeaderLine;
            }
        }

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

    private void CreateTableHeader(
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

    private void CreateTable(
        IXLWorksheet worksheet,
        List<object[]> dataList,
        IReadOnlyList<ReportColumn> columns,
        int startRow,
        IEnumerable<AggregateColumn>? aggregateColumns,
        TableTheme? normalTheme
    )
    {
        var endRow = startRow + dataList.Count;
        worksheet.Cell(startRow + 1, 1).InsertData(dataList);
        var range = worksheet.Range(startRow, 1, endRow, columns.Count);

        // create the actual table
        var table = range.CreateTable();
        if (aggregateColumns is not null)
        {
            table.ShowTotalsRow = true;
        }

        // apply style
        table.Theme = aggregateColumns is null
            ? XLTableTheme.FromName(normalTheme.ToString())
            : XLTableTheme.FromName(normalTheme.ToString());

        // Add aggregated columns
        if (aggregateColumns is not null)
        {
            var cols = columns.Select(x => x.Name).ToList();
            var titles = columns.Select(x => x.Title).ToList();
            foreach (var column in aggregateColumns)
            {
                var colIdx = cols.IndexOf(column.ColumnName);
                if (colIdx < 0 && column.AggregateType != AggregateType.LABEL)
                {
                    continue;
                }

                switch (column.AggregateType)
                {
                    case AggregateType.LABEL:
                        table.Field(0).TotalsRowLabel = column.Title;
                        break;

                    case AggregateType.AVERAGE:
                        table.Field(titles[colIdx]).TotalsRowFunction = XLTotalsRowFunction.Average;
                        break;

                    case AggregateType.COUNT:
                        table.Field(titles[colIdx]).TotalsRowFunction = XLTotalsRowFunction.Count;
                        break;

                    default:
                        table.Field(titles[colIdx]).TotalsRowFunction = XLTotalsRowFunction.Sum;
                        break;
                }
            }
        }
    }
}
