using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkTrafficManagerEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkTrafficManagerEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkTrafficManagerEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointShowOptions(), token);
    }

    public async Task<CommandResult> ShowGeographicHierarchy(AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointShowGeographicHierarchyOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkTrafficManagerEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkTrafficManagerEndpointUpdateOptions(), token);
    }
}