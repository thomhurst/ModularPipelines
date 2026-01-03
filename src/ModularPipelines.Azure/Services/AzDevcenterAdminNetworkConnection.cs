using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin")]
public class AzDevcenterAdminNetworkConnection
{
    public AzDevcenterAdminNetworkConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDevcenterAdminNetworkConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminNetworkConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminNetworkConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListHealthCheck(AzDevcenterAdminNetworkConnectionListHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependenciesEndpoint(AzDevcenterAdminNetworkConnectionListOutboundNetworkDependenciesEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RunHealthCheck(AzDevcenterAdminNetworkConnectionRunHealthCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionRunHealthCheckOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminNetworkConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowHealthCheck(AzDevcenterAdminNetworkConnectionShowHealthCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionShowHealthCheckOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminNetworkConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminNetworkConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionWaitOptions(), cancellationToken: token);
    }
}