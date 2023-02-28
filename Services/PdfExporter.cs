using PugPdf.Core;

namespace Idevs.Services;

public interface IPdfExporter
{
    byte[] Export(string html, PdfPrintOptions? options = null);
}

public class PdfExporter : IPdfExporter
{
    public byte[] Export(string html, PdfPrintOptions? options = null)
    {
        options ??= new PdfPrintOptions();
        var renderer = new HtmlToPdf
        {
            PrintOptions = options
        };

        var pdf = renderer.RenderHtmlAsPdfAsync(html).Result;
        return pdf.BinaryData;
    }
}