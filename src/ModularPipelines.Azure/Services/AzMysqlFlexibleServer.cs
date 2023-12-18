using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql")]
public class AzMysqlFlexibleServer
{
    public AzMysqlFlexibleServer(
        AzMysqlFlexibleServerAdAdmin adAdmin,
        AzMysqlFlexibleServerBackup backup,
        AzMysqlFlexibleServerDb db,
        AzMysqlFlexibleServerDeploy deploy,
        AzMysqlFlexibleServerExport export,
        AzMysqlFlexibleServerFirewallRule firewallRule,
        AzMysqlFlexibleServerGtid gtid,
        AzMysqlFlexibleServerIdentity identity,
        AzMysqlFlexibleServerImport import,
        AzMysqlFlexibleServerParameter parameter,
        AzMysqlFlexibleServerReplica replica,
        AzMysqlFlexibleServerServerLogs serverLogs,
        ICommand internalCommand
    )
    {
        AdAdmin = adAdmin;
        Backup = backup;
        Db = db;
        Deploy = deploy;
        Export = export;
        FirewallRule = firewallRule;
        Gtid = gtid;
        Identity = identity;
        Import = import;
        Parameter = parameter;
        Replica = replica;
        ServerLogs = serverLogs;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMysqlFlexibleServerAdAdmin AdAdmin { get; }

    public AzMysqlFlexibleServerBackup Backup { get; }

    public AzMysqlFlexibleServerDb Db { get; }

    public AzMysqlFlexibleServerDeploy Deploy { get; }

    public AzMysqlFlexibleServerExport Export { get; }

    public AzMysqlFlexibleServerFirewallRule FirewallRule { get; }

    public AzMysqlFlexibleServerGtid Gtid { get; }

    public AzMysqlFlexibleServerIdentity Identity { get; }

    public AzMysqlFlexibleServerImport Import { get; }

    public AzMysqlFlexibleServerParameter Parameter { get; }

    public AzMysqlFlexibleServerReplica Replica { get; }

    public AzMysqlFlexibleServerServerLogs ServerLogs { get; }

    public async Task<CommandResult> Connect(AzMysqlFlexibleServerConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMysqlFlexibleServerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMysqlFlexibleServerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Execute(AzMysqlFlexibleServerExecuteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GeoRestore(AzMysqlFlexibleServerGeoRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMysqlFlexibleServerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzMysqlFlexibleServerListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzMysqlFlexibleServerRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzMysqlFlexibleServerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMysqlFlexibleServerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowConnectionString(AzMysqlFlexibleServerShowConnectionStringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzMysqlFlexibleServerStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzMysqlFlexibleServerStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzMysqlFlexibleServerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(AzMysqlFlexibleServerUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzMysqlFlexibleServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlFlexibleServerWaitOptions(), token);
    }
}