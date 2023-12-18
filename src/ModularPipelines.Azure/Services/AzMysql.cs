using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzMysql
{
    public AzMysql(
        AzMysqlDb db,
        AzMysqlFlexibleServer flexibleServer,
        AzMysqlServer server,
        AzMysqlServerLogs serverLogs,
        ICommand internalCommand
    )
    {
        Db = db;
        FlexibleServer = flexibleServer;
        Server = server;
        ServerLogs = serverLogs;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMysqlDb Db { get; }

    public AzMysqlFlexibleServer FlexibleServer { get; }

    public AzMysqlServer Server { get; }

    public AzMysqlServerLogs ServerLogs { get; }

    public async Task<CommandResult> Down(AzMysqlDownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlDownOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzMysqlShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> Up(AzMysqlUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlUpOptions(), token);
    }
}