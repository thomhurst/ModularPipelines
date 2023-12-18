using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDedicatedHsmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDedicatedHsmListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmListOptions(), token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependencyEndpoint(AzDedicatedHsmListOutboundNetworkDependencyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDedicatedHsmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDedicatedHsmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDedicatedHsmWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDedicatedHsmWaitOptions(), token);
    }
}