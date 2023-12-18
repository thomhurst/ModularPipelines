using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres")]
public class AzPostgresFlexibleServer
{
    public AzPostgresFlexibleServer(
        AzPostgresFlexibleServerAdAdmin adAdmin,
        AzPostgresFlexibleServerAdvancedThreatProtectionSetting advancedThreatProtectionSetting,
        AzPostgresFlexibleServerBackup backup,
        AzPostgresFlexibleServerDb db,
        AzPostgresFlexibleServerDeploy deploy,
        AzPostgresFlexibleServerFirewallRule firewallRule,
        AzPostgresFlexibleServerIdentity identity,
        AzPostgresFlexibleServerMigration migration,
        AzPostgresFlexibleServerParameter parameter,
        AzPostgresFlexibleServerReplica replica,
        ICommand internalCommand
    )
    {
        AdAdmin = adAdmin;
        AdvancedThreatProtectionSetting = advancedThreatProtectionSetting;
        Backup = backup;
        Db = db;
        Deploy = deploy;
        FirewallRule = firewallRule;
        Identity = identity;
        Migration = migration;
        Parameter = parameter;
        Replica = replica;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPostgresFlexibleServerAdAdmin AdAdmin { get; }

    public AzPostgresFlexibleServerAdvancedThreatProtectionSetting AdvancedThreatProtectionSetting { get; }

    public AzPostgresFlexibleServerBackup Backup { get; }

    public AzPostgresFlexibleServerDb Db { get; }

    public AzPostgresFlexibleServerDeploy Deploy { get; }

    public AzPostgresFlexibleServerFirewallRule FirewallRule { get; }

    public AzPostgresFlexibleServerIdentity Identity { get; }

    public AzPostgresFlexibleServerMigration Migration { get; }

    public AzPostgresFlexibleServerParameter Parameter { get; }

    public AzPostgresFlexibleServerReplica Replica { get; }

    public async Task<CommandResult> Connect(AzPostgresFlexibleServerConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzPostgresFlexibleServerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPostgresFlexibleServerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Execute(AzPostgresFlexibleServerExecuteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GeoRestore(AzPostgresFlexibleServerGeoRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPostgresFlexibleServerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzPostgresFlexibleServerListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzPostgresFlexibleServerRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzPostgresFlexibleServerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReviveDropped(AzPostgresFlexibleServerReviveDroppedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPostgresFlexibleServerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowConnectionString(AzPostgresFlexibleServerShowConnectionStringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzPostgresFlexibleServerStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzPostgresFlexibleServerStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPostgresFlexibleServerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(AzPostgresFlexibleServerUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzPostgresFlexibleServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresFlexibleServerWaitOptions(), token);
    }
}