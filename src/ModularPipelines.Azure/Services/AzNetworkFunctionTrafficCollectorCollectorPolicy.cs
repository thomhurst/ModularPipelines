using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network-function", "traffic-collector")]
public class AzNetworkFunctionTrafficCollectorCollectorPolicy
{
    public AzNetworkFunctionTrafficCollectorCollectorPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkFunctionTrafficCollectorCollectorPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkFunctionTrafficCollectorCollectorPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorCollectorPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkFunctionTrafficCollectorCollectorPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkFunctionTrafficCollectorCollectorPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorCollectorPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkFunctionTrafficCollectorCollectorPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorCollectorPolicyUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkFunctionTrafficCollectorCollectorPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkFunctionTrafficCollectorCollectorPolicyWaitOptions(), token);
    }
}