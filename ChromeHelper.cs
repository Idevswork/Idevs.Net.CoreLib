using PuppeteerSharp;

namespace Idevs;

/// <summary>
/// Usage: It should call ChromeHelper.DownloadChrome()
/// before using PuppeteerSharp to ensure the Chromium browser is downloaded.
/// The best way is to call on Program.cs Main method.
/// Example:
/// public static void Main(string[] args)
/// {
///     ChromeHelper.DownloadChrome();
///
///     CreateHostBuilder(args).Build().Run();
/// }
/// </summary>
public static class ChromeHelper
{
    private static string BasePath => Path.Combine(AppContext.BaseDirectory, "Idevs", "chromium");

    public static bool IsChromeDownloaded()
    {
        var browserPath = GetChromePath();
        return !string.IsNullOrEmpty(browserPath) && File.Exists(browserPath);
    }

    public static string GetChromePath()
    {
        var basePath = BasePath;
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
            return null;
        }

        string chromiumPath = null;
        if (OperatingSystem.IsWindows())
        {
            basePath = Path.Combine(basePath, "Chrome");
            var directories = Directory.GetDirectories(basePath, "Win*");
            if (directories.Length > 0)
            {
                chromiumPath = Path.Combine(basePath, directories[0], "chrome-win64", "chrome.exe");
            }
        }
        else if (OperatingSystem.IsMacOS())
        {
            basePath = Path.Combine(basePath, "Chrome");
            var directories = Directory.GetDirectories(basePath, "Mac*");
            if (directories.Length > 0)
            {
                chromiumPath = Path.Combine(basePath, directories[0], "chrome-mac-x64", "Google Chrome for Testing.app", "Contents", "MacOS", "Google Chrome for Testing");
            }
        }

        return chromiumPath;
    }

    public static void DownloadChrome()
    {
        if (IsChromeDownloaded()) return;

        var basePath = BasePath;

        // If the Chromium browser is not found, download it
        var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
        {
            Path = basePath
        });

        Task.Run(async () =>
        {
            await browserFetcher.DownloadAsync();
        }).GetAwaiter().GetResult();
    }
}
