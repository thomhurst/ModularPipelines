using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb")]
public class AzMariadbServer
{
    public AzMariadbServer(
        AzMariadbServerConfiguration configuration,
        AzMariadbServerFirewallRule firewallRule,
        AzMariadbServerPrivateEndpointConnection privateEndpointConnection,
        AzMariadbServerPrivateLinkResource privateLinkResource,
        AzMariadbServerReplica replica,
        AzMariadbServerVnetRule vnetRule,
        ICommand internalCommand
    )
    {
        Configuration = configuration;
        FirewallRule = firewallRule;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Replica = replica;
        VnetRule = vnetRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMariadbServerConfiguration Configuration { get; }

    public AzMariadbServerFirewallRule FirewallRule { get; }

    public AzMariadbServerPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzMariadbServerPrivateLinkResource PrivateLinkResource { get; }

    public AzMariadbServerReplica Replica { get; }

    public AzMariadbServerVnetRule VnetRule { get; }

    public async Task<CommandResult> Create(AzMariadbServerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMariadbServerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Georestore(AzMariadbServerGeorestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMariadbServerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzMariadbServerListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzMariadbServerRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzMariadbServerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMariadbServerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzMariadbServerShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> Start(AzMariadbServerStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzMariadbServerStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzMariadbServerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMariadbServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerWaitOptions(), token);
    }
}