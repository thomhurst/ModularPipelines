using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevcenterAdminNetworkConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevcenterAdminNetworkConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHealthCheck(AzDevcenterAdminNetworkConnectionListHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependenciesEndpoint(AzDevcenterAdminNetworkConnectionListOutboundNetworkDependenciesEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RunHealthCheck(AzDevcenterAdminNetworkConnectionRunHealthCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionRunHealthCheckOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevcenterAdminNetworkConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionShowOptions(), token);
    }

    public async Task<CommandResult> ShowHealthCheck(AzDevcenterAdminNetworkConnectionShowHealthCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionShowHealthCheckOptions(), token);
    }

    public async Task<CommandResult> Update(AzDevcenterAdminNetworkConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDevcenterAdminNetworkConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevcenterAdminNetworkConnectionWaitOptions(), token);
    }
}

