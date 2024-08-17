using PuppeteerSharp;

namespace Idevs.Services;

public interface IIdevsPdfExporter
{
    byte[] Export(string html, string headerTemplate = "<p></p>", string footerTemplate = "<p></p>", string browserPath = "");
}

public class IdevsPdfExporter : IIdevsPdfExporter
{
    public byte[] Export(
        string html,
        string headerTemplate = "<p></p>",
        string footerTemplate = "<p></p>",
        string browserPath = "")
    {
        var pdfBytes = Task.Run(async () => await DoGeneratePdf(html, headerTemplate, footerTemplate, browserPath)).Result;
        return pdfBytes;
    }

    private static async Task<byte[]> DoGeneratePdf(
        string html,
        string headerTemplate = "<p></p>",
        string footerTemplate = "<p></p>",
        string browserPath = "")
    {
        var launchOption = new LaunchOptions
        {
            Headless = true
        };

        if (string.IsNullOrEmpty(browserPath))
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
        }
        else
        {
            launchOption.ExecutablePath = browserPath;
        }

        await using var browser = await Puppeteer.LaunchAsync(launchOption);
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation. Networkidle0 }});
        return await page.PdfDataAsync(new PdfOptions
        {
            PreferCSSPageSize = true,
            HeaderTemplate = headerTemplate,
            FooterTemplate = footerTemplate,
            DisplayHeaderFooter = true
        });
    }
}
