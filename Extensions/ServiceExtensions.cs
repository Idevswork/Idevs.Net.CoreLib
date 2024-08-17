using System.Reflection;
using Idevs.ComponentModel;
using Idevs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs.Extensions;

public static class ServicExtensions
{
    public static IServiceCollection AddIdevsCoreLibServices(this IServiceCollection services) {
        services.AddScoped<IViewPageRenderer, ViewPageRenderer>();
        services.AddScoped<IIdevsPdfExporter, IdevsPdfExporter>();
        services.AddScoped<IIdevsExcelExporter, IdevsExcelExporter>();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var scopedRegistration = typeof(ScopedRegistrationAttribute);
        var singletonRegistration = typeof(SingletonRegiatrationAttribute);
        var transientRegistration = typeof(TransientRegistrationAttribute);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !(assembly.FullName ?? string.Empty).StartsWith("System.Data.SqlClient"))
            .SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null);
                }
            })
            .Where(type =>
                (type.IsDefined(scopedRegistration, false) || type.IsDefined(singletonRegistration, false) ||
                 type.IsDefined(transientRegistration, false)) && !type.IsInterface)
            .Select(s => new
            {
                Interface = s.GetInterface($"I{s.Name}"),
                Implementation = s
            })
            .Where(x => x.Interface != null)
            .ToList();

        foreach (var type in types)
        {
            if (type.Implementation.IsDefined(scopedRegistration, false) && type.Interface is not null)
            {
                services.AddScoped(type.Interface, type.Implementation);
                continue;
            }

            if (type.Implementation.IsDefined(transientRegistration, false) && type.Interface is not null)
            {
                services.AddTransient(type.Interface, type.Implementation);
                continue;
            }

            if (type.Implementation.IsDefined(singletonRegistration, false) && type.Interface is not null)
            {
                services.AddSingleton(type.Interface, type.Implementation);
            }
        }

        return services;
    }
}
