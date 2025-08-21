namespace Idevs;

/// <summary>
/// Static service provider for backward compatibility
/// </summary>
/// <remarks>
/// This class is obsolete and will be removed in a future version.
/// Use StaticServiceLocator instead for better Autofac support and thread safety.
/// </remarks>
[Obsolete("Use StaticServiceLocator instead for better Autofac support. This will be removed in version 1.0.0", false)]
public static class StaticServiceProvider
{
    private static IServiceProvider? _provider;

    /// <summary>
    /// Gets or sets the service provider instance
    /// </summary>
    /// <remarks>
    /// Setting this property automatically initializes StaticServiceLocator for backward compatibility.
    /// Consider migrating to StaticServiceLocator directly for better performance and features.
    /// </remarks>
    [Obsolete("Use StaticServiceLocator instead for better Autofac support. This will be removed in version 1.0.0", false)]
    public static IServiceProvider? Provider 
    { 
        get => _provider;
        set 
        {
            _provider = value;
            
            // Automatically initialize StaticServiceLocator for backward compatibility
            if (value != null && !StaticServiceLocator.IsInitialized)
            {
                try
                {
                    StaticServiceLocator.Initialize(value);
                }
                catch (InvalidOperationException)
                {
                    // StaticServiceLocator is already initialized with a different provider type
                    // This is acceptable for backward compatibility
                }
            }
            else if (value == null)
            {
                // Reset StaticServiceLocator if provider is set to null
                StaticServiceLocator.Reset();
            }
        }
    }

    /// <summary>
    /// Resolves a service of the specified type (backward compatibility method)
    /// </summary>
    /// <typeparam name="T">The type of service to resolve</typeparam>
    /// <returns>The resolved service instance, or null if not found</returns>
    /// <remarks>
    /// This method delegates to StaticServiceLocator for improved functionality.
    /// Consider using StaticServiceLocator.Resolve&lt;T&gt;() directly.
    /// </remarks>
    [Obsolete("Use StaticServiceLocator.Resolve<T>() instead for better error handling. This will be removed in version 1.0.0", false)]
    public static T? GetService<T>() where T : class
    {
        if (StaticServiceLocator.IsInitialized)
        {
            return StaticServiceLocator.TryResolve<T>();
        }

        return _provider?.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Resolves a required service of the specified type (backward compatibility method)
    /// </summary>
    /// <typeparam name="T">The type of service to resolve</typeparam>
    /// <returns>The resolved service instance</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service cannot be resolved</exception>
    /// <remarks>
    /// This method delegates to StaticServiceLocator for improved functionality.
    /// Consider using StaticServiceLocator.Resolve&lt;T&gt;() directly.
    /// </remarks>
    [Obsolete("Use StaticServiceLocator.Resolve<T>() instead for better error handling. This will be removed in version 1.0.0", false)]
    public static T GetRequiredService<T>() where T : class
    {
        if (StaticServiceLocator.IsInitialized)
        {
            return StaticServiceLocator.Resolve<T>();
        }

        if (_provider == null)
            throw new InvalidOperationException("StaticServiceProvider.Provider is not set.");

        var service = _provider.GetService(typeof(T)) as T;
        if (service == null)
            throw new InvalidOperationException($"Service of type {typeof(T).Name} could not be resolved.");
        
        return service;
    }
}
