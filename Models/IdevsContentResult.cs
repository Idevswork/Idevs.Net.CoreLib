using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Idevs.Models;

public enum IdevsContentType
{
    Excel,
    PDF
}

public class IdevsContentResult
{
    /// <summary>
    /// Creates a FileContentResult containing passed data
    /// </summary>
    /// <param name="data">Data containing Pdf bytes</param>
    /// <returns></returns>
    public static FileContentResult Create(byte[] data, IdevsContentType contentType)
    {
        return Create(data, contentType);
    }

    /// <summary>
    /// Creates a FileContentResult containing passed data and a download name
    /// </summary>
    /// <param name="data">Data containing Pdf file bytes</param>
    /// <param name="contentType">Content type</param>
    /// <param name="downloadName">Optional download name</param>
    public static FileContentResult Create(byte[] data, IdevsContentType contentType, string downloadName)
    {
        var dataType = contentType == IdevsContentType.Excel
        ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        : "application/octet-stream";
        var result = new FileContentResult(data, dataType)
        {
            FileDownloadName = downloadName ?? GetDownloadName(contentType)
        };
        return result;
    }

    private static string GetDownloadName(IdevsContentType contentType)
    {
        var ext = contentType == IdevsContentType.Excel ? ".xlsx" : ".pdf";
        return ("report" + DateTime.Now.ToString("yyyyMMddHHmmss",
                           CultureInfo.InvariantCulture) + ext);
    }
}