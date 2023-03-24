using System.Collections;
using ClosedXML.Excel;
using FastMember;
using Serenity.Data;
using Serenity.Reporting;

namespace Idevs.Services;

public interface IIdevsExcelExporter
{
    byte[] Export(IEnumerable data, IEnumerable<ReportColumn> columns, IEnumerable<string>? headers = null);
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string>? headers = null);
    byte[] Export(IEnumerable data, Type columnsType, IEnumerable<string> exportColumns, IEnumerable<string>? headers = null);
    byte[] Generate(IReadOnlyList<ReportColumn> columns, ICollection rows, IEnumerable<string>? headers = null,
        string sheetName = "Page1", string tableName = "Table1");
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

    public byte[] Generate(IReadOnlyList<ReportColumn> columns, ICollection rows, IEnumerable<string>? headers = null,
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

        if (rows.Count > 0)
        {
            worksheet.Cell(startRow + 1, 1).InsertData(dataList);
            var range = worksheet.Range(startRow, 1, endRow, colCount);

            // create the actual table
            var table = range.CreateTable();

            // apply style
            table.Theme = XLTableTheme.TableStyleMedium2;
        }

        worksheet.Columns().AdjustToContents();

        var ms = new MemoryStream();
        workbook.SaveAs(ms);

        return ms.ToArray();
    }
}
