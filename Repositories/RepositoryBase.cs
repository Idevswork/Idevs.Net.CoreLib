using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serenity;
using Serenity.Abstractions;
using Serenity.Data;

namespace Idevs.Repositories;

#if NET6_0
public class RepositoryBase
{
    protected IExceptionLogger ExceptionLog { get; }
    protected ISqlConnections SqlConnections { get; }
    protected ITextLocalizer Localizer { get; }

    protected SqlQuery SqlQuery => new SqlQuery().Dialect(SqlServer2012Dialect.Instance);
    protected SqlInsert SqlInsert(string tableName) => new SqlInsert(tableName).Dialect(SqlServer2012Dialect.Instance);
    protected SqlUpdate SqlUpdate(string tableName) => new SqlUpdate(tableName).Dialect(SqlServer2012Dialect.Instance);
    protected SqlDelete SqlDelete(string tableName) => new SqlDelete(tableName);

    public RepositoryBase(ISqlConnections sqlConnections, IExceptionLogger exceptionLogger, ITextLocalizer localizer)
    {
        SqlConnections = sqlConnections
                         ?? throw new ArgumentNullException(nameof(sqlConnections));
        ExceptionLog = exceptionLogger
                       ?? throw new ArgumentNullException(nameof(exceptionLogger));
        Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }
#else
public class RepositoryBase<T>
{
    protected IServiceProvider ServiceProvider { get; }
    protected Microsoft.Extensions.Logging.ILogger<T> ExceptionLog { get; }
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
#endif

    protected IDbConnection Connection => SqlConnections.NewByKey("Default");
}
