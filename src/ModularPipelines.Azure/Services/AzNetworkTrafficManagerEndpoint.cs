using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager")]
public class AzNetworkTrafficManagerEndpoint
{
    public AzNetworkTrafficManagerEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkTrafficManagerEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkTrafficManagerEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkTrafficManagerEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkTrafficManagerEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowGeographicHierarchy(AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkTrafficManagerEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointUpdateOptions(), cancellationToken: token);
    }
}