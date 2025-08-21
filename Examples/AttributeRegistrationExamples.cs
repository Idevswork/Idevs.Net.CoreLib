using Idevs.ComponentModel;
using Idevs.ComponentModels.Standard;

namespace Idevs.Examples;

/// <summary>
/// Examples demonstrating both legacy and standard service registration attributes
/// </summary>

// ===== LEGACY ATTRIBUTES (Backward Compatibility) =====

/// <summary>
/// Example service using legacy ScopedRegistrationAttribute
/// </summary>
[ScopedRegistration]
public class LegacyOrderService : ILegacyOrderService
{
    public void ProcessOrder(string orderId)
    {
        // Implementation here
    }
}

public interface ILegacyOrderService
{
    void ProcessOrder(string orderId);
}

/// <summary>
/// Example service using legacy SingletonRegistrationAttribute
/// </summary>
[SingletonRegiatration] // Note: there's a typo in the original attribute name
public class LegacyCacheService : ILegacyCacheService
{
    public void CacheData(string key, object data)
    {
        // Implementation here
    }
}

public interface ILegacyCacheService
{
    void CacheData(string key, object data);
}

/// <summary>
/// Example service using legacy TransientRegistrationAttribute
/// </summary>
[TransientRegistration]
public class LegacyEmailService : ILegacyEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        // Implementation here
    }
}

public interface ILegacyEmailService
{
    void SendEmail(string to, string subject, string body);
}

// ===== STANDARD ATTRIBUTES (New and Enhanced) =====

/// <summary>
/// Example service using standard ScopedAttribute with default interface discovery
/// </summary>
[Scoped]
public class OrderService : IOrderService
{
    public void ProcessOrder(string orderId)
    {
        // Implementation here
    }
}

public interface IOrderService
{
    void ProcessOrder(string orderId);
}

/// <summary>
/// Example service using standard SingletonAttribute with explicit service type
/// </summary>
[Singleton(ServiceType = typeof(ICacheService))]
public class CacheService : ICacheService
{
    public void CacheData(string key, object data)
    {
        // Implementation here
    }

    public T GetData<T>(string key)
    {
        // Implementation here
        return default(T)!;
    }
}

public interface ICacheService
{
    void CacheData(string key, object data);
    T GetData<T>(string key);
}

/// <summary>
/// Example service using standard TransientAttribute with service key for named registration
/// </summary>
[Transient(ServiceKey = "smtp")]
public class SmtpEmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        // SMTP implementation
    }
}

/// <summary>
/// Alternative email service with different key
/// </summary>
[Transient(ServiceKey = "sendgrid")]
public class SendGridEmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        // SendGrid implementation
    }
}

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}

/// <summary>
/// Example service with self-registration (no interface required)
/// </summary>
[Scoped(AllowSelfRegistration = true)]
public class UtilityService
{
    public void DoUtilityWork()
    {
        // Implementation here
    }
}

/// <summary>
/// Example service with multiple interfaces - using explicit service type
/// </summary>
[Singleton(ServiceType = typeof(IDataProcessor))]
public class DataProcessingService : IDataProcessor, IDataValidator
{
    public void ProcessData(object data)
    {
        // Processing implementation
    }

    public bool ValidateData(object data)
    {
        // Validation implementation
        return true;
    }
}

public interface IDataProcessor
{
    void ProcessData(object data);
}

public interface IDataValidator
{
    bool ValidateData(object data);
}

/// <summary>
/// Example showing how the enhanced registration handles edge cases
/// </summary>
public class RegistrationExamples
{
    public static void ShowRegistrationCapabilities()
    {
        // Legacy attributes work exactly as before:
        // - ScopedRegistration -> Scoped lifetime, I{ClassName} interface
        // - SingletonRegistration -> Singleton lifetime, I{ClassName} interface  
        // - TransientRegistration -> Transient lifetime, I{ClassName} interface

        // Standard attributes provide more flexibility:
        // - [Scoped] -> Scoped lifetime, auto-discover interface
        // - [Scoped(ServiceType = typeof(IMyService))] -> Explicit service type
        // - [Scoped(ServiceKey = "mykey")] -> Named registration (Autofac only)
        // - [Scoped(AllowSelfRegistration = true)] -> Register as concrete type if no interface

        // Both attribute types are supported simultaneously for maximum compatibility
    }
}

/// <summary>
/// Example showing practical usage patterns
/// </summary>
public static class UsagePatterns
{
    // Pattern 1: Legacy attributes for existing code (no changes needed)
    [ScopedRegistration]
    public class ExistingService : IExistingService
    {
        public void DoWork() { }
    }

    // Pattern 2: Standard attributes for new code with more control
    [Scoped(ServiceKey = "primary")]
    public class PrimaryService : IMyService
    {
        public void Execute() { }
    }

    [Scoped(ServiceKey = "secondary")]
    public class SecondaryService : IMyService
    {
        public void Execute() { }
    }

    // Pattern 3: Self-registration for concrete classes
    [Transient(AllowSelfRegistration = true)]
    public class UtilityHelper
    {
        public void Help() { }
    }
}

// Supporting interfaces
public interface IExistingService
{
    void DoWork();
}

public interface IMyService
{
    void Execute();
}
