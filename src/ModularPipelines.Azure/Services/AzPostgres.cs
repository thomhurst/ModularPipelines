using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPostgres
{
    public AzPostgres(
        AzPostgresDb db,
        AzPostgresFlexibleServer flexibleServer,
        AzPostgresServer server,
        AzPostgresServerArc serverArc,
        AzPostgresServerLogs serverLogs,
        ICommand internalCommand
    )
    {
        Db = db;
        FlexibleServer = flexibleServer;
        Server = server;
        ServerArc = serverArc;
        ServerLogs = serverLogs;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPostgresDb Db { get; }

    public AzPostgresFlexibleServer FlexibleServer { get; }

    public AzPostgresServer Server { get; }

    public AzPostgresServerArc ServerArc { get; }

    public AzPostgresServerLogs ServerLogs { get; }

    public async Task<CommandResult> Down(AzPostgresDownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresDownOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzPostgresShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> Up(AzPostgresUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresUpOptions(), token);
    }
}