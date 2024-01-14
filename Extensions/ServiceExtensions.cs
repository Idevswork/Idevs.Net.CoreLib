using System.Reflection;
using Idevs.ComponentModel;
using Idevs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs.Extensions;

public static class ServicExtensions
{
    public static void AddIdevsCoreLibServices(this IServiceCollection services) {
        services.AddScoped<IViewPageRenderer, ViewPageRenderer>();
        services.AddScoped<IIdevsPdfExporter, IdevsPdfExporter>();
        services.AddScoped<IIdevsExcelExporter, IdevsExcelExporter>();

        services.RegisterServices();
    }

    public static void RegisterServices(this IServiceCollection services, Assembly assembly)
    {
        var scopedRegistration = typeof(ScopedRegistrationAttribute);
        var singletonRegistration = typeof(SingletonRegiatrationAttribute);
        var transientRegistration = typeof(TransientRegistrationAttribute);

        var types = assembly.GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttributes(scopedRegistration, true).Length > 0)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type, serviceProvider => ActivatorUtilities.CreateInstance(serviceProvider, type));
        }

        types = assembly.GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttributes(singletonRegistration, true).Length > 0)
            .ToList();
        
        foreach (var type in types)
        {
            services.AddSingleton(type, serviceProvider => ActivatorUtilities.CreateInstance(serviceProvider, type));
        }

        types = assembly.GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttributes(transientRegistration, true).Length > 0)
            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type, serviceProvider => ActivatorUtilities.CreateInstance(serviceProvider, type));
        }
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            services.RegisterServices(assembly);
        }
    }
}
