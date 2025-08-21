using Ardalis.GuardClauses;
using PuppeteerSharp;

namespace Idevs;

/// <summary>
/// Provides PDF export functionality using Puppeteer Sharp for HTML-to-PDF conversion
/// </summary>
public interface IIdevsPdfExporter
{
    /// <summary>
    /// Exports HTML content to PDF format synchronously
    /// </summary>
    /// <param name="html">HTML content to convert to PDF</param>
    /// <param name="headerTemplate">HTML template for page header</param>
    /// <param name="footerTemplate">HTML template for page footer</param>
    /// <returns>PDF file as byte array</returns>
    byte[] Export(string html, string headerTemplate = "<p></p>", string footerTemplate = "<p></p>") =>
        Task.Run(async () => await ExportAsync(html, headerTemplate, footerTemplate)).GetAwaiter().GetResult();

    /// <summary>
    /// Exports HTML content to PDF format asynchronously
    /// </summary>
    /// <param name="html">HTML content to convert to PDF</param>
    /// <param name="headerTemplate">HTML template for page header</param>
    /// <param name="footerTemplate">HTML template for page footer</param>
    /// <returns>Task containing PDF file as byte array</returns>
    Task<byte[]> ExportAsync(string html, string headerTemplate = "<p></p>", string footerTemplate = "<p></p>");
}

/// <summary>
/// Implementation of PDF export functionality using Puppeteer Sharp
/// </summary>
public class IdevsPdfExporter : IIdevsPdfExporter
{
    /// <inheritdoc />
    public async Task<byte[]> ExportAsync(
        string html,
        string headerTemplate = "<p></p>",
        string footerTemplate = "<p></p>")
    {
        Guard.Against.NullOrEmpty(html, nameof(html));
        Guard.Against.Null(headerTemplate, nameof(headerTemplate));
        Guard.Against.Null(footerTemplate, nameof(footerTemplate));
        
        return await DoGeneratePdf(html, headerTemplate, footerTemplate);
    }

    private static async Task<byte[]> DoGeneratePdf(
        string html,
        string headerTemplate,
        string footerTemplate)
    {
        var chromePath = ChromeHelper.GetChromePath();
        var launchOption = new LaunchOptions
        {
            Headless = true,
            IgnoredDefaultArgs = ["--disable-extensions"],
            Args = ["--no-sandbox", "--disable-setuid-sandbox"],
            ExecutablePath = chromePath
        };

        await using var browser = await Puppeteer.LaunchAsync(launchOption);
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle0] });
        return await page.PdfDataAsync(new PdfOptions
        {
            PreferCSSPageSize = true,
            HeaderTemplate = headerTemplate,
            FooterTemplate = footerTemplate,
            DisplayHeaderFooter = true
        });
    }
}
