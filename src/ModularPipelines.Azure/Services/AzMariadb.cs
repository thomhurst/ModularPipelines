using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzMariadb
{
    public AzMariadb(
        AzMariadbDb db,
        AzMariadbServer server,
        AzMariadbServerLogs serverLogs
    )
    {
        Db = db;
        Server = server;
        ServerLogs = serverLogs;
    }

    public AzMariadbDb Db { get; }

    public AzMariadbServer Server { get; }

    public AzMariadbServerLogs ServerLogs { get; }
}