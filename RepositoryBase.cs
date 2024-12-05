using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serenity;
using Serenity.Data;

namespace Idevs;

public class RepositoryBase<T>
{
    protected IServiceProvider ServiceProvider { get; }
    protected ILogger<T> ExceptionLog { get; }
    protected ISqlConnections SqlConnections { get; }
    protected ITextLocalizer Localizer { get; }

    protected SqlQuery SqlQuery => new SqlQuery();
    protected SqlInsert SqlInsert(string tableName) => new SqlInsert(tableName);
    protected SqlUpdate SqlUpdate(string tableName) => new SqlUpdate(tableName);
    protected SqlDelete SqlDelete(string tableName) => new SqlDelete(tableName);

    public RepositoryBase(IServiceProvider serviceProvider, ILogger<T> logger)
    {
        ServiceProvider = serviceProvider;
        var scoped = serviceProvider.CreateScope();
        SqlConnections = scoped.ServiceProvider.GetRequiredService<ISqlConnections>();
        ExceptionLog = logger;
        Localizer = scoped.ServiceProvider.GetRequiredService<ITextLocalizer>();
    }

    protected IDbConnection Connection => SqlConnections.NewByKey("Default");
}