using Idevs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs.Examples;

/// <summary>
/// Example demonstrating how to use StaticServiceLocator with Idevs.Net.CoreLib
/// </summary>
public class StaticServiceLocatorExample
{
    /// <summary>
    /// Example of setting up StaticServiceLocator with Autofac (recommended)
    /// </summary>
    public static WebApplication CreateAppWithAutofac(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Autofac with Idevs services
        builder.UseIdevsAutofac();
        // builder.Services.AddControllers(); // Add if using MVC

        var app = builder.Build();

        // Initialize StaticServiceLocator (automatic detection)
        app.UseIdevsStaticServiceLocator();

        // app.UseRouting(); // Add if needed
        // app.MapControllers(); // Add if using MVC

        return app;
    }

    /// <summary>
    /// Example of setting up StaticServiceLocator with traditional DI
    /// </summary>
    public static WebApplication CreateAppWithTraditionalDI(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Use traditional service registration
        builder.Services.AddIdevsCorelibServices();
        // builder.Services.AddControllers(); // Add if using MVC

        var app = builder.Build();

        // Initialize StaticServiceLocator
        app.UseIdevsStaticServiceLocator();

        // app.UseRouting(); // Add if needed
        // app.MapControllers(); // Add if using MVC

        return app;
    }
}

/// <summary>
/// Example static helper class using StaticServiceLocator
/// </summary>
public static class ReportHelper
{
    /// <summary>
    /// Generate Excel report from static context
    /// </summary>
    public static byte[] GenerateExcelReport<T>(IEnumerable<T> data, Type columnsType)
    {
        // Resolve service statically
        var excelExporter = StaticServiceLocator.Resolve<IIdevsExcelExporter>();
        return excelExporter.Export(data, columnsType);
    }

    /// <summary>
    /// Generate PDF report from static context
    /// </summary>
    public static async Task<byte[]> GeneratePdfReportAsync(string html, string? header = null, string? footer = null)
    {
        // Resolve service statically
        var pdfExporter = StaticServiceLocator.Resolve<IIdevsPdfExporter>();
        return await pdfExporter.ExportAsync(html, header ?? "<p></p>", footer ?? "<p></p>");
    }

    /// <summary>
    /// Safe service resolution with null check
    /// </summary>
    public static bool IsServiceAvailable<T>() where T : class
    {
        var service = StaticServiceLocator.TryResolve<T>();
        return service != null;
    }

    /// <summary>
    /// Example of using cached singleton resolution for better performance
    /// </summary>
    public static void DoWorkWithCachedService()
    {
        // For services you know are registered as singletons
        // This caches the result for subsequent calls
        var cachedService = StaticServiceLocator.ResolveSingleton<IIdevsExcelExporter>();
        
        // Use the cached service
        // ... your work here
    }

    /// <summary>
    /// Example of working with scoped services
    /// </summary>
    public static void DoWorkWithScopedServices()
    {
        // Create a new scope for per-request services
        using var scope = StaticServiceLocator.CreateScope();
        
        // Resolve services from the scope
        var scopedService = scope.ServiceProvider.GetService<ISomeService>();
        
        // Work with scoped services
        // Scope is automatically disposed at the end
    }
}

/// <summary>
/// Example of legacy code integration
/// </summary>
public static class LegacyIntegration
{
    /// <summary>
    /// Example of integrating with legacy code that cannot use dependency injection
    /// </summary>
    public static void ProcessLegacyData(string filePath)
    {
        try
        {
            // Check if StaticServiceLocator is properly initialized
            if (!StaticServiceLocator.IsInitialized)
            {
                throw new InvalidOperationException("StaticServiceLocator is not initialized. Call app.UseIdevsStaticServiceLocator() in your Program.cs");
            }

            // Resolve required services
            var excelExporter = StaticServiceLocator.Resolve<IIdevsExcelExporter>();
            var viewRenderer = StaticServiceLocator.TryResolve<IViewPageRenderer>();

            // Process the data
            var data = LoadDataFromFile(filePath);
            var excelBytes = excelExporter.Export(data, typeof(DataColumns));

            // If view renderer is available, also generate PDF
            if (viewRenderer != null)
            {
                var html = viewRenderer.RenderViewAsync("DataReport", data).GetAwaiter().GetResult();
                var pdfExporter = StaticServiceLocator.Resolve<IIdevsPdfExporter>();
                var pdfBytes = pdfExporter.ExportAsync(html).GetAwaiter().GetResult();
                
                // Save both files
                SaveFile($"{filePath}.xlsx", excelBytes);
                SaveFile($"{filePath}.pdf", pdfBytes);
            }
            else
            {
                // Save only Excel file
                SaveFile($"{filePath}.xlsx", excelBytes);
            }
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("StaticServiceLocator"))
        {
            // Handle initialization issues
            Console.WriteLine($"Service locator error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Handle service resolution issues
            Console.WriteLine($"Error processing data: {ex.Message}");
            throw;
        }
    }

    private static IEnumerable<object> LoadDataFromFile(string filePath)
    {
        // Your data loading logic here
        return new List<object>();
    }

    private static void SaveFile(string filePath, byte[] data)
    {
        // Your file saving logic here
        File.WriteAllBytes(filePath, data);
    }
}

/// <summary>
/// Example interface for demonstration
/// </summary>
public interface ISomeService
{
    void DoSomething();
}

/// <summary>
/// Example data columns for demonstration
/// </summary>
public class DataColumns
{
    public string? Name { get; set; }
    public int Value { get; set; }
    public DateTime Date { get; set; }
}
