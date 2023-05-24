using Serenity.Services;

namespace Idevs.Models;

public interface IIdevsExportRequest
{
    string ViewName { get; set; }
    string? CompanyName { get; set; }
    string? ReportName { get; set; }
    string? SelectionRange { get; set; }
    string? Logo { get; set; }
    object? Entity { get; set; }
}

public class IdevsExportRequest : ListRequest, IIdevsExportRequest
{
    public string ViewName { get; set; } = "";
    public string? CompanyName { get; set; }
    public string? ReportName { get; set; }
    public string? SelectionRange { get; set; }
    public string? Logo { get; set; }
    public object? Entity { get; set; }
}