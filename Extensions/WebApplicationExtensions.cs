using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs.Extensions;

/// <summary>
/// Extension methods for WebApplication to initialize StaticServiceLocator
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Initializes the StaticServiceLocator with the application's service provider
    /// </summary>
    /// <param name="app">The web application</param>
    /// <returns>The web application for chaining</returns>
    /// <remarks>
    /// This method automatically detects whether Autofac or traditional DI is being used
    /// and initializes the StaticServiceLocator accordingly.
    /// Call this method early in your application startup, typically right after Build().
    /// </remarks>
    public static WebApplication UseIdevsStaticServiceLocator(this WebApplication app)
    {
        // Check if we're using Autofac by looking for ILifetimeScope
        var lifetimeScope = app.Services.GetService<ILifetimeScope>();
        
        if (lifetimeScope != null)
        {
            // Initialize with Autofac container
            StaticServiceLocator.Initialize(lifetimeScope);
        }
        else
        {
            // Initialize with traditional service provider
            StaticServiceLocator.Initialize(app.Services);
        }
        
        return app;
    }

    /// <summary>
    /// Initializes the StaticServiceLocator with a specific Autofac container
    /// </summary>
    /// <param name="app">The web application</param>
    /// <param name="container">The Autofac container to use</param>
    /// <returns>The web application for chaining</returns>
    public static WebApplication UseIdevsStaticServiceLocator(this WebApplication app, ILifetimeScope container)
    {
        StaticServiceLocator.Initialize(container);
        return app;
    }

    /// <summary>
    /// Initializes the StaticServiceLocator with a specific service provider
    /// </summary>
    /// <param name="app">The web application</param>
    /// <param name="serviceProvider">The service provider to use</param>
    /// <returns>The web application for chaining</returns>
    public static WebApplication UseIdevsStaticServiceLocator(this WebApplication app, IServiceProvider serviceProvider)
    {
        StaticServiceLocator.Initialize(serviceProvider);
        return app;
    }
}
