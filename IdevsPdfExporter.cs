using System.Collections.Concurrent;
using System.Globalization;
using Ardalis.GuardClauses;
using HandlebarsDotNet;
using Idevs.Models;
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
    /// <param name="header">HTML template for page header</param>
    /// <param name="footer">HTML template for page footer</param>
    /// <returns>PDF file as a byte array</returns>
    byte[] ExportByteArray(string html, string? header = null, string? footer = null) =>
        Task.Run(async () => await ExportByteArrayAsync(html, header, footer)).GetAwaiter().GetResult();

    /// <summary>
    /// Exports HTML content to PDF format asynchronously
    /// </summary>
    /// <param name="html">HTML content to convert to PDF</param>
    /// <param name="header">HTML template for page header</param>
    /// <param name="footer">HTML template for page footer</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task containing a PDF file as a byte array</returns>
    Task<byte[]> ExportByteArrayAsync(string html, string? header = null, string? footer = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a response containing the PDF file for download
    /// </summary>
    /// <param name="html"></param>
    /// <param name="header"></param>
    /// <param name="footer"></param>
    /// <param name="downloadName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdevsContentResponse> CreateResponseAsync(string html, string? header = null, string? footer = null,
        string? downloadName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Register custom helpers for Handlebars.NET
    /// </summary>
    /// <param name="registerHelper"></param>
    void RegisterCustomHelpers(Action<IHandlebars, CultureInfo> registerHelper);

    /// <summary>
    /// Exports data to a PDF format using a template file
    /// </summary>
    /// <param name="templatePath"></param>
    /// <param name="model"></param>
    /// <param name="header"></param>
    /// <param name="footer"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    Task<byte[]> CompileTemplateAsync<TModel>(string templatePath, TModel model, string? header = null,
        string? footer = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports data to a PDF format using a template file and creates a response for download
    /// </summary>
    /// <param name="templatePath"></param>
    /// <param name="model"></param>
    /// <param name="header"></param>
    /// <param name="footer"></param>
    /// <param name="downloadName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TModel"></typeparam>
    /// <returns></returns>
    Task<IdevsContentResponse> CreateResponseAsync<TModel>(string templatePath, TModel model, string? header = null,
        string? footer = null, string? downloadName = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of PDF export functionality using Puppeteer Sharp
/// </summary>
public class IdevsPdfExporter : IIdevsPdfExporter
{
    // Optional per-scope template cache (ok)
    private readonly ConcurrentDictionary<string, HandlebarsTemplate<object, object>> _compiledTemplates = new();

    private readonly IHandlebars _handlebars;
    private readonly CultureInfo _defaultCulture;

    public IdevsPdfExporter(CultureInfo? defaultCulture = null)
    {
        _defaultCulture = defaultCulture ?? CultureInfo.GetCultureInfo("th-TH");
        _handlebars = Handlebars.Create(new HandlebarsConfiguration
        {
            ThrowOnUnresolvedBindingExpression = false
        });
        RegisterHelpers();
    }

    /// <inheritdoc />
    public async Task<byte[]> ExportByteArrayAsync(
        string html,
        string? header = null,
        string? footer = null,
        CancellationToken cancellationToken = default
    )
    {
        Guard.Against.NullOrEmpty(html, nameof(html));

        return await DoGeneratePdfAsync(html, header, footer, cancellationToken);
    }

    public async Task<IdevsContentResponse> CreateResponseAsync(string html, string? header = null, string? footer = null,
        string? downloadName = null, CancellationToken cancellationToken = default)
    {
        var bytes = await ExportByteArrayAsync(html, header, footer, cancellationToken);
        return new IdevsContentResponse
        {
            Content = Convert.ToBase64String(bytes),
            ContentType = "application/pdf",
            DownloadName = downloadName ??
                           "report" + DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + ".pdf"
        };
    }

    private static async Task<byte[]> DoGeneratePdfAsync(
        string html,
        string? header = null,
        string? footer = null,
        CancellationToken cancellationToken = default
    )
    {
        Guard.Against.NullOrEmpty(html, nameof(html));

        // Use per-request browser instance for better reliability
        var chromePath = ChromeHelper.GetChromePath();
        var launchOption = new LaunchOptions
        {
            Headless = true,
            IgnoredDefaultArgs = ["--disable-extensions"],
            Args =
            [
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-web-security",
                "--allow-running-insecure-content"
            ],
            ExecutablePath = chromePath
        };

        await using var browser = await Puppeteer.LaunchAsync(launchOption);
        if (browser == null)
        {
            throw new InvalidOperationException("Failed to initialize browser instance");
        }

        await using var page = await browser.NewPageAsync();
        if (page == null)
        {
            throw new InvalidOperationException("Failed to create new page in browser");
        }

        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle0] });

        var pdfData = await page.PdfDataAsync(new PdfOptions
        {
            PreferCSSPageSize = true,
            PrintBackground = true,
            HeaderTemplate = header ?? string.Empty,
            FooterTemplate = footer ?? string.Empty,
            DisplayHeaderFooter = !string.IsNullOrEmpty(header) || !string.IsNullOrEmpty(footer)
        });

        if (pdfData == null || pdfData.Length == 0)
        {
            throw new InvalidOperationException("PDF generation failed - no data returned");
        }

        return pdfData;
    }


    private void RegisterHelpers()
    {
        // Equality helper for conditionals
        _handlebars.RegisterHelper("eq", (writer, options, context, parameters) =>
        {
            if (parameters.Length < 2) return;

            var isEqual = parameters[0]?.ToString() == parameters[1]?.ToString();

            if (isEqual)
            {
                options.Template(writer, context);   // renders block
            }
            else
            {
                options.Inverse(writer, context);    // renders else block
            }
        });

        // Register the format number helper with optional precision
        _handlebars.RegisterHelper("formatNumber", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] is null)
            {
                writer.Write("0");
                return;
            }

            if (parameters[0] is not IFormattable n)
            {
                writer.WriteSafeString(parameters[0]?.ToString() ?? "0");
                return;
            }

            // Default: "N2"
            var pattern = parameters.Length >= 2 && parameters[1] is string s ? s : "N2";

            // Optional: culture as 3rd arguments (e.g. "th-TH", "en-US")
            var culture = _defaultCulture;
            if (parameters.Length >= 3 && parameters[2] is string c && !string.IsNullOrWhiteSpace(c))
            {
                try
                {
                    culture = CultureInfo.GetCultureInfo(c);
                }
                catch (CultureNotFoundException)
                {
                    // Fall back to default culture if invalid culture specified
                    culture = _defaultCulture;
                }
            }

            writer.WriteSafeString(n.ToString(pattern, culture));
        });

        // Usage:
        //   {{formatCurrency amount}}                      -> default culture, "C2"
        //   {{formatCurrency amount "C0"}}                 -> default culture, no decimals
        //   {{formatCurrency amount "C2" "en-US"}}         -> US culture, with symbol $
        //   {{formatCurrency amount "N2" "th-TH" "฿"}}     -> Thai culture, custom symbol ฿
        //   {{formatCurrency amount "N2" "" "USD"}}        -> default culture, custom USD symbol
        _handlebars.RegisterHelper("formatCurrency", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString("0");
                return;
            }

            if (parameters[0] is not IFormattable number)
            {
                writer.WriteSafeString(parameters[0]?.ToString() ?? "0");
                return;
            }

            // Format string (default "C2")
            var format = (parameters.Length >= 2 && parameters[1] is string f && !string.IsNullOrWhiteSpace(f))
                ? f
                : "C2";

            // Culture (default = injected) with error handling
            var culture = _defaultCulture;
            if (parameters.Length >= 3 && parameters[2] is string c && !string.IsNullOrWhiteSpace(c))
            {
                try
                {
                    culture = CultureInfo.GetCultureInfo(c);
                }
                catch (CultureNotFoundException)
                {
                    // Fall back to default culture if invalid culture specified
                    culture = _defaultCulture;
                }
            }

            // Custom symbol (optional)
            var customSymbol = (parameters.Length >= 4 && parameters[3] is string s && !string.IsNullOrWhiteSpace(s))
                ? s
                : null;

            var value = number.ToString(format, culture);

            if (!string.IsNullOrEmpty(customSymbol))
            {
                // Replace culture currency symbol if "C" format used
                if (format.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                {
                    var symbol = culture.NumberFormat.CurrencySymbol;
                    value = value.Replace(symbol, customSymbol);
                }
                else
                {
                    // For "N2" or "F2" formats, just prepend/append
                    value = $"{customSymbol}{value}";
                }
            }

            writer.WriteSafeString(value);
        });

        // {{formatDate dateValue "dd/MM/yyyy" "th-TH"}}
        _handlebars.RegisterHelper("formatDate", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString(string.Empty);
                return;
            }

            if (parameters[0] is not DateTime dt)
            {
                // try to parse string
                var stringValue = parameters[0]?.ToString();
                if (string.IsNullOrEmpty(stringValue) || !DateTime.TryParse(stringValue, out dt))
                {
                    writer.WriteSafeString(stringValue ?? string.Empty);
                    return;
                }
            }

            // format pattern (default: short date)
            var format = (parameters.Length >= 2 && parameters[1] is string f && !string.IsNullOrWhiteSpace(f))
                ? f
                : "d";

            // culture (default: provided) with error handling
            var culture = _defaultCulture;
            if (parameters.Length >= 3 && parameters[2] is string c && !string.IsNullOrWhiteSpace(c))
            {
                try
                {
                    culture = CultureInfo.GetCultureInfo(c);
                }
                catch (CultureNotFoundException)
                {
                    culture = _defaultCulture;
                }
            }

            writer.WriteSafeString(dt.ToString(format, culture));
        });

        // {{formatDateTime dateValue "g" "en-US"}}
        _handlebars.RegisterHelper("formatDateTime", (writer, context, parameters) =>
        {
            if (parameters.Length == 0 || parameters[0] == null)
            {
                writer.WriteSafeString(string.Empty);
                return;
            }

            if (parameters[0] is not DateTime dt)
            {
                var stringValue = parameters[0]?.ToString();
                if (string.IsNullOrEmpty(stringValue) || !DateTime.TryParse(stringValue, out dt))
                {
                    writer.WriteSafeString(stringValue ?? string.Empty);
                    return;
                }
            }

            // format pattern (default: "g" general short date/time)
            var format = (parameters.Length >= 2 && parameters[1] is string f && !string.IsNullOrWhiteSpace(f))
                ? f
                : "g";

            // culture with error handling
            var culture = _defaultCulture;
            if (parameters.Length >= 3 && parameters[2] is string c && !string.IsNullOrWhiteSpace(c))
            {
                try
                {
                    culture = CultureInfo.GetCultureInfo(c);
                }
                catch (CultureNotFoundException)
                {
                    culture = _defaultCulture;
                }
            }

            writer.WriteSafeString(dt.ToString(format, culture));
        });

        // {{conditionalClass value conditions "class-when-true" ["class-when-false"] ["ignoreCase"]}}
        // Example:
        // <div class="{{conditionalClass status "paid" "text-green" "text-red" true}}">
        //   {{status}}
        // </div>
        _handlebars.RegisterHelper("conditionalClass", (writer, context, parameters) =>
        {
            if (parameters.Length is 0 or < 3) return;

            var value     = parameters[0]?.ToString() ?? "";
            var condition = parameters[1]?.ToString() ?? "";
            var trueClass = parameters[2]?.ToString() ?? "";
            var falseClass = parameters.Length >= 4 ? parameters[3]?.ToString() ?? "" : "";

            var ignoreCase = parameters.Length >= 5 &&
                             bool.TryParse(parameters[4]?.ToString(), out var ic) && ic;

            var comparison = ignoreCase
                ? string.Equals(value, condition, StringComparison.OrdinalIgnoreCase)
                : string.Equals(value, condition, StringComparison.Ordinal);

            writer.WriteSafeString(comparison ? trueClass : falseClass);
        });
    }

    /// <inheritdoc />
    public void RegisterCustomHelpers(Action<IHandlebars, CultureInfo> registerHelper)
    {
        Guard.Against.Null(registerHelper, nameof(registerHelper));
        registerHelper(_handlebars, _defaultCulture);
    }

    /// <inheritdoc />
    public async Task<byte[]> CompileTemplateAsync<TModel>(string templatePath, TModel model, string? header = null, string? footer = null,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(templatePath, nameof(templatePath));
        Guard.Against.Null(model, nameof(model));

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template file not found: {templatePath}");
        }

        var tpl = await File.ReadAllTextAsync(templatePath, cancellationToken);
        if (string.IsNullOrEmpty(tpl))
        {
            throw new InvalidOperationException($"Template file is empty: {templatePath}");
        }

        var compiled = _compiledTemplates.GetOrAdd(templatePath, _ => _handlebars.Compile(tpl));
        var html = compiled(model);

        if (string.IsNullOrEmpty(html))
        {
            throw new InvalidOperationException("Template compilation resulted in empty HTML");
        }

        return await ExportByteArrayAsync(html, header, footer, cancellationToken);
    }

    public async Task<IdevsContentResponse> CreateResponseAsync<TModel>(string templatePath, TModel model, string? header = null, string? footer = null,
        string? downloadName = null, CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(templatePath, nameof(templatePath));
        Guard.Against.Null(model, nameof(model));

        var bytes = await CompileTemplateAsync(templatePath, model, header, footer, cancellationToken);

        if (bytes == null || bytes.Length == 0)
        {
            throw new InvalidOperationException("PDF generation resulted in empty content");
        }

        try
        {
            var base64Content = Convert.ToBase64String(bytes);
            return new IdevsContentResponse
            {
                Content = base64Content,
                ContentType = "application/pdf",
                DownloadName = downloadName ??
                               "report" + DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + ".pdf"
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to convert PDF content to Base64: {ex.Message}", ex);
        }
    }
}
