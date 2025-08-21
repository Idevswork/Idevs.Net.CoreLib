using System.Collections.Concurrent;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Idevs;

/// <summary>
/// Static service locator that supports both Autofac and traditional DI containers
/// </summary>
/// <remarks>
/// This class provides static access to services for scenarios where dependency injection
/// is not feasible. Use with caution and prefer proper dependency injection when possible.
/// </remarks>
public static class StaticServiceLocator
{
    private static readonly object _lock = new();
    private static IServiceProvider? _serviceProvider;
    private static ILifetimeScope? _autofacContainer;
    private static readonly ConcurrentDictionary<Type, object> _singletonCache = new();
    private static bool _isInitialized = false;
    private static bool _useAutofac = false;

    /// <summary>
    /// Gets a value indicating whether the service locator has been initialized
    /// </summary>
    public static bool IsInitialized => _isInitialized;

    /// <summary>
    /// Gets a value indicating whether Autofac is being used as the container
    /// </summary>
    public static bool IsUsingAutofac => _useAutofac;

    /// <summary>
    /// Initializes the service locator with an Autofac container
    /// </summary>
    /// <param name="container">The Autofac container</param>
    /// <exception cref="ArgumentNullException">Thrown when container is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when already initialized with a different type</exception>
    public static void Initialize(ILifetimeScope container)
    {
        if (container == null)
            throw new ArgumentNullException(nameof(container));

        lock (_lock)
        {
            if (_isInitialized && !_useAutofac)
                throw new InvalidOperationException("StaticServiceLocator is already initialized with a traditional service provider. Cannot switch to Autofac.");

            _autofacContainer = container;
            _serviceProvider = null;
            _useAutofac = true;
            _isInitialized = true;
            _singletonCache.Clear();
        }
    }

    /// <summary>
    /// Initializes the service locator with a traditional service provider
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <exception cref="ArgumentNullException">Thrown when serviceProvider is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when already initialized with a different type</exception>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        lock (_lock)
        {
            if (_isInitialized && _useAutofac)
                throw new InvalidOperationException("StaticServiceLocator is already initialized with Autofac. Cannot switch to traditional service provider.");

            _serviceProvider = serviceProvider;
            _autofacContainer = null;
            _useAutofac = false;
            _isInitialized = true;
            _singletonCache.Clear();
        }
    }

    /// <summary>
    /// Resolves a service of the specified type
    /// </summary>
    /// <typeparam name="T">The type of service to resolve</typeparam>
    /// <returns>The resolved service instance</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service locator is not initialized</exception>
    /// <exception cref="InvalidOperationException">Thrown when the service cannot be resolved</exception>
    public static T Resolve<T>() where T : class
    {
        EnsureInitialized();

        try
        {
            if (_useAutofac)
            {
                return _autofacContainer!.Resolve<T>();
            }
            else
            {
                var service = _serviceProvider!.GetService<T>();
                if (service == null)
                    throw new InvalidOperationException($"Service of type {typeof(T).Name} could not be resolved.");
                return service;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to resolve service of type {typeof(T).Name}.", ex);
        }
    }

    /// <summary>
    /// Resolves a service of the specified type
    /// </summary>
    /// <param name="serviceType">The type of service to resolve</param>
    /// <returns>The resolved service instance</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service locator is not initialized</exception>
    /// <exception cref="InvalidOperationException">Thrown when the service cannot be resolved</exception>
    public static object Resolve(Type serviceType)
    {
        EnsureInitialized();

        try
        {
            if (_useAutofac)
            {
                return _autofacContainer!.Resolve(serviceType);
            }
            else
            {
                var service = _serviceProvider!.GetService(serviceType);
                if (service == null)
                    throw new InvalidOperationException($"Service of type {serviceType.Name} could not be resolved.");
                return service;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to resolve service of type {serviceType.Name}.", ex);
        }
    }

    /// <summary>
    /// Tries to resolve a service of the specified type
    /// </summary>
    /// <typeparam name="T">The type of service to resolve</typeparam>
    /// <returns>The resolved service instance, or null if not found</returns>
    public static T? TryResolve<T>() where T : class
    {
        if (!_isInitialized)
            return null;

        try
        {
            if (_useAutofac)
            {
                return _autofacContainer!.ResolveOptional<T>();
            }
            else
            {
                return _serviceProvider!.GetService<T>();
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to resolve a service of the specified type
    /// </summary>
    /// <param name="serviceType">The type of service to resolve</param>
    /// <returns>The resolved service instance, or null if not found</returns>
    public static object? TryResolve(Type serviceType)
    {
        if (!_isInitialized)
            return null;

        try
        {
            if (_useAutofac)
            {
                return _autofacContainer!.ResolveOptional(serviceType);
            }
            else
            {
                return _serviceProvider!.GetService(serviceType);
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Resolves a singleton service and caches it for subsequent calls
    /// </summary>
    /// <typeparam name="T">The type of service to resolve</typeparam>
    /// <returns>The cached singleton instance</returns>
    /// <remarks>
    /// Use this method for services that you know are registered as singletons
    /// and want to cache the result for performance reasons.
    /// </remarks>
    public static T ResolveSingleton<T>() where T : class
    {
        var serviceType = typeof(T);
        
        if (_singletonCache.TryGetValue(serviceType, out var cachedService))
        {
            return (T)cachedService;
        }

        var service = Resolve<T>();
        _singletonCache.TryAdd(serviceType, service);
        return service;
    }

    /// <summary>
    /// Creates a new service scope for scoped service resolution
    /// </summary>
    /// <returns>A disposable service scope</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service locator is not initialized</exception>
    public static IServiceScope CreateScope()
    {
        EnsureInitialized();

        if (_useAutofac)
        {
            var scope = _autofacContainer!.BeginLifetimeScope();
            return new AutofacServiceScope(scope);
        }
        else
        {
            return _serviceProvider!.CreateScope();
        }
    }

    /// <summary>
    /// Resets the service locator, clearing all cached services and initialization
    /// </summary>
    /// <remarks>
    /// This method is primarily intended for testing scenarios.
    /// Use with caution in production code.
    /// </remarks>
    public static void Reset()
    {
        lock (_lock)
        {
            _serviceProvider = null;
            _autofacContainer = null;
            _isInitialized = false;
            _useAutofac = false;
            _singletonCache.Clear();
        }
    }

    private static void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException(
                "StaticServiceLocator is not initialized. " +
                "Call Initialize() with either an IServiceProvider or ILifetimeScope before using.");
        }
    }

    /// <summary>
    /// Wrapper class to adapt Autofac's ILifetimeScope to IServiceScope
    /// </summary>
    private class AutofacServiceScope : IServiceScope
    {
        private readonly ILifetimeScope _scope;
        private bool _disposed = false;

        public AutofacServiceScope(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            ServiceProvider = new AutofacServiceProvider(_scope);
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                _scope.Dispose();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Wrapper class to adapt Autofac's ILifetimeScope to IServiceProvider
    /// </summary>
    private class AutofacServiceProvider : IServiceProvider
    {
        private readonly ILifetimeScope _scope;

        public AutofacServiceProvider(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public object? GetService(Type serviceType)
        {
            return _scope.ResolveOptional(serviceType);
        }

        public object GetRequiredService(Type serviceType)
        {
            return _scope.Resolve(serviceType);
        }
    }
}
