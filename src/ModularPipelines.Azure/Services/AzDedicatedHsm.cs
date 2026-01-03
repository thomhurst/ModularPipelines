using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDedicatedHsm
{
    public AzDedicatedHsm(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDedicatedHsmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDedicatedHsmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDedicatedHsmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependencyEndpoint(AzDedicatedHsmListOutboundNetworkDependencyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDedicatedHsmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDedicatedHsmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDedicatedHsmWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmWaitOptions(), cancellationToken: token);
    }
}