using Serenity.Services;

namespace Idevs.Models;

public interface IIdevsExportRequest
{
    string ViewName { get; set; }
}

public class IdevsExportRequest : ListRequest, IIdevsExportRequest
{
    public string ViewName { get; set; } = "";
}