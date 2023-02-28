using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Idevs.Models;

public class PdfContentResult
{
    /// <summary>
    /// Creates a FileContentResult containing passed data
    /// </summary>
    /// <param name="data">Data containing Pdf bytes</param>
    /// <returns></returns>
    public static FileContentResult Create(byte[] data)
    {
        return Create(data, GetDownloadName());
    }

    /// <summary>
    /// Creates a FileContentResult containing passed data and a download name
    /// </summary>
    /// <param name="data">Data containing Pdf file bytes</param>
    /// <param name="downloadName">Optional download name</param>
    public static FileContentResult Create(byte[] data, string downloadName)
    {
        var result = new FileContentResult(data, "application/octet-stream")
        {
            FileDownloadName = downloadName ?? GetDownloadName()
        };
        return result;
    }

    private static string GetDownloadName() => ("report" +
                                                DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + ".pdf");
}