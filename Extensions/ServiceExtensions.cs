using Idevs.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs.Extensions;

public static class ServicExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Define types that need matching
        Type scopedRegistration = typeof(ScopedRegistrationAttribute);
        Type singletonRegistration = typeof(SingletonRegiatrationAttribute);
        Type transientRegistration = typeof(TransientRegistrationAttribute);

        var types = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(p => p.IsDefined(scopedRegistration, false) || p.IsDefined(transientRegistration, false) || p.IsDefined(singletonRegistration, false) && !p.IsInterface)
        .Select(s => new
        {
            Service = s.GetInterface($"I{s.Name}"),
            Implementation = s
        })
        .Where(x => x.Service != null);

        foreach (var type in types)
        {
            if (type.Implementation.IsDefined(scopedRegistration, false) && type.Service is not null)
            {
                services.AddScoped(type.Service, type.Implementation);
            }

            if (type.Implementation.IsDefined(transientRegistration, false) && type.Service is not null)
            {
                services.AddTransient(type.Service, type.Implementation);
            }

            if (type.Implementation.IsDefined(singletonRegistration, false) && type.Service is not null)
            {
                services.AddTransient(type.Service, type.Implementation);
            }
        }
    }
}