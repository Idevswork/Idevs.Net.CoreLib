using Idevs.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Idevs.Examples;

/// <summary>
/// Example demonstrating how to use Idevs.Net.CoreLib with Autofac
/// </summary>
public class AutofacExample
{
    /// <summary>
    /// Basic Autofac configuration example
    /// </summary>
    public static WebApplication CreateBasicApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Autofac with Idevs services
        builder.UseIdevsAutofac();

        // Add your application services here
        // builder.Services.AddControllers(); // Example - add if using MVC

        var app = builder.Build();

        // Configure the HTTP request pipeline
        // app.UseRouting(); // Example - add if needed
        // app.MapControllers(); // Example - add if using MVC

        return app;
    }

    /// <summary>
    /// Advanced Autofac configuration example with custom services
    /// </summary>
    public static WebApplication CreateAdvancedApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Autofac with custom container configuration
        builder.UseIdevsAutofac(containerBuilder =>
        {
            // Register your custom services
            // containerBuilder.RegisterType<MyCustomService>()
            //     .As<IMyCustomService>()
            //     .InstancePerLifetimeScope();
            
            // Register decorators, interceptors, etc.
        });

        // builder.Services.AddControllers(); // Example - add if using MVC

        var app = builder.Build();

        // app.UseRouting(); // Example - add if needed
        // app.MapControllers(); // Example - add if using MVC

        return app;
    }

    /// <summary>
    /// Legacy approach for projects that cannot use Autofac
    /// </summary>
    public static WebApplication CreateLegacyApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Use traditional service registration (fallback)
        builder.Services.AddIdevsCorelibServices();
        // builder.Services.AddControllers(); // Example - add if using MVC

        var app = builder.Build();

        // app.UseRouting(); // Example - add if needed
        // app.MapControllers(); // Example - add if using MVC

        return app;
    }
}

// Example controller using Idevs services
/*
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    private readonly IIdevsExcelExporter _excelExporter;
    private readonly IIdevsPdfExporter _pdfExporter;
    private readonly IViewPageRenderer _viewRenderer;

    public ExampleController(
        IIdevsExcelExporter excelExporter,
        IIdevsPdfExporter pdfExporter,
        IViewPageRenderer viewRenderer)
    {
        _excelExporter = excelExporter;
        _pdfExporter = pdfExporter;
        _viewRenderer = viewRenderer;
    }

    [HttpPost("export-excel")]
    public IActionResult ExportExcel()
    {
        var data = GetSampleData();
        var bytes = _excelExporter.Export(data, typeof(SampleColumns));
        return IdevsContentResult.Create(bytes, IdevsContentType.Excel, "export.xlsx");
    }

    [HttpPost("export-pdf")]
    public async Task<IActionResult> ExportPdf()
    {
        var html = await _viewRenderer.RenderViewAsync("SampleReport", GetSampleData());
        var bytes = await _pdfExporter.ExportAsync(html);
        return IdevsContentResult.Create(bytes, IdevsContentType.PDF, "report.pdf");
    }

    private List<object> GetSampleData()
    {
        // Return your sample data
        return new List<object>();
    }
}
*/
