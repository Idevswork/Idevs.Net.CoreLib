using System.Data;
using Serenity;
using Serenity.Abstractions;
using Serenity.Data;

namespace Idevs.Repositories;

public class RepositoryBase
{
    protected ISqlConnections SqlConnections { get; }
    protected IDbConnection Connection => SqlConnections.NewByKey("Default");
    protected IExceptionLogger ExceptionLog { get; }
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
}