using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres")]
public class AzPostgresServer
{
    public AzPostgresServer(
        AzPostgresServerAdAdmin adAdmin,
        AzPostgresServerConfiguration configuration,
        AzPostgresServerFirewallRule firewallRule,
        AzPostgresServerKey key,
        AzPostgresServerPrivateEndpointConnection privateEndpointConnection,
        AzPostgresServerPrivateLinkResource privateLinkResource,
        AzPostgresServerReplica replica,
        AzPostgresServerVnetRule vnetRule,
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

    public AzPostgresServerAdAdmin AdAdmin { get; }

    public AzPostgresServerConfiguration Configuration { get; }

    public AzPostgresServerFirewallRule FirewallRule { get; }

    public AzPostgresServerKey Key { get; }

    public AzPostgresServerPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzPostgresServerPrivateLinkResource PrivateLinkResource { get; }

    public AzPostgresServerReplica Replica { get; }

    public AzPostgresServerVnetRule VnetRule { get; }

    public async Task<CommandResult> Create(AzPostgresServerCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzPostgresServerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerDeleteOptions(), token);
    }

    public async Task<CommandResult> Georestore(AzPostgresServerGeorestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPostgresServerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerListOptions(), token);
    }

    public async Task<CommandResult> ListSkus(AzPostgresServerListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzPostgresServerRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerRestartOptions(), token);
    }

    public async Task<CommandResult> Restore(AzPostgresServerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPostgresServerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerShowOptions(), token);
    }

    public async Task<CommandResult> ShowConnectionString(AzPostgresServerShowConnectionStringOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerShowConnectionStringOptions(), token);
    }

    public async Task<CommandResult> Update(AzPostgresServerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPostgresServerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerWaitOptions(), token);
    }
}