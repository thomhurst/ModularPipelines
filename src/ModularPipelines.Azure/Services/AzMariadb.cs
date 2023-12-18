using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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