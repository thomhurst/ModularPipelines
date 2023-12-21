using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql")]
public class AzMysqlServer
{
    public AzMysqlServer(
        AzMysqlServerAdAdmin adAdmin,
        AzMysqlServerConfiguration configuration,
        AzMysqlServerFirewallRule firewallRule,
        AzMysqlServerKey key,
        AzMysqlServerPrivateEndpointConnection privateEndpointConnection,
        AzMysqlServerPrivateLinkResource privateLinkResource,
        AzMysqlServerReplica replica,
        AzMysqlServerVnetRule vnetRule,
        ICommand internalCommand
    )
    {
        AdAdmin = adAdmin;
        Configuration = configuration;
        FirewallRule = firewallRule;
        Key = key;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Replica = replica;
        VnetRule = vnetRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMysqlServerAdAdmin AdAdmin { get; }

    public AzMysqlServerConfiguration Configuration { get; }

    public AzMysqlServerFirewallRule FirewallRule { get; }

    public AzMysqlServerKey Key { get; }

    public AzMysqlServerPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzMysqlServerPrivateLinkResource PrivateLinkResource { get; }

    public AzMysqlServerReplica Replica { get; }

    public AzMysqlServerVnetRule VnetRule { get; }

    public async Task<CommandResult> Create(AzMysqlServerCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzMysqlServerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerDeleteOptions(), token);
    }

    public async Task<CommandResult> Georestore(AzMysqlServerGeorestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMysqlServerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerListOptions(), token);
    }

    public async Task<CommandResult> ListSkus(AzMysqlServerListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzMysqlServerRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerRestartOptions(), token);
    }

    public async Task<CommandResult> Restore(AzMysqlServerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMysqlServerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzMysqlServerShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> Start(AzMysqlServerStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzMysqlServerStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzMysqlServerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerUpdateOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzMysqlServerUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzMysqlServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerWaitOptions(), token);
    }
}