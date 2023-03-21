using Serenity.Services;

namespace Idevs.Models;

public interface IIdevsExportRequest
{
    string ViewName { get; set; }
    object? Filters { get; set; }
}

public class IdevsExportRequest : ListRequest, IIdevsExportRequest
{
    public string ViewName { get; set; } = "";
    private object? filters;
    public object? Filters { get => filters; set => filters = value; }
}