using Serenity.Services;

namespace Idevs.Models;

public enum AggregateType
{
    AVERAGE = 0,
    COUNT = 1,
    SUM = 2
}

public struct AggregateColumn
{
    public string ColumnName { get; set; }
    public AggregateType AggregateType { get; set; }
}

public interface IIdevsExportRequest
{
    string ViewName { get; set; }
    string? CompanyName { get; set; }
    string? ReportName { get; set; }
    string? SelectionRange { get; set; }
    string? Logo { get; set; }
    IEnumerable<AggregateColumn>? AggregateColumns { get; set; }
}

public class IdevsExportRequest : ListRequest, IIdevsExportRequest
{
    public string ViewName { get; set; } = "";
    public string? CompanyName { get; set; }
    public string? ReportName { get; set; }
    public string? SelectionRange { get; set; }
    public string? Logo { get; set; }
    public IEnumerable<AggregateColumn>? AggregateColumns { get; set; }
}